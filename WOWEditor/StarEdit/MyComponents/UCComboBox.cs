using System;
using System.Windows.Forms;
using StarEdit.Interface;

namespace StarEdit.MyComponents
{
    public partial class UCComboBox : UserControl, IAutoControl
    {
        public event EventHandler SelectedIndexChanged;

        public string Value
        {
            get
            {
                string s="";
                if(comboBox1.SelectedItem!=null)s = comboBox1.SelectedItem.ToString();
                if (s.Contains(".")) //.分割结构
                {
                    String[] data = s.Split('.');
                    return data[0];
                }
                else //string结构
                {
                    return s;
                }
            }
            set
            {
                foreach (String s in comboBox1.Items)
                {
                    if (s.Contains(".")) //.分割结构
                    {
                        String[] data = s.Split('.');
                        if (value == data[0])
                        {
                            comboBox1.SelectedItem = s;
                        }
                    }
                    else //string结构
                    {
                        if (value == s)
                        {
                            comboBox1.SelectedItem = s;
                        }
                    }
                }
            }
        }

        public string Info
        {
            set
            {
                String[] datas = value.Split('|');
                foreach (String data in datas)
                {
                    comboBox1.Items.Add(data);
                }
            }
        }

        public UCComboBox()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, e);
        }
    }
}
