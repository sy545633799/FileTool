using System;
using System.Drawing;
using System.Windows.Forms;
using StarEdit.Interface;
using StarEdit.MysqlServices;

namespace StarEdit.MyComponents
{
    public partial class UCWiseBoxLabel : UserControl, IAutoControl
    {
        public UCWiseBoxLabel()
        {
            InitializeComponent();
        }

        public new bool Enabled
        {
            set
            {
                textBoxData.Enabled = value;
            }
        }

        public String Value
        {
            get { return textBoxData.Text; }
            set { textBoxData.Text = value;
            onSetValue();
            }
        }

        private void onSetValue()
        {
            int outdata;
            bool isInt = int.TryParse(textBoxData.Text, out outdata);
            if (isInt)
            {
                {
                    OldDataPack pack = OldDataPackBook.GetPack("cfg_item");
                    if (pack.data.ContainsKey(outdata))
                    {
                        labelData.Text = pack.data[outdata][1];
                        labelData.ForeColor = Color.Orange;
                        return;
                    }
                }

                {
                    OldDataPack pack = OldDataPackBook.GetPack("cfg_equip");
                    if (pack.data.ContainsKey(outdata))
                    {
                        labelData.Text = pack.data[outdata][1];
                        labelData.ForeColor = Color.Orange;
                        return;
                    }
                }
            }
            labelData.Text = "无";
            labelData.ForeColor = Color.LightGray;
        }

        private void textBoxData_TextChanged(object sender, EventArgs e)
        {
            onSetValue();
        }
    }
}
