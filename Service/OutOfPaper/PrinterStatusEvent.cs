using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Service.OutOfPaper
{
    public delegate void PrinterStatusHandler();
    public class PrinterStatusEvent
    {
        public event PrinterStatusHandler PrinterStatusChange;

        public void Check()
        {
            PrinterStatusChange?.Invoke();
        }


    }

    public class CheckPrinterStatus
    {
        public CheckPrinterStatus(PrinterStatusEvent sevent)
        {
            sevent.PrinterStatusChange += new PrinterStatusHandler(CheckStatus);
        }

        public void CheckStatus()
        {
            var flag = WindowsManager.IsPrinterOutOfPaper();
            if (flag)
            {
                MessageBox.Show("打印机缺纸，请装入纸张！如已装入纸张，请重启打印机！");
            }
            else
            {
                MessageBox.Show("测试");
            }
        }
    }


}
