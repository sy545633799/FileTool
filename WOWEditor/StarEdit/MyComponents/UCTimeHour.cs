using System;
using System.Drawing;
using System.Windows.Forms;
using StarEdit.Interface;
using StarEdit.MysqlServices;

namespace StarEdit.MyComponents
{
    public partial class UCTimeHour : UserControl, IAutoControl
    {
        public UCTimeHour()
        {
            InitializeComponent();
        }

        public String Value
        {
            get { return textBoxData.Text; }
            set { textBoxData.Text = value; }
        }

        private void textBoxData_TextChanged(object sender, EventArgs e)
        {
            int outdata;

            bool isInt = int.TryParse(textBoxData.Text, out outdata);
            if(isInt)
            {
                labelData.Text = string.Format("{0}点{1}分", outdata/ 60, outdata % 60);
                labelData.ForeColor = Color.Green;
                return;
            }
            labelData.Text = "无";
            labelData.ForeColor = Color.LightGray;

        }
    }
}
