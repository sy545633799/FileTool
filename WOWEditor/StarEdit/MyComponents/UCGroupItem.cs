using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using StarEdit.Interface;
using StarEdit.DataServices;
using StarEdit.PaintServices;

namespace StarEdit.MyComponents
{
    public partial class UCGroupItem : UserControl, IAutoControl
    {
        string val;
        List<TriData> tks;

        public UCGroupItem()
        {
            InitializeComponent();
        }

        public string Value
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
                tks = new List<TriData>();

                TriDataPack data = TriDataPackBook.GetPack("drop_group");
                foreach (TriData tk in data.data.Keys)
                {
                    if (tk.a == int.Parse(val))
                    {
                        tks.Add(tk);
                    }
                }

                this.Invalidate();
            }
        }

        private void UCGroupItem_Paint(object sender, PaintEventArgs e)
        {
            TriDataPack data = TriDataPackBook.GetPack("drop_group");

            Font font = new Font("宋体", 10, FontStyle.Regular);
            e.Graphics.DrawString(String.Format("掉落组: {0}, 累计{1}件", val ,tks.Count), font, Brushes.Black, 0, 0);

            int pos = 0;
            foreach (TriData tk in tks)
            {
                int id = int.Parse(data.data[tk][1]);
                int type = int.Parse(data.data[tk][2]);
                int pro = int.Parse(data.data[tk][3]);

                string url = "";
                string name = "";
                if (type == 1)
                {
                    DataPack itm = DataPackBook.GetPack("item");
                    int index = itm.GetPackIndexByName("icon");
                    if (!itm.data.ContainsKey(id))
                        continue;
                    url = string.Format("res/images/icon/item/{0}.png", itm.data[id][index]);
                    index = itm.GetPackIndexByName("name");
                    name = itm.data[id][index];
                }
                else if (type == 2)
                {
                    DataPack itm = DataPackBook.GetPack("equip");
                    int index = itm.GetPackIndexByName("icon");
                    if (!itm.data.ContainsKey(id))
                        continue;
                    url = string.Format("res/images/icon/equip/{0}.png", itm.data[id][index]);
                    index = itm.GetPackIndexByName("name");
                    name = itm.data[id][index];
                }
                else if (type == 3)
                {
                    DataPack itm = DataPackBook.GetPack("heji_rune");
                    int index = itm.GetPackIndexByName("icon");
                    if (!itm.data.ContainsKey(id))
                        continue;
                    url = string.Format("res/images/icon/rune/{0}.png", itm.data[id][index]);
                    index = itm.GetPackIndexByName("name");
                    name = itm.data[id][index];
                }

                //if (File.Exists(url))
                Image item = MapPainter.GetNetImage(url);
                e.Graphics.DrawImage(item, pos * 45 + 5, 20, 30, 30);

                e.Graphics.DrawString(String.Format("{0}", name.Substring(0, Math.Min(name.Length, 3))), font, Brushes.Black, pos * 45, 52);
                e.Graphics.DrawString(String.Format("{0}%", pro / 10), font, Brushes.Black, pos * 45 + 5, 65);

                pos++;
            }
            font.Dispose();
        }
    }
}
