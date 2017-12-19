using System;
using System.Drawing;

namespace StarEdit.JudgeServices
{
    class TilePixelConvertor
    {
        public static Point TileToPixel(int tx, int ty)
        {
            int px = tx * 48 + 24;
            int py = ty * 34 + 17;

            return new Point(px, py);
        }

        public static Point PixelToTile(int px, int py)
        {
            return new Point(px / 48, py / 34);
        }

        public static bool InRegion(int min_x, int min_y, int max_x, int max_y, int x, int y)
        {
            int exp1min = min_x + min_y / 2;
            int exp1max = max_x + max_y / 2;
            int exp1 = x + y / 2;

            int exp2min = min_x - min_y / 2;
            int exp2max = max_x - max_y / 2;
            int exp2 = x - y / 2;

            if ((exp1 >= exp1min && exp1 <= exp1max) && (exp2 >= exp2min && exp2 <= exp2max))
            {
                return true;
            }
            return false;
        }
    }
}
