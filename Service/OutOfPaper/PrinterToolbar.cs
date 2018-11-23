using Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Printing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Service.ShowWindow;

namespace Demo
{
    [Flags]
    public enum ProcessAccess
    {
        PROCESS_VM_OPERATION = 0x0008,
        PROCESS_VM_READ = 0x0010,
        PROCESS_VM_WRITE = 0x0020,
    }

    /// <summary>
    /// 系统托盘图标操作类
    /// </summary>
    public class PrinterToolbar
    {
        public const int WM_USER = 0x400;
        public const int WM_CLOSE = 0x10;
        public const int WM_GETTEXT = 0x000D;
        public const int WM_SETTEXT = 0x000C;

        public const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
        public const int SYNCHRONIZE = 0x100000;
        public const int PROCESS_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF;
        public const int PROCESS_TERMINATE = 0x1;

        public const int PROCESS_VM_OPERATION = 0x8;
        public const int PROCESS_VM_READ = 0x10;
        public const int PROCESS_VM_WRITE = 0x20;

        public const int MEM_RESERVE = 0x2000;
        public const int MEM_COMMIT = 0x1000;
        public const int MEM_RELEASE = 0x8000;

        public const int PAGE_READWRITE = 0x4;

        public const int TB_BUTTONCOUNT = (WM_USER + 24);
        public const int TB_HIDEBUTTON = (WM_USER + 4);
        public const int TB_GETBUTTON = (WM_USER + 23);
        public const int WM_LeftDown = 0x0002;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_SETFOCUS = 0x07;
        public const int WM_LBUTTONDBLCLK = 0x203;
        public const int WM_Click = 0xF5;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int  WM_LBUTTONUP = 0x202;
        public const int WM_Move = 0x0001;
        public const int WM_KILLFOCUS = 0x08;
        public const int WM_SHOWWINDOW = 0x18;
        public const int TB_GETBUTTONTEXT = WM_USER + 75;
        public const int TB_GETBITMAP = (WM_USER + 44);
        public const int TB_DELETEBUTTON = (WM_USER + 22);
        public const int TB_ADDBUTTONS = (WM_USER + 20);
        public const int TB_INSERTBUTTON = (WM_USER + 21);
        public const int TB_ISBUTTONHIDDEN = (WM_USER + 12);
        public const int ILD_NORMAL = 0x0;
        public const int TPM_NONOTIFY = 0x80;

        public const int WS_VISIBLE = 268435456;//窗体可见 
        public const int WS_MINIMIZEBOX = 131072;//有最小化按钮 
        public const int WS_MAXIMIZEBOX = 65536;//有最大化按钮 
        public const int WS_BORDER = 8388608;//窗体有边框 
        public const int GWL_STYLE = (-16);//窗体样式 
        public const int GW_HWNDFIRST = 0;
        public const int GW_HWNDNEXT = 2;
        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;


        [DllImport("user32", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);


        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hWnd, IntPtr hWndChildAfter, string lpClassName, string lpWindowName);

        [DllImport("user32")]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out uint pid);

        [DllImport("Kernel32")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("kernel32", EntryPoint = "CloseHandle")]
        public static extern int CloseHandle(IntPtr hObject);

        [DllImport("Kernel32")]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, int lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32", EntryPoint = "VirtualFreeEx")]
        public static extern int VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, int dwFreeType);

        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref IntPtr lpBuffer, int nSize, int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        public static extern bool ReadProcessMemoryEx(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, int lpNumberOfBytesWritten);


        [DllImport("user32.dll")]
        public extern static int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        /// <summary>
        /// 获取系统托盘图标列表
        /// </summary>
        public static void GetToolBarList()
        {
            EnumNotifyWindow(FindTrayWnd());//非隐藏
            EnumNotifyWindow(FindNotifyIconOverflowWindow());//隐藏
        }


        static IntPtr FindTrayWnd()
        {
            IntPtr hwnd;
            hwnd = FindWindow("Shell_TrayWnd", null);
            hwnd = FindWindowEx(hwnd, IntPtr.Zero, "TrayNotifyWnd", null);
            hwnd = FindWindowEx(hwnd, IntPtr.Zero, "SysPager", null);
            hwnd = FindWindowEx(hwnd, IntPtr.Zero, "ToolbarWindow32", null);

            return hwnd;
        }
        static IntPtr FindNotifyIconOverflowWindow()
        {
            IntPtr hwnd;
            hwnd = FindWindow("NotifyIconOverflowWindow", null);
            hwnd = FindWindowEx(hwnd, IntPtr.Zero, "ToolbarWindow32", null);

            return hwnd;
        }

        static List<WindowInfo> EnumNotifyWindow(IntPtr hWnd)
        {
            List<WindowInfo> iconList = new List<WindowInfo>();
            IntPtr ipHandle = IntPtr.Zero; //图标句柄 
            GetWindowThreadProcessId(hWnd, out uint pid);

            IntPtr hProcess = OpenProcess((uint)(ProcessAccess.PROCESS_VM_OPERATION | ProcessAccess.PROCESS_VM_READ | ProcessAccess.PROCESS_VM_WRITE), false, pid);
            if (hProcess == IntPtr.Zero)
            {
                return iconList;
            }
            IntPtr lAddress = VirtualAllocEx(hProcess, 0, 4096, 0x00001000, 0x40);
            if (lAddress == IntPtr.Zero)
            {
                return iconList;
            }
            int buttons = SendMessage(hWnd, 0x0418, 0, 0);
         
            IntPtr lTextAdr = IntPtr.Zero; //文本内存地址 
            for (int i = 0; i < buttons; i++)
            {
                SendMessage(hWnd, TB_GETBUTTON, i, lAddress);
                
                //读文本地址
                ReadProcessMemory(hProcess, (IntPtr)(lAddress.ToInt32() + 16), ref lTextAdr, 4, 0);
                if (!lTextAdr.Equals(-1))
                {
                    byte[] buff = new byte[1024];

                    ReadProcessMemory(hProcess, lTextAdr, buff, 1024, 0);//读文本 
                    string title = Encoding.Unicode.GetString(buff).Substring(284);

                    // 从字符0处截断
                    int nullindex = title.IndexOf("\0");
                    if (nullindex > 0)
                    {
                        title = title.Substring(0, nullindex);
                    }

                    if (title.Contains("HP LaserJet CP 1025"))
                    {
                        //弹出窗口  TODO:只能双击到第一个图标
                        SendMessage(hWnd, WM_LBUTTONDBLCLK, i, lAddress);
                        //RECT rect = new RECT();
                        //GetWindowRect(hWnd, ref rect);
                        //int width = rect.Right - rect.Left;                        //窗口的宽度
                        //int height = rect.Bottom - rect.Top;                   //窗口的高度
                        //int x = rect.Left;  //zuo                                            
                        //int y = rect.Top;//shang
                        //int a = x+1 ;
                        //int b = y+617 ;
                        //var c = (b << 16) + a;
                        //  SendMessage(hWnd, WM_LBUTTONDBLCLK,0,0);
                   
                        // SendMessage(hWnd, WM_LBUTTONDBLCLK, i, lAddress);

                        //SendMessage(hWnd, WM_LBUTTONDBLCLK, i, lAddress);
                        // SendMessage(hWnd, WM_LBUTTONUP, 0, 0);
                        // SendMessage(hWnd, WM_LBUTTONDOWN, i, lAddress);
                        // SendMessage(hWnd, WM_LBUTTONUP, i, lAddress);
                        //SendMessage(hWnd,WM_KILLFOCUS,i, lAddress);


                        //  IntPtr childHwnd = FindWindow(null, "HP Color Laserjet CP1020 系列警报");   //获得按钮的句柄

                        //StringBuilder s = new StringBuilder(512);
                        //    int x = GetWindowText(childHwnd, s, s.Capacity); //把this.handle换成你需要的句柄           
                        //   var titleStr = x.ToString();

                        //SendMessage(childHwnd, WM_CLOSE, 0, 0);
                    }

                    IntPtr ipHandleAdr = IntPtr.Zero;

                    //读句柄地址 
                    Win32API.ReadProcessMemory(hProcess, (IntPtr)(lAddress.ToInt32() + 12), ref ipHandleAdr, 4, 0);
                    Win32API.ReadProcessMemory(hProcess, ipHandleAdr, ref ipHandle, 4, 0);
               
                    iconList.Add(new WindowInfo(title, ipHandle));
                }
            }
            VirtualFreeEx(hProcess, lAddress, 4096, MEM_RELEASE);
            CloseHandle(hProcess);
            return iconList;
        }
    }
}
