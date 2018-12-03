using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Service.OutOfPaper
{
    public class ShowBox
    {
        public static void ShowMessageBox()
        {
            MessageBox.Show("打印机缺纸，请装入纸张！如已装入纸张，请重启打印机！", "系统警告", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);

        }
    }
}
