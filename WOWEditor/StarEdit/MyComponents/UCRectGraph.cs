using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using StarEdit.Interface;
using StarEdit.JudgeServices;
using StarEdit.PaintServices;

namespace StarEdit.MyComponents
{
    public partial class UCRectGraph : UserControl, IAutoControl
    {
        string val;
        Dictionary<string, int> parms = new Dictionary<string, int>();

        public UCRectGraph()
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
            }
        }

        public void SetData(string name, int value)
        {
            if (!parms.ContainsKey(name))
            {
                parms.Add(name, value);
            }
            else
            {
                parms[name] = value;
            }
            doubleBufferedPanel1.Invalidate();
        }

        public int GetData(string name)
        {
            if (parms.ContainsKey(name))
            {
                return parms[name];
            }
            return 0;
        }

        private void doubleBufferedPanel1_Paint(object sender, PaintEventArgs e)
        {
            int mid1 = GetData("m1");
            int mid2 = GetData("m2");

            if (mid1 == 0)
            {
                mid1 = mid2;
            }

            if (mid2 == 0)
            {
                mid2 = mid1;
            }


            Point pos = TilePixelConvertor.TileToPixel(GetData("x1"), GetData("y1"));

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    String imagepath = String.Format("res/map/{0}/{1}_{2}.jpg", mid1, pos.Y / 300 + j, pos.X / 300 + i);

                    Image item = MapPainter.GetNetImage(imagepath);
                    //if (File.Exists(imagepath))
                    if (item!=null)
                    {
                        Image image = item;// new Bitmap(imagepath);
                        e.Graphics.DrawImage(image, (i + 1) * 80, (j + 1) * 80, 80, 80);
                        image.Dispose();
                    }
                }
            }

            e.Graphics.DrawEllipse(Pens.Red, (pos.X % 80) + 80 - 15, (pos.Y % 80) + 80 - 15, 30, 30);

            pos = TilePixelConvertor.TileToPixel(GetData("x2"), GetData("y2"));

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    String imagepath = String.Format("res/map/{0}/{1}_{2}.jpg", mid2, pos.Y / 300 + j, pos.X / 300 + i);

                    Image item = MapPainter.GetNetImage(imagepath);

                    if (item!=null)
                    {
                        Image image = item;// new Bitmap(imagepath);
                        e.Graphics.DrawImage(image, (i + 1) * 80 + 240, (j + 1) * 80, 80, 80);
                        image.Dispose();
                    }
                }
            }

            e.Graphics.DrawEllipse(Pens.Red, (pos.X % 80) + 80 + 240 - 15, (pos.Y % 80) + 80 - 15, 30, 30);

            Font font = new Font("宋体", 10, FontStyle.Bold);
            e.Graphics.DrawString("出发地", font, Brushes.Black, 10, 10);
            e.Graphics.DrawString("出发地", font, Brushes.White, 9, 9);

            e.Graphics.DrawString("目的地", font, Brushes.Black, 250, 10);
            e.Graphics.DrawString("目的地", font, Brushes.White, 249, 9);
            font.Dispose();
        }
    }
}
