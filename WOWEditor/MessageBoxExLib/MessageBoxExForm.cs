using System;
using System.Drawing;
using System.Windows.Forms;

namespace MessageBoxExLib
{
    internal partial class MessageBoxExForm : Form
    {
        DialogResult result = System.Windows.Forms.DialogResult.Cancel;

        public MessageBoxExForm()
        {
            InitializeComponent();
        }

        public DialogResult Result
        {
            get { return result; }
            set { result = value; }
        }

        public string FulText
        {
            set
            {
                labelText.Text = value;
            }
        }

        public string Caption
        {
            set { Text = value; }
        }

        public new Icon Icon
        {
            set { pictureBox1.Image = ((Icon)value).ToBitmap(); }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            result = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}