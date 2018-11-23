using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintApp
{
    /// <summary>
    /// 打印
    /// </summary>
    public partial class Form2 : Form
    {
        public string printerName { get; set; }
        public Form2()
        {
            InitializeComponent();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //设置字体样式
            Font f = new System.Drawing.Font("微软雅黑", 16);
            f = richTextBox1.Font;
            //设置字体颜色
            Brush b = new SolidBrush(richTextBox1.ForeColor);

            //e.绘制.画字符串（要打印的文本，文本格式，画刷-颜色和纹理，位置坐标）
            e.Graphics.DrawString(richTextBox1.Text, f, b, 20, 10);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void printPreviewControl1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dr = fontDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                richTextBox1.Font = fontDialog1.Font;
                richTextBox1.ForeColor = fontDialog1.Color;
            }
        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pageSetupDialog1.Document = printDocument1;
            pageSetupDialog1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            printPreviewControl1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            printDialog1.Document = printDocument1;
            DialogResult dr = printDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }
    }
}
