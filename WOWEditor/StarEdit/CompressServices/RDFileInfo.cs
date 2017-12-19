using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using StarEdit.PaintServices;

namespace StarEdit.CompressServices
{
    class RDFileInfo
    {
        public static void GetBlockInfo(string selectFolder, int mapid, Size mapsize, List<int> blockInfos)
        {
            int row = mapsize.Height / 34;
            int col = mapsize.Width / 48;

            string rdpath = String.Format("res/map/{0}.rd", selectFolder);

            byte[] datas = new byte[(row * col + 1) / 2];
            MemoryStream ms = new MemoryStream(datas);
            ZlibService.DecompressStreamToStream(MapPainter.GetNetStream(rdpath), ms);
            // ZlibService.DecompressFileToStream(rdpath, ms);
            for (int i = 0; i < (row * col + 1) / 2; i++)
            {
                blockInfos.Add(datas[i] & 15);
                blockInfos.Add((datas[i] & 240) >> 4);
            }
        }
    }
}
