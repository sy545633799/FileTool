using System;
using System.Windows.Forms;

namespace StarEdit.Forms
{
    public partial class AddDataForm : Form
    {
        DialogResult result;
        int id;

        public DialogResult Result
        {
            get { return result; }
            set { result = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public bool NeedCopy
        {
            get { return checkBox1.Checked; }
        }

        public String Label1
        {
            set { label1.Text = value; }
        }

        public AddDataForm()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            id = int.Parse(textBoxId.Text);
            result = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            result = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
