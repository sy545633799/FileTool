using System;
using System.Drawing;
using System.Windows.Forms;
using StarEdit.Interface;
using StarEdit.DataServices;
using StarEdit.PaintServices;

namespace StarEdit.MyComponents
{
    public partial class UCItemList : UserControl, IAutoControl
    {
        public string Value
        {
            get { return textBoxData.Text; }
            set { textBoxData.Text = value; }
        }

        public UCItemList()
        {
            InitializeComponent();
        }

        private void textBoxData_TextChanged(object sender, EventArgs e)
        {
            String[] items = textBoxData.Text.Split('|');
            int count = 0;
            panelData.Controls.Clear();
            if (textBoxData.Text == "")
                return;

            try
            {
                foreach (String s in items)
                {
                    String[] itemData = s.Split(':');
                    String path = "";
                    String name = "";
                    if (int.Parse(itemData[1]) == 1 || int.Parse(itemData[1]) == 3)
                    {
                        DataPack pack = DataPackBook.GetPack("item");
                        int urlindex = pack.GetPackIndexByName("icon");
                        int nameindex = pack.GetPackIndexByName("name");
                        path = String.Format("res/images/icon/item/{0}.png", pack.data[int.Parse(itemData[0])][urlindex]);
                        name = pack.data[int.Parse(itemData[0])][nameindex];
                    }
                    else if (int.Parse(itemData[1]) == 2)
                    {
                        DataPack pack = DataPackBook.GetPack("equip");
                        int urlindex = pack.GetPackIndexByName("icon");
                        int nameindex = pack.GetPackIndexByName("name");
                        path = String.Format("res/images/icon/equip/{0}.png", pack.data[int.Parse(itemData[0])][urlindex]);
                        name = pack.data[int.Parse(itemData[0])][nameindex];
                    }
                    else if (int.Parse(itemData[1]) == 3)
                    {
                        DataPack pack = DataPackBook.GetPack("heji_rune");
                        int urlindex = pack.GetPackIndexByName("icon");
                        int nameindex = pack.GetPackIndexByName("name");
                        path = String.Format("res/images/icon/rune/{0}.png", pack.data[int.Parse(itemData[0])][urlindex]);
                        name = pack.data[int.Parse(itemData[0])][nameindex];
                    }

                    PictureBox pc = new PictureBox();
                    pc.Location = new Point((count % 2) * 90, (count / 2) * 30);
                    pc.SizeMode = PictureBoxSizeMode.Zoom;
                    pc.Width = 20;
                    pc.Height = 20;

                    pc.Image = MapPainter.GetNetImage(path);//Image.FromFile(path);
                    panelData.Controls.Add(pc);

                    Label lb = new Label();
                    lb.Location = new Point((count % 2) * 90 + 31, (count / 2) * 30);
                    lb.AutoSize = true;
                    lb.Text = String.Format("{0}x{1}", name, (itemData.Length < 3 ? "1" : itemData[2]));
                    panelData.Controls.Add(lb);

                    count++;
                }

                panelData.Height = (count + 1) / 2 * 30;
            }
            catch (Exception)
            {
                panelData.Controls.Clear();

                Label lb = new Label();
                lb.Location = new Point(0, 0);
                lb.AutoSize = true;
                lb.Text = "数据错误";
                panelData.Controls.Add(lb);

                panelData.Height = 30;
            }
        }
    }
}
