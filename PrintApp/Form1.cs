using Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Printing;
using TestDll;
using Demo;
using Service.OutOfPaper;

namespace PrintApp
{
    public partial class Form1 : Form
    {
        public string printerName { get; set; }
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            printerName = printDocument2.PrinterSettings.PrinterName;
            //MessageBox.Show(PrintManager.GetPrinterStat(printerName).ToString());
            //MessageBox.Show(PrinterHelper.GetPrinterStatus(printerName).ToString());
            TestClass test = new TestClass();

            var status = test.GetPrinterStatusByName(printerName);
            MessageBox.Show(PrinterQueue.GetPrintQueue(printerName).ToString());
            printDocument2.PrinterSettings.PrinterName = printerName;
            
            //  printDocument2.PrintController = new System.Drawing.Printing.StandardPrintController();
            foreach (PaperSize item in printDocument2.PrinterSettings.PaperSizes)
            {

            }
            try {
                printDocument2.Print();

            } catch (Exception ex)
            {
                
            }
         
            //MessageBox.Show(PrinterHelper.GetPrinterStatus(printDocument1.PrinterSettings.PrinterName).ToString());
            //MessageBox.Show(PrintManager.GetPrinterStat(printer).ToString());
        }


        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            //MessageBox.Show(PrinterQueue.GetStatus(printerName));
            //MessageBox.Show(PrinterQueue.GetPrintQueue(printerName).ToString());
            Font font = new Font("宋体", 9);
            Brush bru = Brushes.Black;
            e.Graphics.DrawString("Hello，world！", font, bru, 0, 0);
        }

        private void printDocument2_PrintPage(object sender, PrintPageEventArgs e)
        {
            var list = PrinterQueue.GetList(printerName);
            //MessageBox.Show(PrintManager.GetPrinterStat(printerName).ToString());
            //MessageBox.Show(PrinterHelper.GetPrinterStatus(printerName).ToString());

            Font font = new Font("宋体", 9);
            Brush bru = Brushes.Black;
            e.Graphics.DrawString("Hello，world！", font, bru, 0, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ToolbarWindow32
            // ProgramTest.Test();
            //WindowHide.GetIconList();
            //找到托盘图标
            // ProgramTest.Test();
            //获取窗口信息
            //Boolean flag= WinTest.IsPrinterOutOfPaper();
            // if (flag)
            // {
            //     MessageBox.Show("打印机缺纸，请装入纸张！");
            // }

            //清空打印机队列
            //测试打印
            //try
            //{
            //    var name=printDocument2.PrinterSettings.PrinterName;
            //    printDocument2.Print();

            //}
            //catch (Exception ex)
            //{

            //}


            FindWinTimer findWinTimer = new FindWinTimer();
            findWinTimer.BeginFind();


            //PrinterStatusEvent statusEvent = new PrinterStatusEvent();
            //CheckPrinterStatus status = new CheckPrinterStatus(statusEvent);
            //statusEvent.Check();

        }
    }
}
