using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Service
{
    /// <summary>
    /// 计时器
    /// </summary>
    public class FindWinTimer
    {
        private static System.Timers.Timer _SmsTimer;
        public delegate void ShowMessage();
        ShowMessage _showMessage;
        public void BeginFind(ShowMessage showMessage)
        {
            _SmsTimer = new System.Timers.Timer(1000 * 3 * 1);//TODO:先硬编码
            _SmsTimer.Elapsed += ElapsedEventHandler;
            _SmsTimer.Start();
            _showMessage = showMessage;
        }
        public void ElapsedEventHandler(object sender, ElapsedEventArgs e)
        {
            _SmsTimer.Stop();

            if (DateTime.Now.Hour > 1 && DateTime.Now.Hour < 23)
            {
                try
                {
                    var flag = WindowsManager.IsPrinterOutOfPaper();
                    if (flag)
                    {
                        //   MessageBox.Show("打印机缺纸，请装入纸张！如已装入纸张，请重启打印机！");
                        _showMessage();
                    }
                }

                catch (Exception ex)
                {
                   
                }
                finally
                {
                    _SmsTimer.Start();
                }
            }
            else
            {
                _SmsTimer.Start();
            }

        }
    }
}
