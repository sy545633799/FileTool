using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using StarEdit.Interface;
using StarEdit.DataServices;
using StarEdit.PaintServices;

namespace StarEdit.MyComponents
{
    public partial class UCItem : UserControl, IAutoControl
    {
        string val;
        int type;

        public UCItem()
        {
            InitializeComponent();
        }

        public int Type
        {
            set
            {
                type = value;
            }
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

                this.Invalidate();
            }
        }

        private void UCGroupItem_Paint(object sender, PaintEventArgs e)
        {
            TriDataPack data = TriDataPackBook.GetPack("drop_group");

            Font font = new Font("宋体", 10, FontStyle.Regular);
            e.Graphics.DrawString(String.Format("物品id: {0}, 物品类型: {1}", val, type), font, Brushes.Black, 0, 0);

            int id = int.Parse(val);

            string url = "";
            string name = "";
            if (type == 1)
            {
                DataPack itm = DataPackBook.GetPack("item");
                int index = itm.GetPackIndexByName("icon");
                if (!itm.data.ContainsKey(id))
                    return;
                url = string.Format("res/images/icon/item/{0}.png", itm.data[id][index]);
                index = itm.GetPackIndexByName("name");
                name = itm.data[id][index];
            }
            else if( type == 2 )
            {
                DataPack itm = DataPackBook.GetPack("equip");
                int index = itm.GetPackIndexByName("icon");
                if (!itm.data.ContainsKey(id))
                    return;
                url = string.Format("res/images/icon/equip/{0}.png", itm.data[id][index]);
                index = itm.GetPackIndexByName("name");
                name = itm.data[id][index];
            }
            else if (type == 3)
            {
                DataPack itm = DataPackBook.GetPack("heji_rune");
                int index = itm.GetPackIndexByName("icon");
                if (!itm.data.ContainsKey(id))
                    return;
                url = string.Format("res/images/icon/rune/{0}.png", itm.data[id][index]);
                index = itm.GetPackIndexByName("name");
                name = itm.data[id][index];
            }

            //if (File.Exists(url))
            Image item = MapPainter.GetNetImage(url);
            if(item!=null)
                e.Graphics.DrawImage(item, 5, 20, 30, 30);

            e.Graphics.DrawString(name, font, Brushes.Black, 52, 30);

            font.Dispose();
        }
    }
}
