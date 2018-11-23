//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;

//namespace Service
//{
//    public class Win32
//    {
//        public const int MEM_RELEASE = 0x8000;
//        public const int MEM_COMMIT = 0x1000;
//        public const int PAGE_READWRITE = 0x04;
//        public const int WM_USER = 0x0400;
//        public const int TB_GETBUTTON = (WM_USER + 23);
//        public const int TB_BUTTONCOUNT = (WM_USER + 24);
//        public const int TB_HIDEBUTTON = (WM_USER + 4);
//        public const int STILL_ACTIVE = 0x0103;
//        [StructLayout(LayoutKind.Sequential)]
//        public class TRAYDATA
//        {
//            public IntPtr hwnd;
//            public uint uID;
//            public uint uCallbackMessage;
//            public int Reserved0;
//            public int Reserved1;
//            public IntPtr hIcon;                //托盘图标的句柄 
//        }
//        [StructLayout(LayoutKind.Sequential, Pack = 1)]
//        public class TBBUTTON
//        {
//            public int iBitmap;
//            public int idCommand;
//            public byte fsState;
//            public byte fsStyle;
//            public byte bReserved0;
//            public byte bReserved1;
//            public int dwData;
//            public int iString;
//        }
//        [Flags]
//        public enum ProcessAccessFlags : uint
//        {
//            All = 0x001F0FFF,
//            Terminate = 0x00000001,
//            CreateThread = 0x00000002,
//            VMOperation = 0x00000008,
//            VMRead = 0x00000010,
//            VMWrite = 0x00000020,
//            DupHandle = 0x00000040,
//            SetInformation = 0x00000200,
//            QueryInformation = 0x00000400,
//            Synchronize = 0x00100000
//        }
//        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
//        public static extern int VirtualAllocEx(IntPtr hProcess,
//            int lpAddress,
//            int dwSize,
//            int flAllocationType,
//            int flProtect);
//        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
//        public static extern bool VirtualFreeEx(IntPtr hProcess, int lpAddress,
//           int dwSize, uint dwFreeType);
//        [DllImport("kernel32.dll", SetLastError = true)]
//        public static extern bool CloseHandle(IntPtr hHandle);
//        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
//        public static extern int SendMessage(IntPtr hWnd, Int32 Msg, Int32 wParam, Int32 lParam);
//        [DllImport("kernel32.dll", SetLastError = true, PreserveSig = true)]
//        public static extern int ReadProcessMemory(IntPtr hProcess, int lpBaseAddress,
//             IntPtr lpBuffer, int nSize, out int lpNumberOfBytesRead);
//        [DllImport("kernel32.dll", SetLastError = true, PreserveSig = true)]
//        public static extern int ReadProcessMemory(IntPtr hProcess, int lpBaseAddress,
//             byte[] lpBuffer, int nSize, out int lpNumberOfBytesRead);


//        [DllImport("user32.dll", SetLastError = true)]
//        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
//        [DllImport("user32.dll", SetLastError = true)]
//        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
//        [DllImport("user32.dll")]
//        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, ref int ProcessId);
//        [DllImport("kernel32.dll", SetLastError = true)]
//        public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);
//        [DllImport("psapi.dll", SetLastError = true)]
//        public static extern int GetProcessImageFileName(IntPtr hProcess, StringBuilder lpImageFileName, int nSize);
//        [DllImport("kernel32.dll", SetLastError = true)]
//        public static extern int QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);
//        [DllImport("kernel32.dll", SetLastError = true)]
//        public static extern bool GetExitCodeProcess(IntPtr hProcess, ref int lpExitCode);
//    }

//    public class SysTrayWnd
//    {
//        public struct TrayItemData
//        {
//            public int dwProcessID;
//            public byte fsState;
//            public byte fsStyle;
//            public IntPtr hIcon;
//            public IntPtr hProcess;
//            public IntPtr hWnd;
//            public int idBitmap;
//            public int idCommand;
//            public string lpProcImagePath;
//            public string lpTrayToolTip;
//        }

//        public static IntPtr GetTrayWnd()
//        {
//            IntPtr handle = Win32.FindWindow("Shell_TrayWnd", null);
//            handle = Win32.FindWindowEx(handle, IntPtr.Zero, "TrayNotifyWnd", null);
//            handle = Win32.FindWindowEx(handle, IntPtr.Zero, "SysPager", null);
//            handle = Win32.FindWindowEx(handle, IntPtr.Zero, "ToolbarWindow32", null);
//            return handle;
//        }

//        public static TrayItemData[] GetTrayWndDetail()
//        {
//            SortedList<string, TrayItemData> stlTrayItems = new SortedList<string, TrayItemData>();
//            try
//            {
//                Win32.TBBUTTON tbButtonInfo = new Win32.TBBUTTON();
//                IntPtr hTrayWnd = IntPtr.Zero;
//                IntPtr hTrayProcess = IntPtr.Zero;
//                int iTrayProcessID = 0;
//                int iAllocBaseAddress = 0;
//                int iRet = 0;
//                int iTrayItemCount = 0;

//                hTrayWnd = GetTrayWnd();
//                Win32.GetWindowThreadProcessId(hTrayWnd, ref iTrayProcessID);
//                hTrayProcess = Win32.OpenProcess(
//                (uint)Win32.ProcessAccessFlags.All |
//                (uint)Win32.ProcessAccessFlags.VMOperation |
//                (uint)Win32.ProcessAccessFlags.VMRead |
//                (uint)Win32.ProcessAccessFlags.VMWrite, 0, (uint)iTrayProcessID);


//                iAllocBaseAddress = Win32.VirtualAllocEx(hTrayProcess, 0, Marshal.SizeOf(tbButtonInfo), Win32.MEM_COMMIT, Win32.PAGE_READWRITE);
//                iTrayItemCount = Win32.SendMessage(hTrayWnd, Win32.TB_BUTTONCOUNT, 0, 0);

//                for (int i = 0; i < iTrayItemCount; i)
//                {
//                    try
//                    {
//                        TrayItemData trayItem = new TrayItemData();
//                        Win32.TRAYDATA trayData = new Win32.TRAYDATA();
//                        int iOut = 0;
//                        int dwProcessID = 0;
//                        IntPtr hRelProcess = IntPtr.Zero;
//                        string strTrayToolTip = string.Empty;

//                        iRet = Win32.SendMessage(hTrayWnd, Win32.TB_GETBUTTON, i, iAllocBaseAddress);
//                        IntPtr hButtonInfo = Marshal.AllocHGlobal(Marshal.SizeOf(tbButtonInfo));
//                        IntPtr hTrayData = Marshal.AllocHGlobal(Marshal.SizeOf(trayData));

//                        iRet = Win32.ReadProcessMemory(hTrayProcess, iAllocBaseAddress, hButtonInfo, Marshal.SizeOf(tbButtonInfo), out iOut);
//                        Marshal.PtrToStructure(hButtonInfo, tbButtonInfo);

//                        iRet = Win32.ReadProcessMemory(hTrayProcess, tbButtonInfo.dwData, hTrayData, Marshal.SizeOf(trayData), out iOut);
//                        Marshal.PtrToStructure(hTrayData, trayData);

//                        byte[] bytTextData = new byte[1024];
//                        iRet = Win32.ReadProcessMemory(hTrayProcess, tbButtonInfo.iString, bytTextData, 1024, out iOut);
//                        strTrayToolTip = Encoding.Unicode.GetString(bytTextData);
//                        if (!string.IsNullOrEmpty(strTrayToolTip))
//                        {
//                            int iNullIndex = strTrayToolTip.IndexOf('\0');
//                            strTrayToolTip = strTrayToolTip.Substring(0, iNullIndex);
//                        }

//                        Win32.GetWindowThreadProcessId(trayData.hwnd, ref dwProcessID);
//                        hRelProcess = Win32.OpenProcess((uint)Win32.ProcessAccessFlags.QueryInformation, 0, (uint)dwProcessID);
//                        StringBuilder sbProcImagePath = new StringBuilder(256);
//                        if (hRelProcess != IntPtr.Zero)
//                        {
//                            Win32.GetProcessImageFileName(hRelProcess, sbProcImagePath, sbProcImagePath.Capacity);
//                        }

//                        string strImageFilePath = string.Empty;
//                        if (sbProcImagePath.Length > 0)
//                        {
//                            int iDeviceIndex = sbProcImagePath.ToString().IndexOf("\\", "\\Device\\HarddiskVolume".Length);
//                            string strDevicePath = sbProcImagePath.ToString().Substring(0, iDeviceIndex);
//                            int iStartDisk = (int)'A';
//                            while (iStartDisk <= (int)'Z')
//                            {
//                                StringBuilder sbWindowImagePath = new StringBuilder(256);
//                                iRet = Win32.QueryDosDevice(((char)iStartDisk).ToString()   ":", sbWindowImagePath, sbWindowImagePath.Capacity);
//                                if (iRet != 0)
//                                {
//                                    if (sbWindowImagePath.ToString() == strDevicePath)
//                                    {
//                                        strImageFilePath = ((char)iStartDisk).ToString()   ":"   sbProcImagePath.ToString().Replace(strDevicePath, "");
//                                        break;
//                                    }
//                                }
//                                iStartDisk;
//                            }
//                        }

//                        trayItem.dwProcessID = dwProcessID;
//                        trayItem.fsState = tbButtonInfo.fsState;
//                        trayItem.fsStyle = tbButtonInfo.fsStyle;
//                        trayItem.hIcon = trayData.hIcon;
//                        trayItem.hProcess = hRelProcess;
//                        trayItem.hWnd = trayData.hwnd;
//                        trayItem.idBitmap = tbButtonInfo.iBitmap;
//                        trayItem.idCommand = tbButtonInfo.idCommand;
//                        trayItem.lpProcImagePath = strImageFilePath;
//                        trayItem.lpTrayToolTip = strTrayToolTip;
//                        stlTrayItems[string.Format("{0:d8}", tbButtonInfo.idCommand)] = trayItem;
//                    }
//                    catch { continue; }
//                }

//                Win32.VirtualFreeEx(hTrayProcess, iAllocBaseAddress, Marshal.SizeOf(tbButtonInfo), Win32.MEM_RELEASE);
//                Win32.CloseHandle(hTrayProcess);

//                TrayItemData[] trayItems = new TrayItemData[stlTrayItems.Count];
//                stlTrayItems.Values.CopyTo(trayItems, 0);
//                return trayItems;
//            }
//            catch (SEHException ex)
//            {
//                throw ex;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public static void RefreshTrayWnd()
//        {
//            TrayItemData[] trayItems = GetTrayWndDetail();
//            IntPtr hTrayWnd = GetTrayWnd();
//            for (int i = trayItems.Length - 1; i >= 0; i--)
//            {
//                int iProcessExitCode = 0;
//                Win32.GetExitCodeProcess(trayItems[i].hProcess, ref iProcessExitCode);
//                if (iProcessExitCode != Win32.STILL_ACTIVE)
//                {
//                    //通过隐藏图标来达到刷新的动作
//                    int iRet = Win32.SendMessage(hTrayWnd, Win32.TB_HIDEBUTTON, trayItems[i].idCommand, 0);
//                }
//            }
//        }
//    }
//}
