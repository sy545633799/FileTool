using System;
using System.Drawing;
using System.Windows.Forms;
using StarEdit.Interface;
using StarEdit.MysqlServices;

namespace StarEdit.MyComponents
{
    public partial class UCWiseGuideBoxLabel : UserControl, IAutoControl
    {
        public UCWiseGuideBoxLabel()
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
            try
            {
                if (textBoxData.Text.Contains(":"))
                {
                    string[] infos = textBoxData.Text.Split(':');
                    OldDataPack pack = OldDataPackBook.GetPack("cfg_task");
                    if (pack.data.ContainsKey(int.Parse(infos[0])))
                    {
                        labelData.Text = pack.data[int.Parse(infos[0])][1];
                        labelData.ForeColor = Color.Orange;
                        if (infos[1] == "3")
                            labelData.Text += "(接受)";
                        else if (infos[1] == "4")
                            labelData.Text += "(达成)";
                        else if (infos[1] == "5")
                            labelData.Text += "(提交)";
                        return;
                    }
                }
            }
            catch { }
            labelData.Text = "";
            labelData.ForeColor = Color.LightGray;
        }

        private void textBoxData_TextChanged(object sender, EventArgs e)
        {
            onSetValue();
        }
    }
}
