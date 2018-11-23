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

        public void BeginFind()
        {
            _SmsTimer = new System.Timers.Timer(1000 * 3 * 1);//TODO:先硬编码
            _SmsTimer.Elapsed += ElapsedEventHandler;
            _SmsTimer.Start();
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
                        MessageBox.Show("打印机缺纸，请装入纸张！");
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
