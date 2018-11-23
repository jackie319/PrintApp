using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Service.ShowWindow;

namespace Service
{
    public class Win32API
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
        public const  int WM_SETFOCUS = 0x07;
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

        [DllImport("User32.Dll")]
        public static extern void GetClassName(IntPtr hwnd, StringBuilder s, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true)]
        public static extern void SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "GetDlgItem", SetLastError = true)]
        public static extern IntPtr GetDlgItem(int nID, IntPtr phWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int RegisterWindowMessage(string msg);

        [DllImport("kernel32", EntryPoint = "OpenProcess")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, IntPtr bInheritHandle, IntPtr dwProcessId);

        [DllImport("kernel32", EntryPoint = "CloseHandle")]
        public static extern int CloseHandle(IntPtr hObject);

        [DllImport("user32", EntryPoint = "GetWindowThreadProcessId")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hwnd, ref IntPtr lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);

        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref IntPtr lpBuffer, int nSize, int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        public static extern bool ReadProcessMemoryEx(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, int lpNumberOfBytesWritten);

        [DllImport("kernel32", EntryPoint = "WriteProcessMemory")]
        public static extern int WriteProcessMemory(IntPtr hProcess, ref int lpBaseAddress, ref int lpBuffer, int nSize, ref int lpNumberOfBytesWritten);

        [DllImport("kernel32", EntryPoint = "VirtualAllocEx")]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, int lpAddress, int dwSize, int flAllocationType, int flProtect);

        [DllImport("kernel32", EntryPoint = "VirtualFreeEx")]
        public static extern int VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, int dwFreeType);

        [DllImport("User32.dll")]
        public extern static int GetWindow(int hWnd, int wCmd);

        [DllImport("User32.dll")]
        public extern static int GetWindowLongA(int hWnd, int wIndx);

        [DllImport("user32.dll")]
        public static extern bool GetWindowText(int hWnd, StringBuilder title, int maxBufSize);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public extern static int GetWindowTextLength(IntPtr hWnd);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

    }

    //窗体信息 
    public class WindowInfo
    {
        public WindowInfo(string title, IntPtr handle)
        {
            this._title = title;
            this._handle = handle;
        }

        private string _title;
        private IntPtr _handle;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public IntPtr Handle
        {
            get { return _handle; }
            set { _handle = value; }
        }

        public override string ToString()
        {
            return _handle.ToString() + ":" + _title;
        }
    }

    public class WindowHide
    {
        //获取托盘指针 
        private static IntPtr TrayToolbarWindow32()
        {
            IntPtr h = IntPtr.Zero;
            IntPtr hTemp = IntPtr.Zero;

            h = Win32API.FindWindow("Shell_TrayWnd", null); //托盘容器 
            h = Win32API.FindWindowEx(h, IntPtr.Zero, "TrayNotifyWnd", null);//找到托盘 
            h = Win32API.FindWindowEx(h, IntPtr.Zero, "SysPager", null);

            hTemp = Win32API.FindWindowEx(h, IntPtr.Zero, "ToolbarWindow32", null);

            return hTemp;
        }


        //获取托盘图标列表 
        public static List<WindowInfo> GetIconList()
        {
            List<WindowInfo> iconList = new List<WindowInfo>();
           
            IntPtr pid = IntPtr.Zero;
            IntPtr ipHandle = IntPtr.Zero; //图标句柄 
            IntPtr lTextAdr = IntPtr.Zero; //文本内存地址 

            IntPtr ipTray = TrayToolbarWindow32();

            Win32API.GetWindowThreadProcessId(ipTray, ref pid);
            if (pid.Equals(0)) return iconList;

            IntPtr hProcess = Win32API.OpenProcess(Win32API.PROCESS_ALL_ACCESS | Win32API.PROCESS_VM_OPERATION | Win32API.PROCESS_VM_READ | Win32API.PROCESS_VM_WRITE, IntPtr.Zero, pid);
            IntPtr lAddress = Win32API.VirtualAllocEx(hProcess, 0, 4096, Win32API.MEM_COMMIT, Win32API.PAGE_READWRITE);

            //得到图标个数 
            int lButton = Win32API.SendMessage(ipTray, Win32API.TB_BUTTONCOUNT, 0, 0);

            for (int i = 0; i < lButton; i++)
            {
                Win32API.SendMessage(ipTray, Win32API.TB_GETBUTTON, i, lAddress);

                 Win32API.SendMessage(ipTray, Win32API.WM_SETFOCUS, i, lAddress);
                //RECT rc = new RECT();
                //ShowWindow.GetWindowRect(lAddress, ref rc);
                //int width = rc.Right - rc.Left;                        //窗口的宽度
                //int height = rc.Bottom - rc.Top;                   //窗口的高度
                //int x = rc.Left;
                //int y = rc.Top;


                //MouseFlag.MouseMoveEvent(x,y, 0);
                //读文本地址 
                Win32API.ReadProcessMemory(hProcess, (IntPtr)(lAddress.ToInt32() + 16), ref lTextAdr, 4, 0);

                if (!lTextAdr.Equals(-1))
                {
                    byte[] buff = new byte[1024];

                    Win32API.ReadProcessMemory(hProcess, lTextAdr, buff, 1024, 0);//读文本 
                    string title = System.Text.ASCIIEncoding.Unicode.GetString(buff).Substring(284);
                    //截断
                    title = title.Replace("\\0","").Trim();
                    // 从字符0处截断 
                    int nullindex = title.IndexOf("\0");
                    if (nullindex > 0)
                    {
                        title = title.Substring(0, nullindex);
                    }

                
                    IntPtr ipHandleAdr = IntPtr.Zero;

                    //读句柄地址 
                    Win32API.ReadProcessMemory(hProcess, (IntPtr)(lAddress.ToInt32() + 12), ref ipHandleAdr, 4, 0);
                    Win32API.ReadProcessMemory(hProcess, ipHandleAdr, ref ipHandle, 4, 0);

                    if (title.Replace("\0", "") == "") continue;//不加载空项 
                    iconList.Add(new WindowInfo(title, ipHandle));
                }
            }
            Win32API.VirtualFreeEx(hProcess, lAddress, 4096, Win32API.MEM_RELEASE);
            Win32API.CloseHandle(hProcess);

            return iconList;
        }

        //获取QQ窗体列表 
        public static List<WindowInfo> GetQQWindowList(int Handle)
        {
            const string CLASS_PARENT = "TXGuiFoundation";
            const string CLASS_CHILD = "ATL:006CC4D0";

            List<WindowInfo> list = new List<WindowInfo>();

            StringBuilder title = new StringBuilder(256);
            StringBuilder className = new StringBuilder(256);

            int h = Handle;// (int)Win32API.FindWindow(CLASS_PARENT, ""); 
            while (h != 0)
            {
                //QQ聊天窗体的子窗体 
                IntPtr p = Win32API.FindWindowEx(new IntPtr(h), IntPtr.Zero, CLASS_CHILD, "");

                if (p != IntPtr.Zero)
                {
                    Win32API.GetWindowText(h, title, title.Capacity);//得到窗口的标题 

                    if (title.ToString().Trim() != "")
                        Win32API.GetClassName(new IntPtr(h), className, className.Capacity);

                    if (className.ToString().Trim() != "" && className.ToString() == CLASS_PARENT)
                    {
                        WindowInfo wi = new WindowInfo(title.ToString(), new IntPtr(h));
                        list.Add(wi);
                    }
                }

                h = Win32API.GetWindow(h, Win32API.GW_HWNDNEXT);
            }

            return list;
        }

        //获取QQ窗体列表 
        public static List<WindowInfo> GetMSNWindowList(int Handle)
        {
            const string CLASS_PARENT = "IMWindowClass";
            const string CLASS_CHILD = "IM Window Native WindowBar Class";

            List<WindowInfo> list = new List<WindowInfo>();

            StringBuilder title = new StringBuilder(256);
            StringBuilder className = new StringBuilder(256);

            int h = Handle;// (int)Win32API.FindWindow(CLASS_PARENT, ""); 
            while (h != 0)
            {
                //QQ聊天窗体的子窗体 
                IntPtr p = Win32API.FindWindowEx(new IntPtr(h), IntPtr.Zero, CLASS_CHILD, "");

                if (p != IntPtr.Zero)
                {
                    Win32API.GetWindowText(h, title, title.Capacity);//得到窗口的标题 

                    if (title.ToString().Trim() != "")
                        Win32API.GetClassName(new IntPtr(h), className, className.Capacity);

                    if (className.ToString().Trim() != "" && className.ToString() == CLASS_PARENT)
                    {
                        WindowInfo wi = new WindowInfo(title.ToString(), new IntPtr(h));
                        list.Add(wi);
                    }
                }

                h = Win32API.GetWindow(h, Win32API.GW_HWNDNEXT);
            }

            return list;
        }

        //获取打开的窗体列表 
        public static List<WindowInfo> GetHandleList(int Handle)
        {
            List<WindowInfo> fromInfo = new List<WindowInfo>();

            int handle = Win32API.GetWindow(Handle, Win32API.GW_HWNDFIRST);
            while (handle > 0)
            {
                int IsTask = Win32API.WS_VISIBLE | Win32API.WS_BORDER | Win32API.WS_MAXIMIZEBOX;//窗体可见、有边框、有最大化按钮 
                int lngStyle = Win32API.GetWindowLongA(handle, Win32API.GWL_STYLE);
                bool TaskWindow = ((lngStyle & IsTask) == IsTask);
                if (TaskWindow)
                {
                    int length = Win32API.GetWindowTextLength(new IntPtr(handle));
                    StringBuilder stringBuilder = new StringBuilder(2 * length + 1);
                    Win32API.GetWindowText(handle, stringBuilder, stringBuilder.Capacity);
                    string strTitle = stringBuilder.ToString();
                    if (!string.IsNullOrEmpty(strTitle))
                    {
                        fromInfo.Add(new WindowInfo(strTitle, (IntPtr)handle));
                    }
                    else
                    {
                        fromInfo.Add(new WindowInfo("无标题", (IntPtr)handle));
                    }
                }
                handle = Win32API.GetWindow(handle, Win32API.GW_HWNDNEXT);
            }
            return fromInfo;
        }

        //显示/隐藏所有系统托盘图标 
        public static void SetTrayIconAllVisible(bool visible)
        {
            IntPtr vHandle = TrayToolbarWindow32();
            int vCount = Win32API.SendMessage(vHandle, Win32API.TB_BUTTONCOUNT, 0, 0);
            IntPtr vProcessId = IntPtr.Zero;
            Win32API.GetWindowThreadProcessId(vHandle, ref vProcessId);
            IntPtr vProcess = Win32API.OpenProcess(Win32API.PROCESS_VM_OPERATION | Win32API.PROCESS_VM_READ |
            Win32API.PROCESS_VM_WRITE, IntPtr.Zero, vProcessId);
            IntPtr vPointer = Win32API.VirtualAllocEx(vProcess, (int)IntPtr.Zero, 0x1000,
            Win32API.MEM_RESERVE | Win32API.MEM_COMMIT, Win32API.PAGE_READWRITE);
            char[] vBuffer = new char[256];
            IntPtr pp = Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0);
            uint vNumberOfBytesRead = 0;
            try
            {
                for (int i = 0; i < vCount; i++)
                {
                    Win32API.SendMessage(vHandle, Win32API.TB_GETBUTTONTEXT, i, vPointer.ToInt32());

                    Win32API.ReadProcessMemoryEx(vProcess, vPointer,
                    Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
                    vBuffer.Length * sizeof(char), ref vNumberOfBytesRead);

                    if (visible)
                        Win32API.SendMessage(vHandle, Win32API.TB_HIDEBUTTON, i, 0);
                    else
                        Win32API.SendMessage(vHandle, Win32API.TB_HIDEBUTTON, i, 1);

                }
            }
            finally
            {
                Win32API.VirtualFreeEx(vProcess, vPointer, 0, Win32API.MEM_RELEASE);
                Win32API.CloseHandle(vProcess);
            }
        }

        //显示/隐藏单个系统托盘图标,由参数caption指定图标 
        public static void SetTrayIconVisible(string caption, bool isShow)
        {
            IntPtr vHandle = TrayToolbarWindow32();
            int vCount = Win32API.SendMessage(vHandle, Win32API.TB_BUTTONCOUNT, 0, 0);
            IntPtr vProcessId = IntPtr.Zero;
            Win32API.GetWindowThreadProcessId(vHandle, ref vProcessId);
            IntPtr vProcess = Win32API.OpenProcess(Win32API.PROCESS_VM_OPERATION | Win32API.PROCESS_VM_READ |
            Win32API.PROCESS_VM_WRITE, IntPtr.Zero, vProcessId);
            IntPtr vPointer = Win32API.VirtualAllocEx(vProcess, (int)IntPtr.Zero, 0x1000,
            Win32API.MEM_RESERVE | Win32API.MEM_COMMIT, Win32API.PAGE_READWRITE);
            char[] vBuffer = new char[256];
            IntPtr pp = Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0);
            uint vNumberOfBytesRead = 0;
            try
            {
                for (int i = 0; i < vCount; i++)
                {
                    Win32API.SendMessage(vHandle, Win32API.TB_GETBUTTONTEXT, i, vPointer.ToInt32());

                    Win32API.ReadProcessMemoryEx(vProcess, vPointer,
                    Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
                    vBuffer.Length * sizeof(char), ref vNumberOfBytesRead);

                    int l = 0;
                    for (int j = 0; j < vBuffer.Length; j++)
                    {
                        if (vBuffer[j] == (char)0)
                        {
                            l = j;
                            break;
                        }
                    }
                    string s = new string(vBuffer, 0, l);

                    if (s.IndexOf(caption) >= 0)
                    {
                        if (isShow)
                            Win32API.SendMessage(vHandle, Win32API.TB_HIDEBUTTON, i, 0);
                        else
                            Win32API.SendMessage(vHandle, Win32API.TB_HIDEBUTTON, i, 1);
                    }
                    Console.WriteLine(s);
                }
            }
            finally
            {
                Win32API.VirtualFreeEx(vProcess, vPointer, 0, Win32API.MEM_RELEASE);
                Win32API.CloseHandle(vProcess);
            }
        }

        //显示/隐藏所有QQ窗体及QQ托盘图标 
        public static void SetQQWindowVisible(IntPtr owner, bool visible)
        {
            //显示/隐藏QQ窗体 
            IList<WindowInfo> list = WindowHide.GetQQWindowList((int)owner);
            foreach (WindowInfo wi in list)
                Win32API.ShowWindow(wi.Handle, visible ? Win32API.SW_SHOW : Win32API.SW_HIDE);

            //显示/隐藏QQ托盘图标 
            WindowHide.SetTrayIconVisible("QQ", visible);
        }

        //显示/隐藏所有MSN窗体及MSN托盘图标 
        public static void SetMSNWindowVisible(IntPtr owner, bool visible)
        {
            //显示/隐藏MSN窗体 
            IList<WindowInfo> list = WindowHide.GetMSNWindowList((int)owner);
            foreach (WindowInfo wi in list)
                Win32API.ShowWindow(wi.Handle, visible ? Win32API.SW_SHOW : Win32API.SW_HIDE);

            //显示/隐藏MSN托盘图标 
            WindowHide.SetTrayIconVisible("Windows Live Messenger", visible);
        }
    }

}

