using System;
using System.Windows.Forms;
using StarEdit.Interface;

namespace StarEdit.MyComponents
{
    public partial class UCTextBox : UserControl, IAutoControl
    {
        public new event EventHandler TextChanged;

        int textSize;

        public string Value
        {
            get { return textBoxData.Text; }
            set { textBoxData.Text = value; }
        }

        public bool ReadOnly
        {
            set { textBoxData.ReadOnly = value; }
        }

        public int TextSize
        {
            get { return textSize; }
            set
            {
                textSize = value;
                if (textSize > 8 && textSize <= 32)
                {
                    textBoxData.Width = textSize * 15;
                }
                else if (textSize > 32)
                {
                    textBoxData.Width = 32 * 15;
                    textBoxData.Multiline = true;
                    textBoxData.Height += 30 * 2;
                }
                this.Width = textBoxData.Width;
                this.Height = textBoxData.Height;
            }
        }

        public UCTextBox()
        {
            InitializeComponent();
        }

        private void textBoxData_TextChanged(object sender, EventArgs e)
        {
            if (TextChanged != null)
                TextChanged(this, e);
        }
    }
}
