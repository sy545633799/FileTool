using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using StarEdit.Interface;
using StarEdit.DataServices;
using System.Collections.Generic;

namespace StarEdit.MyComponents
{
    public partial class UCDialogBox : UserControl, IAutoControl
    {
        public string Value
        {
            get { return textBoxData.Text; }
            set { textBoxData.Text = value; }
        }

        public UCDialogBox()
        {
            InitializeComponent();
        }

        private void textBoxData_TextChanged(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"<l><n>(?<num>\d+)</n></l>", RegexOptions.Multiline);
            string data = textBoxData.Text;
            MatchCollection col = regex.Matches(data);
            foreach(Match mat in col)
            {
                string cap = mat.Groups["num"].Value;
                DataPack pack = DataPackBook.GetPack("npc");
                int info = int.Parse(cap);
                if (pack.data.ContainsKey(info))
                {
                    cap = pack.data[info][1];
                }
                data = data.Replace(mat.Value, String.Format("[{0}]", cap));
            }

            regex = new Regex(@"<l><g>(?<num>\d+)</g></l>", RegexOptions.Multiline);
            col = regex.Matches(data);
            foreach (Match mat in col)
            {
                string cap = mat.Groups["num"].Value;
                DataPack pack = DataPackBook.GetPack("monster");
                int index = pack.GetPackIndexByName("group_id");
                foreach (List<String> monsterData in pack.data.Values)
                {
                    if (monsterData[index] == cap)
                    {
                        cap = monsterData[1];
                        break;
                    }
                }
                data = data.Replace(mat.Value, String.Format("[{0}]", cap));
            }

            regex = new Regex(@"<l><z>(?<num>\d+)</z></l>", RegexOptions.Multiline);
            col = regex.Matches(data);
            foreach (Match mat in col)
            {
                string cap = mat.Groups["num"].Value;
                DataPack pack = DataPackBook.GetPack("plant");
                int index = pack.GetPackIndexByName("group_id");
                foreach (List<String> plantData in pack.data.Values)
                {
                    if (plantData[index] == cap)
                    {
                        cap = plantData[1];
                        break;
                    }
                }
                data = data.Replace(mat.Value, String.Format("[{0}]", cap));
            }

            regex = new Regex(@"<l><f>(?<num>\d+)</f></l>", RegexOptions.Multiline);
            col = regex.Matches(data);
            foreach (Match mat in col)
            {
                string cap = mat.Groups["num"].Value;
                DataPack pack = DataPackBook.GetPack("dungeon");
                int info = int.Parse(cap);
                if (pack.data.ContainsKey(info))
                {
                    cap = pack.data[info][1];
                }
                data = data.Replace(mat.Value, String.Format("[{0}]", cap));
            }

            regex = new Regex("<l><d m=\"\\d+\" x=\"\\d+\" y=\"\\d+\">(?<name>\\w+)</d></l>", RegexOptions.Multiline);
            col = regex.Matches(data);
            foreach (Match mat in col)
            {
                string cap = mat.Groups["name"].Value;
                data = data.Replace(mat.Value, String.Format("[{0}]", cap));
            }

            richTextBox1.Text = "";
            richTextBox1.Text = data;

            int baseq = 0;
            while (true)
            {
                int start = richTextBox1.Text.IndexOf("[", baseq);
                int end = richTextBox1.Text.IndexOf("]", baseq);
                if (start == -1)
                    break;

                if (start >= 0 && end >= 0)
                {
                    richTextBox1.Select(start, end - start + 1);
                    richTextBox1.SelectionColor = Color.Blue;
                    richTextBox1.Select(start, 0);
                }
                baseq = end + 1;
            }
        }
    }
}

