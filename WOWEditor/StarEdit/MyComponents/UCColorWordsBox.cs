using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using StarEdit.Interface;
using StarEdit.MysqlServices;

namespace StarEdit.MyComponents
{
    public partial class UCColorWordsBox : UserControl, IAutoControl
    {
        public string Value
        {
            get { return textBoxData.Text; }
            set { textBoxData.Text = value; }
        }

        public UCColorWordsBox()
        {
            InitializeComponent();
        }

        private void textBoxData_TextChanged(object sender, EventArgs e)
        {
            string data = textBoxData.Text.Replace("|n", "");
            richTextBox1.Text = "";
            if (!data.Contains("|"))
            {
                richTextBox1.Text = data;
                return;
            }

            try
            {
                string[] attrs = data.Split('|');
                for (int i = 0; i < attrs.Length; i += 2)
                {
                    int start = richTextBox1.Text.Length;
                    richTextBox1.AppendText(String.Format("{0}", attrs[i + 1]));
                    int end = richTextBox1.Text.Length;
                    richTextBox1.Select(start, end - start + 1);
                    if (attrs[i] == "")
                        attrs[i] = "#000000";
                    richTextBox1.SelectionColor = System.Drawing.ColorTranslator.FromHtml(attrs[i]);
                    richTextBox1.Select(start, 0);
                }
            }
            catch
            {
                richTextBox1.Text = "错误数据";
            }
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                Color col = colorDialog1.Color;
                textBoxData.Text = textBoxData.Text.Insert(textBoxData.SelectionStart, string.Format("#{0:X2}{1:X2}{2:X2}", col.R, col.G, col.B));
            }
        }
    }
}
