using System;
using System.Windows.Forms;

namespace StarEdit.Forms
{
    public partial class AddBiDataForm : Form
    {
        DialogResult result;
        int id;
        int id2;

        public DialogResult Result
        {
            get { return result; }
            set { result = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; textBoxId.Text = id.ToString(); }
        }

        public int Id2
        {
            get { return id2; }
            set { id2 = value; textBoxId2.Text = id2.ToString(); }
        }

        public String Label1
        {
            set { label1.Text = value; }
        }

        public String Label2
        {
            set { label2.Text = value; }
        }

        public AddBiDataForm()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            id = int.Parse(textBoxId.Text);
            id2 = int.Parse(textBoxId2.Text);
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
