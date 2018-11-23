using Demo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class WindowsApi
    {
        public delegate bool EnumDesktopWindowsDelegate(IntPtr hWnd, uint lParam);
        [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDesktopWindowsDelegate lpEnumCallbackFunction, IntPtr lParam);

    }

    /// <summary>
    /// 窗口管理类
    /// </summary>
    public class WindowsManager
    {
        //public void Test()
        //{
        //    WindowsApi.EnumDesktopWindows(IntPtr.Zero, new WindowsApi.EnumDesktopWindowsDelegate(EnumWindowsProc), IntPtr.Zero);
        //    bool EnumWindowsProc(IntPtr hWnd, uint lparam)
        //    {
        //        if (pids.Contains(WndHelper.GetProcessId(hWnd)))
        //        {
        //            weas.Add(GetWinArgs(hWnd));
        //        }
        //        return true;
        //    }
        //}


        public const int WM_CLOSE = 0x10;
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, GetWindowCmd uCmd);

        [DllImport("user32.dll")]
        public extern static int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        /// <summary>
        /// 判断打印机是否缺纸
        /// 通过捕获打印机驱动的弹窗警告来判断
        /// </summary>
        /// <returns></returns>
        public static Boolean IsPrinterOutOfPaper()
        {
            Boolean flag = false;
            IntPtr hwnd_win;
            hwnd_win = FindWindow(null, "HP Color Laserjet CP1020 系列警报");
            if (hwnd_win != IntPtr.Zero)
            {
                IntPtr childHwnd = FindWindowEx(hwnd_win, IntPtr.Zero, "Static", "装入纸张");
                if (childHwnd != IntPtr.Zero)
                {
                    flag = true;
                }
                SendMessage(hwnd_win, WM_CLOSE, 0, 0);
            }
            else
            {
                PrinterToolbar.GetToolBarList();
            }
            return flag;
        }
      

    }

    /// <summary>
    /// 窗口与要获得句柄的窗口之间的关系。
    /// </summary>
    enum GetWindowCmd : uint
    {
        /// <summary>
        /// 返回的句柄标识了在Z序最高端的相同类型的窗口。
        /// 如果指定窗口是最高端窗口，则该句柄标识了在Z序最高端的最高端窗口；
        /// 如果指定窗口是顶层窗口，则该句柄标识了在z序最高端的顶层窗口：
        /// 如果指定窗口是子窗口，则句柄标识了在Z序最高端的同属窗口。
        /// </summary>
        GW_HWNDFIRST = 0,
        /// <summary>
        /// 返回的句柄标识了在z序最低端的相同类型的窗口。
        /// 如果指定窗口是最高端窗口，则该柄标识了在z序最低端的最高端窗口：
        /// 如果指定窗口是顶层窗口，则该句柄标识了在z序最低端的顶层窗口；
        /// 如果指定窗口是子窗口，则句柄标识了在Z序最低端的同属窗口。
        /// </summary>
        GW_HWNDLAST = 1,
        /// <summary>
        /// 返回的句柄标识了在Z序中指定窗口下的相同类型的窗口。
        /// 如果指定窗口是最高端窗口，则该句柄标识了在指定窗口下的最高端窗口：
        /// 如果指定窗口是顶层窗口，则该句柄标识了在指定窗口下的顶层窗口；
        /// 如果指定窗口是子窗口，则句柄标识了在指定窗口下的同属窗口。
        /// </summary>
        GW_HWNDNEXT = 2,
        /// <summary>
        /// 返回的句柄标识了在Z序中指定窗口上的相同类型的窗口。
        /// 如果指定窗口是最高端窗口，则该句柄标识了在指定窗口上的最高端窗口；
        /// 如果指定窗口是顶层窗口，则该句柄标识了在指定窗口上的顶层窗口；
        /// 如果指定窗口是子窗口，则句柄标识了在指定窗口上的同属窗口。
        /// </summary>
        GW_HWNDPREV = 3,
        /// <summary>
        /// 返回的句柄标识了指定窗口的所有者窗口（如果存在）。
        /// GW_OWNER与GW_CHILD不是相对的参数，没有父窗口的含义，如果想得到父窗口请使用GetParent()。
        /// 例如：例如有时对话框的控件的GW_OWNER，是不存在的。
        /// </summary>
        GW_OWNER = 4,
        /// <summary>
        /// 如果指定窗口是父窗口，则获得的是在Tab序顶端的子窗口的句柄，否则为NULL。
        /// 函数仅检查指定父窗口的子窗口，不检查继承窗口。
        /// </summary>
        GW_CHILD = 5,
        /// <summary>
        /// （WindowsNT 5.0）返回的句柄标识了属于指定窗口的处于使能状态弹出式窗口（检索使用第一个由GW_HWNDNEXT 查找到的满足前述条件的窗口）；
        /// 如果无使能窗口，则获得的句柄与指定窗口相同。
        /// </summary>
        GW_ENABLEDPOPUP = 6
    }


    public class WindowTest
    {
        public delegate bool CallBack(int hwnd, int lParam);
        [DllImport("user32")] public static extern int EnumWindows(CallBack x, int y);
        [DllImport("user32.dll")] private static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")] private static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);

        public static  void Test()
        {
            CallBack myCallBack = new CallBack(Recall);
            EnumWindows(myCallBack, 0);
        }

        public static bool Recall(int hwnd, int lParam)
        {
            StringBuilder sb = new StringBuilder(256);
            IntPtr PW = new IntPtr(hwnd);

            GetWindowTextW(PW, sb, sb.Capacity); //得到窗口名并保存在strName中
            string strName = sb.ToString();

            GetClassNameW(PW, sb, sb.Capacity); //得到窗口类名并保存在strClass中
            string strClass = sb.ToString();

            if (strName.IndexOf("应用管理器") >= 0 && strClass.IndexOf("类名关键字") >= 0)
            {
                return false; //返回false中止EnumWindows遍历
            }
            else
            {
                return true; //返回true继续EnumWindows遍历
            }
        }
    }
}
