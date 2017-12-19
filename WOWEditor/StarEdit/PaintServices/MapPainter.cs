using System.Drawing;
using System.Net;
using System.IO;
using StarEdit.MysqlServices;

namespace StarEdit.PaintServices
{
    class MapPainter
    {
        /// <summary>
        /// 用来绘制地形的阻挡区域函数
        /// </summary>
        /// <param name="g">绘画句柄</param>
        /// <param name="centerX">菱形的中点X坐标</param>
        /// <param name="centerY">菱形的中点Y坐标</param>
        /// <param name="val">阻挡值0-3</param>
        public static void DrawRectTile(Graphics g, int x, int y, int val)
        {
            if (val == 0)
            {
                return;
            }
            Brush sb = new SolidBrush(getTileColor(val));
            Rectangle rect = new Rectangle(x, y, 48, 34);
            g.FillRectangle(sb, rect);
            sb.Dispose();
        }

        /// <summary>
        /// 获得地形值对应的颜色
        /// </summary>
        /// <param name="val">对应的地形值</param>
        /// <returns>对应地形值的颜色</returns>
        private static Color getTileColor(int val)
        {
            switch (val)
            {
                case 1: return Color.FromArgb(150, 0, 50, 150);
                case 2: return Color.FromArgb(150, 255, 200, 0);
                case 3: return Color.FromArgb(150, 255, 0, 0);
            }
            return Color.FromArgb(150, 0, 0, 0);
        }

        public static void DrawDiamondRegion(Graphics g, int centerX, int centerY, int val)
        {
            if (val == 0)
            {
                return;
            }
            Point[] point = new Point[4];
            point[0] = new Point(centerX - 32, centerY);
            point[1] = new Point(centerX, centerY - 16);
            point[2] = new Point(centerX + 32, centerY);
            point[3] = new Point(centerX, centerY + 16);
            Brush sb = new SolidBrush(getRegionColor(val));
            g.FillPolygon(sb, point);
            sb.Dispose();
        }

        public static void DrawCircleRegion(Graphics g, int centerX, int centerY, int val)
        {
            if (val == 0)
            {
                return;
            }
            Brush sb = new SolidBrush(getRegionColor(val));
            g.FillEllipse(sb, centerX - 12, centerY - 12, 24, 24);
            sb.Dispose();
        }

        private static Color getRegionColor(int val)
        {
            switch (val)
            {
                case 1: return Color.FromArgb(100, 100, 255, 100);
                case 2: return Color.FromArgb(100, 255, 255, 255);
                case 3: return Color.FromArgb(100, 204, 80, 255);
                case 4: return Color.FromArgb(100, 255, 200, 0);
            }
            return Color.FromArgb(100, 0, 0, 0);
        }
        public static Image GetNetImage(string url)
        {
            int nlenth =url.Length;
            string urlstr = url.Substring(4, nlenth-4);
            string httpPath = MysqlService.getResPath();
            WebResponse response = null;
            Stream stream = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}{1}", httpPath, urlstr));
            try
            {
                response = request.GetResponse();
                stream = response.GetResponseStream();
            }
            catch
            {
                return Image.FromFile("icons/blank.jpg");
            }
            return new Bitmap(stream);
        }

        public static Stream GetNetStream(string url)
        {
            string httpPath = MysqlService.getResPath();
            Stream stream = null;

            int nlenth = url.Length;
            string urlstr = url.Substring(4, nlenth - 4);

            try
            {
                WebResponse response = null;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}{1}", httpPath, urlstr));
                response = request.GetResponse();
                stream = response.GetResponseStream();
            }
            catch
            {

            }
            return stream;
        }
    }
}
