using System;
using System.Drawing;
using System.Windows.Forms;
using StarEdit.Interface;
using StarEdit.DataServices;

namespace StarEdit.MyComponents
{
    public partial class UCBoxLabel : UserControl, IAutoControl
    {
        public new event EventHandler TextChanged;

        private string tableName;

        public UCBoxLabel()
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
            set { textBoxData.Text = value; }
        }

        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        private void textBoxData_TextChanged(object sender, EventArgs e)
        {
            int outdata;

            if (TextChanged != null)
                TextChanged(this, e);

            bool isInt = int.TryParse(textBoxData.Text, out outdata);
            if(isInt)
            {
                DataPack pack = DataPackBook.GetPack(tableName);
                if (pack.data.ContainsKey(outdata))
                {
                    labelData.Text = pack.data[outdata][1];
                    labelData.ForeColor = Color.Orange;
                    return;
                }
            }
            labelData.Text = "无";
            labelData.ForeColor = Color.LightGray;

        }
    }
}
