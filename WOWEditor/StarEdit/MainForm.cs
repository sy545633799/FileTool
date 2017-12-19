using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using StarEdit.CompressServices;
using StarEdit.Forms;
using StarEdit.JudgeServices;
using StarEdit.MysqlServices;
using StarEdit.PaintServices;
using StarEdit.DataServices;


namespace StarEdit
{
    

    public partial class MainForm : Form
    {
        private int selectMapId;
        private int mouseX, mouseY;
        private string selectMapName;
        private string selectFolder;
        private bool mouseDown = false;
        private Point movePoint;
        private bool showGridLine = false;
        private bool showBarrier = false;
        private bool showMonster = true;
        private bool showPlant = true;
        private bool showSkin = true;
        private bool showNpc = true;             
        private Point baseOffside;
        private Size selectMapSize;
        private List<int> blockInfos;
    //    private int[] depthInfos;
        private int searchBase;
        private ListViewItem lastSelectedLive;
        private ListViewItem lastSelectedVegas;
        private List<Point> targets;

        

        public MainForm()
        {
            InitializeComponent();
            onStackChange(0);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            String version = FileVersionInfo.GetVersionInfo(System.Windows.Forms.Application.ExecutablePath).FileVersion;
            string title = MysqlService.getNamever();
            Text = String.Format("{0} v{1}", title, version);
        }

        private void viewStack1_SelectedIndexChanged(object sender, EventArgs e)
        {
            onStackChange(viewStack1.SelectedIndex);
        }

        private void onStackChange(int cid)
        {
            switch (cid)
            {
                case 0: installMap(); break;
//                case 1: installMonster(); break;
//                case 2: installPlant(); break;
            }
        }

        private void installMap()
        {
            if (listViewMaps.Items.Count > 0)
                return;

            DataPack pack = DataPackBook.GetPack("map");
            int midindex = pack.GetPackIndexByName("id");
            int mnameindex = pack.GetPackIndexByName("name");

            listViewMaps.Items.Clear();
            foreach (List<String> map in pack.data.Values)
            {
                string id = map[midindex];
                string name = map[mnameindex];

                ListViewItem lvm = new ListViewItem(String.Format("{0}.{1}", id, name));
                listViewMaps.Items.Add(lvm);
            }
        }

        private void installMonster()
        {
            if (listViewMonster.Items.Count > 0)
                return;

            OldDataPack pack = OldDataPackBook.GetPack("cfg_monster");
            int midindex = pack.GetPackIndexByName("mid");
            int mnameindex = pack.GetPackIndexByName("name");

            listViewMonster.Items.Clear();
            foreach (List<String> monster in pack.data.Values)
            {
                string id = monster[midindex];
                string name = monster[mnameindex];

                ListViewItem lvm = new ListViewItem(String.Format("{0}.{1}", id, name));
                listViewMonster.Items.Add(lvm);
            }
        }

        private void installLiveMonster()
        {
            OldDataPack pack = OldDataPackBook.GetPack("cfg_map_monster");
            int idindex = pack.GetPackIndexByName("id");
            int mapidindex = pack.GetPackIndexByName("mapid");
            int monsteridindex = pack.GetPackIndexByName("monsterid");

            OldDataPack packM = OldDataPackBook.GetPack("cfg_monster");
            int mnameindex = packM.GetPackIndexByName("name");

            listViewLives.Items.Clear();
            foreach (List<String> monster in pack.data.Values)
            {
                string id = monster[idindex];
                string map = monster[mapidindex];
                int mid = int.Parse(monster[monsteridindex]);
                if (int.Parse(map) == selectMapId)
                {
                    ListViewItem lvm = new ListViewItem(String.Format("{0}.{1} (id={2})", id, packM.data[mid][mnameindex], mid));
                    listViewLives.Items.Add(lvm);
                }
            }
        }

        private void installPlant()
        {
            if (listViewPlant.Items.Count > 0)
                return;

            OldDataPack pack = OldDataPackBook.GetPack("cfg_plant");
            int pidindex = pack.GetPackIndexByName("id");
            int pnameindex = pack.GetPackIndexByName("name");

            listViewPlant.Items.Clear();
            foreach (List<String> plant in pack.data.Values)
            {
                string id = plant[pidindex];
                string name = plant[pnameindex];

                ListViewItem lvm = new ListViewItem(String.Format("{0}.{1}", id, name));
                listViewPlant.Items.Add(lvm);
            }
        }

        private void installLivePlant()
        {
            OldDataPack pack = OldDataPackBook.GetPack("cfg_map_plant");
            int idindex = pack.GetPackIndexByName("id");
            int mapidindex = pack.GetPackIndexByName("mapid");
            int plantidindex = pack.GetPackIndexByName("plantid");

            OldDataPack packM = OldDataPackBook.GetPack("cfg_plant");
            int pnameindex = packM.GetPackIndexByName("name");

            listViewVegas.Items.Clear();
            foreach (List<String> plant in pack.data.Values)
            {
                string id = plant[idindex];
                string map = plant[mapidindex];
                int pid = int.Parse(plant[plantidindex]);

                if (int.Parse(map) == selectMapId)
                {
                    ListViewItem lvm = new ListViewItem(String.Format("{0}.{1} (id={2})", id, packM.data[pid][pnameindex], pid));
                    listViewVegas.Items.Add(lvm);
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewMaps.SelectedItems.Count <= 0)
                return;

            String[] data = listViewMaps.SelectedItems[0].Text.Split('.');
            selectMapId = int.Parse(data[0]);
            changeMap(selectMapId);
        }

        private void changeMap(int mapid)
        {
            DataPack pack = DataPackBook.GetPack("map");
            int width = pack.GetPackIndexByName("width");
            int height = pack.GetPackIndexByName("height");
            int nameindex = pack.GetPackIndexByName("name");
            int folderindex = pack.GetPackIndexByName("url");

            selectMapSize.Width = int.Parse(pack.data[mapid][width]);
            selectMapSize.Height = int.Parse(pack.data[mapid][height]);
            selectMapName = pack.data[mapid][nameindex];
            selectFolder = pack.data[mapid][folderindex];

            if (panelMap.Width == 0 || panelMap.Height == 0)
            {
                return;
            }

            baseOffside = new Point(0, 0);

            String minipath = String.Format("res/map/{0}.jpg", selectFolder);

            //if (File.Exists(minipath))
            {
                pictureBoxMiniMap.Image = MapPainter.GetNetImage(minipath); 
            }

            blockInfos = new List<int>();
            RDFileInfo.GetBlockInfo(selectFolder, selectMapId, selectMapSize, blockInfos);
//            installLiveMonster();
//            installLivePlant();
            panelMap.Invalidate();
        }

        private void panelMap_Paint(object sender, PaintEventArgs e)
        {

            if (selectMapId == 0 || selectMapSize.Width == 0 || selectMapSize.Height == 0)
                return;

            if (showSkin)
            {
                for (int i = baseOffside.X / 192; i < (baseOffside.X + panelMap.Width) / 192 + 1; i++)
                {
                    for (int j = baseOffside.Y / 192; j < (baseOffside.Y + panelMap.Height) / 192 + 1; j++)
                    {
                        string imagepath = string.Format("res/map/{0}/{1}_{2}.jpg", selectFolder, j, i);
                        //if (File.Exists(imagepath))
                        {
                            Image image = MapPainter.GetNetImage(imagepath); 
                            e.Graphics.DrawImage(image, i * 192 - baseOffside.X, j * 192 - baseOffside.Y);
                            image.Dispose();
                        }
                    }
                }
            }

            if (showBarrier) //是否显示阻挡
            {
                int col = (int)Math.Floor((double)selectMapSize.Width / 48);

                int xoff = baseOffside.X % 48;
                int yoff = baseOffside.Y % 34;
                for (int x = -xoff; x < panelMap.Width + xoff; x += 48)
                {
                    for (int y = -yoff; y < panelMap.Width + yoff; y += 34)
                    {
                        Point tile = TilePixelConvertor.PixelToTile(x + baseOffside.X, y + baseOffside.Y);
                        int val = 1;
                        if (tile.Y >= 0 && tile.X >= 0)
                        {
                            if (tile.Y * col + tile.X >= blockInfos.Count)
                                val = 0;
                            else
                                val = blockInfos[tile.Y * col + tile.X];
                        }
                        MapPainter.DrawRectTile(e.Graphics, x, y, val);

/*                        tile = TilePixelConvertor.PixelToTile(i + 32, j + 16);
                        val = 1;
                        if (tile.Y >= 0 && tile.X >= 0)
                        {
                            if (tile.Y * col + tile.X >= blockInfos.Count)
                                val = 2;
                            else
                                val = blockInfos[tile.Y * col + tile.X];
                        }
                        MapPainter.DrawRectTile(e.Graphics, i - baseOffside.X + 32, j - baseOffside.Y + 16, val);*/
                    }
                }
            }

            if (false && showGridLine)
            { //show region
                int col = (int)Math.Round((double)selectMapSize.Width / 64);

                OldDataPack pack = OldDataPackBook.GetPack("cfg_map_region");
                int mapidindex = pack.GetPackIndexByName("mapid");
                int mapminxindex = pack.GetPackIndexByName("min_x");
                int mapminyindex = pack.GetPackIndexByName("min_y");
                int mapmaxxindex = pack.GetPackIndexByName("max_x");
                int mapmaxyindex = pack.GetPackIndexByName("max_y");
                int typeindex = pack.GetPackIndexByName("type");

                List<List<String>> regions = new List<List<string>>();
                foreach (List<String> rect in pack.data.Values)
                {
                    if (int.Parse(rect[mapidindex]) == selectMapId)
                    {
                        regions.Add(rect);
                    }
                }

                Dictionary<int, bool> showname = new Dictionary<int, bool>();
                for (int i = baseOffside.X / 64 * 64; i < baseOffside.X + panelMap.Width; i += 64)
                {
                    for (int j = baseOffside.Y / 32 * 32; j < baseOffside.Y + panelMap.Height; j += 32)
                    {
                        foreach (List<String> region in regions)
                        {
                            Point tile = TilePixelConvertor.PixelToTile(i, j);
                            if (TilePixelConvertor.InRegion(int.Parse(region[mapminxindex]), int.Parse(region[mapminyindex]), int.Parse(region[mapmaxxindex]), int.Parse(region[mapmaxyindex]), tile.X, tile.Y))
                            {
                                MapPainter.DrawCircleRegion(e.Graphics, i - baseOffside.X, j - baseOffside.Y, int.Parse(region[typeindex]));

                                if (tile.X >= (int.Parse(region[mapminxindex]) + int.Parse(region[mapmaxxindex])) / 2 && tile.Y >= (int.Parse(region[mapminyindex]) + int.Parse(region[mapmaxyindex])) / 2)
                                {
                                    if (!showname.ContainsKey(int.Parse(region[0])))
                                    {
                                        Font fontSong = new Font("宋体", 14, FontStyle.Bold);
                                        e.Graphics.DrawString(region[1], fontSong, Brushes.Black, i - baseOffside.X - 10, j - baseOffside.Y - 4);
                                        fontSong.Dispose();
                                        showname.Add(int.Parse(region[0]), true);
                                    }
                                }
                            }

                            tile = TilePixelConvertor.PixelToTile(i + 32, j + 16);
                            if (TilePixelConvertor.InRegion(int.Parse(region[mapminxindex]), int.Parse(region[mapminyindex]), int.Parse(region[mapmaxxindex]), int.Parse(region[mapmaxyindex]), tile.X, tile.Y))
                            {
                                MapPainter.DrawCircleRegion(e.Graphics, i - baseOffside.X + 32, j - baseOffside.Y + 16, int.Parse(region[typeindex]));
                            }
                        }
                    }
                }
            }

            if (showGridLine) //是否显示网格
            {
                int xoff = baseOffside.X % 48;
                int yoff = baseOffside.Y % 34;
                for (int i = 1; i * 48 - xoff < panelMap.Width; ++i)
                {
                    e.Graphics.DrawLine(Pens.DarkGray, i * 48 - xoff, 0, i * 48 - xoff, panelMap.Height);
                }
                for (int i = 1; i * 34 - yoff < panelMap.Height; ++i)
                {
                    e.Graphics.DrawLine(Pens.DarkGray, 0, i * 34 - yoff, panelMap.Width, i * 34 - yoff);
                }
            }

            if (false)
            {//传送点
                OldDataPack pack = OldDataPackBook.GetPack("cfg_map_rect");
                int frommapindex = pack.GetPackIndexByName("from_map");
                int fromxindex = pack.GetPackIndexByName("from_x");
                int fromyindex = pack.GetPackIndexByName("from_y");
                int tomapindex = pack.GetPackIndexByName("to_map");

                Font fontSong = new Font("宋体", 11, FontStyle.Bold);

                foreach (List<String> rect in pack.data.Values)
                {
                    int frommap = int.Parse(rect[frommapindex]);
                    int fromx = int.Parse(rect[fromxindex]);
                    int fromy = int.Parse(rect[fromyindex]);
                    int tomap = int.Parse(rect[tomapindex]);

                    Rectangle dest = new Rectangle(baseOffside.X, baseOffside.Y, panelMap.Width, panelMap.Height);
                    Point mpos = TilePixelConvertor.TileToPixel(fromx, fromy);

                    if (selectMapId == frommap && dest.Contains(mpos))
                    {
                        Image body = Image.FromFile("icons/rect.png");
                        e.Graphics.DrawImage(body, mpos.X - baseOffside.X - body.Width / 2, mpos.Y - baseOffside.Y - body.Height / 2, body.Width, body.Height);
                        body.Dispose();

                        DataPack packm = DataPackBook.GetPack("map");
                        int nameindex = packm.GetPackIndexByName("name");
                        String name = packm.data[tomap][nameindex];
                        e.Graphics.DrawString(name, fontSong, Brushes.Black, mpos.X - baseOffside.X - 10, mpos.Y - baseOffside.Y - 4);
                        e.Graphics.DrawString(name, fontSong, Brushes.Lime, mpos.X - baseOffside.X - 11, mpos.Y - baseOffside.Y - 5);
                    }
                }

                fontSong.Dispose();
            }

            if (false && showPlant) //显示植物
            {
                OldDataPack pack = OldDataPackBook.GetPack("cfg_map_plant");
                int idindex = pack.GetPackIndexByName("id");
                int mapidindex = pack.GetPackIndexByName("mapid");
                int plantidindex = pack.GetPackIndexByName("plantid");
                int xindex = pack.GetPackIndexByName("x");
                int yindex = pack.GetPackIndexByName("y");

                OldDataPack mpack = OldDataPackBook.GetPack("cfg_plant");
                int bodyindex = mpack.GetPackIndexByName("body");

                Font fontSong = new Font("宋体", 11, FontStyle.Bold);
                foreach (List<String> plant in pack.data.Values)
                {
                    int id = int.Parse(plant[idindex]);
                    int map = int.Parse(plant[mapidindex]);
                    int x = int.Parse(plant[xindex]);
                    int y = int.Parse(plant[yindex]);
                    int plantid = int.Parse(plant[plantidindex]);

                    Rectangle dest = new Rectangle(baseOffside.X, baseOffside.Y, panelMap.Width, panelMap.Height);

                    Point mpos = TilePixelConvertor.TileToPixel(x, y);
                    if (map == selectMapId && dest.Contains(mpos))
                    {
                        string bodyurl = mpack.data[plantid][bodyindex];
                        String imagepath = String.Format("res/entity/units/plant/{0}.png", bodyurl);
                        //Image body = MapPainter.GetNetImage(imagepath);
                        // string imagepath = string.Format("res/map/{0}/{1}_{2}.jpg", selectFolder, j, i);
                        // if (File.Exists(imagepath))
                         {
                             Image image = MapPainter.GetNetImage(imagepath);//new Bitmap(imagepath);
                             e.Graphics.DrawImage(image, mpos.X - baseOffside.X - image.Width / 2, mpos.Y - baseOffside.Y - image.Height, image.Width, image.Height);
                             //body.Dispose();
                             image.Dispose();
                         }

                        e.Graphics.DrawString(id.ToString(), fontSong, Brushes.Black, mpos.X - baseOffside.X - 10, mpos.Y - baseOffside.Y - 4);
                        e.Graphics.DrawString(id.ToString(), fontSong, Brushes.White, mpos.X - baseOffside.X - 11, mpos.Y - baseOffside.Y - 5);
                    }
                }

                fontSong.Dispose();
            }

            if (showMonster) //是否绘制怪物
            {
                int monnum = 0;

                DataPack pack = DataPackBook.GetPack("map_monster");
                int idindex = pack.GetPackIndexByName("id");
                int mapidindex = pack.GetPackIndexByName("map_id");
                int monstergroupidindex = pack.GetPackIndexByName("monster_group_id");
                int xindex = pack.GetPackIndexByName("x");
                int yindex = pack.GetPackIndexByName("y");

                DataPack mpack = DataPackBook.GetPack("monster");
                int groupindex = mpack.GetPackIndexByName("group_id");
                int monsterbodyindex = mpack.GetPackIndexByName("monster_body");
                int avatarbodyindex = mpack.GetPackIndexByName("avatar_body");

                Font fontSong = new Font("宋体", 11, FontStyle.Bold);
                foreach (List<String> mapmonster in pack.data.Values)
                {
                    int id = int.Parse(mapmonster[idindex]);
                    int map = int.Parse(mapmonster[mapidindex]);
                    int x = int.Parse(mapmonster[xindex]);
                    int y = int.Parse(mapmonster[yindex]);
                    int monstergroupid = int.Parse(mapmonster[monstergroupidindex]);

                    Rectangle dest = new Rectangle(baseOffside.X, baseOffside.Y, panelMap.Width, panelMap.Height);

                    Point mpos = TilePixelConvertor.TileToPixel(x, y);
                    if (map == selectMapId && dest.Contains(mpos))
                    {
                        foreach (List<String> monster in mpack.data.Values)
                        {
                            int groupid = int.Parse(monster[groupindex]);
                            if (groupid == monstergroupid)
                            {
                                string monsterbodyurl = monster[monsterbodyindex];
                                string avatarbodyurl = monster[avatarbodyindex];
                                string bodyurl = "";
                                if (monsterbodyurl.Length > 0)
                                {
                                    bodyurl = monsterbodyurl;
                                }
                                else
                                {
                                    bodyurl = avatarbodyurl;
                                }
                                String imagepath = String.Format("res/entity/monster/{0}.png", bodyurl);
                                Image body = MapPainter.GetNetImage(imagepath); //new Bitmap(imagepath);//
                                //if (File.Exists(imagepath))
                                {
                                    e.Graphics.DrawImage(body, mpos.X - baseOffside.X - body.Width / 2, mpos.Y - baseOffside.Y - body.Height, body.Width, body.Height);
                                    body.Dispose();
                                }
                                //body.Dispose();

                                e.Graphics.DrawString(id.ToString(), fontSong, Brushes.Black, mpos.X - baseOffside.X - 10, mpos.Y - baseOffside.Y - 4);
                                e.Graphics.DrawString(id.ToString(), fontSong, Brushes.White, mpos.X - baseOffside.X - 11, mpos.Y - baseOffside.Y - 5);

                                monnum++;
                                break;
                            }
                        }
                    }
                }

                String moncount = String.Format("当前屏怪物数:{0}", monnum);
                e.Graphics.DrawString(moncount, fontSong, Brushes.Black, 4, panelMap.Height - 19);
                e.Graphics.DrawString(moncount, fontSong, Brushes.White, 3, panelMap.Height - 20);

                fontSong.Dispose();
            }

            if (showNpc)
            {
                DataPack pack = DataPackBook.GetPack("npc");
                int idindex = pack.GetPackIndexByName("id");
                int mapidindex = pack.GetPackIndexByName("mapid");
                int xindex = pack.GetPackIndexByName("x");
                int yindex = pack.GetPackIndexByName("y");
                int nameindex = pack.GetPackIndexByName("name");
                int bodyindex = pack.GetPackIndexByName("body");

                Font fontSong = new Font("宋体", 11, FontStyle.Bold);
                foreach (List<String> npc in pack.data.Values)
                {
                    int id = int.Parse(npc[idindex]);
                    string map = npc[mapidindex];
                    int x = int.Parse(npc[xindex]);
                    int y = int.Parse(npc[yindex]);
                    string bodyurl = npc[bodyindex];
                    string name = npc[nameindex];

                    Rectangle dest = new Rectangle(baseOffside.X, baseOffside.Y, panelMap.Width, panelMap.Height);

                    Point mpos = TilePixelConvertor.TileToPixel(x, y);
                    if (map.Contains(selectMapId.ToString()) && dest.Contains(mpos))
                    {
                        String imagepath = String.Format("res/entity/npc/{0}.png", bodyurl);
                        Image body = MapPainter.GetNetImage(imagepath);
                        //if (File.Exists(imagepath))
                        {
                         //   Image body =new Bitmap(imagepath);
                            e.Graphics.DrawImage(body, mpos.X - baseOffside.X - body.Width / 2, mpos.Y - baseOffside.Y - body.Height, body.Width, body.Height);
                            body.Dispose();

                        }
                        
                        e.Graphics.DrawString(id + "." + name, fontSong, Brushes.Black, mpos.X - baseOffside.X - 10, mpos.Y - baseOffside.Y - 4);
                        e.Graphics.DrawString(id + "." + name, fontSong, Brushes.White, mpos.X - baseOffside.X - 11, mpos.Y - baseOffside.Y - 5);
                    }
                }

                fontSong.Dispose();
            }

            e.Graphics.FillRectangle(Brushes.Gray, 0, 0, 4, panelMap.Height);
            e.Graphics.FillRectangle(Brushes.Gray, 0, panelMap.Height - 4, panelMap.Width, 4);
            e.Graphics.FillRectangle(Brushes.Orange, 0, baseOffside.Y * panelMap.Height / selectMapSize.Height, 4, panelMap.Height * panelMap.Height / selectMapSize.Height);
            e.Graphics.FillRectangle(Brushes.Orange, baseOffside.X * panelMap.Width / selectMapSize.Width, panelMap.Height - 4, panelMap.Width * panelMap.Width / selectMapSize.Width, 4);
        }

        private void pictureBoxMiniMap_Paint(object sender, PaintEventArgs e)
        {
            if (selectMapSize.Width == 0 || selectMapSize.Height == 0)
                return;

            int ratex = panelMap.Width * 100 / selectMapSize.Width;
            int ratey = panelMap.Height * 100 / selectMapSize.Height;
            int ratex1 = baseOffside.X * 100 / selectMapSize.Width;
            int ratey1 = baseOffside.Y * 100 / selectMapSize.Height;

            Brush sb = new SolidBrush(Color.FromArgb(50, Color.Lime));
            e.Graphics.FillRectangle(sb, pictureBoxMiniMap.Width * ratex1 / 100, pictureBoxMiniMap.Height * ratey1 / 100,
            pictureBoxMiniMap.Width * ratex / 100, pictureBoxMiniMap.Height * ratey / 100);
            sb.Dispose();

            e.Graphics.DrawRectangle(Pens.Lime, pictureBoxMiniMap.Width * ratex1 / 100, pictureBoxMiniMap.Height * ratey1 / 100,
            pictureBoxMiniMap.Width * ratex / 100, pictureBoxMiniMap.Height * ratey / 100);

            Font fontSong = new Font("宋体", 9, FontStyle.Bold);
            e.Graphics.DrawString(selectMapName, fontSong, Brushes.Black, 4, pictureBoxMiniMap.Height - 17);
            e.Graphics.DrawString(selectMapName, fontSong, Brushes.White, 3, pictureBoxMiniMap.Height - 18);
            e.Graphics.DrawString(String.Format("{0}x{1}", selectMapSize.Width, selectMapSize.Height), fontSong, Brushes.Black, pictureBoxMiniMap.Width - 69, pictureBoxMiniMap.Height - 17);
            e.Graphics.DrawString(String.Format("{0}x{1}", selectMapSize.Width, selectMapSize.Height), fontSong, Brushes.Lime, pictureBoxMiniMap.Width - 70, pictureBoxMiniMap.Height - 18);
            fontSong.Dispose();
        }

        private void updateMap()
        {
            panelMap.Invalidate();
            pictureBoxMiniMap.Invalidate();
        }

        private void panelMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                movePoint = new Point(e.X, e.Y);
                mouseDown = true;
            }
        }

        private void panelMap_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && mouseDown)
            {
                int xdiff = e.X - movePoint.X;
                int ydiff = e.Y - movePoint.Y;
                int xtar = baseOffside.X - xdiff;
                int ytar = baseOffside.Y - ydiff;

                setBasePoint(xtar, ytar);

                updateMap();
            }

            mouseDown = false;
        }

        private void setBasePoint(int x, int y)
        {
            int dx = x;
            int dy = y;
            if (dx < 0) dx = 0;
            if (dy < 0) dy = 0;
            if (dx > selectMapSize.Width - panelMap.Width) dx = selectMapSize.Width - panelMap.Width;
            if (dy > selectMapSize.Height - panelMap.Height) dy = selectMapSize.Height - panelMap.Height;

            baseOffside = new Point(dx, dy);
        }

        private void panelMap_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
            Point p = TilePixelConvertor.PixelToTile(baseOffside.X + e.X, baseOffside.Y + e.Y);
            this.toolStripStatusPos.Text = String.Format("x:{0},y:{1} gridX:{2},gridY:{3}", e.X + baseOffside.X, e.Y + baseOffside.Y, p.X, p.Y);
        }

        private void panelMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //摆放怪物
            {
                if (viewStack1.SelectedIndex == 1 && listViewMonster.SelectedItems.Count > 0)
                {
                    String[] data = listViewMonster.SelectedItems[0].Text.Split('.');
                    int monid = int.Parse(data[0]);

                    OldDataPack pack = OldDataPackBook.GetPack("cfg_map_monster");
                    List<String> datas = new List<string>();
                    datas.Add((++pack.maxid).ToString());
                    datas.Add(selectMapId.ToString());
                    datas.Add(monid.ToString());
                    Point p = TilePixelConvertor.PixelToTile(e.X + baseOffside.X, e.Y + baseOffside.Y);
                    datas.Add(p.X.ToString());
                    datas.Add(p.Y.ToString());
                    pack.AddPackData(pack.maxid, datas);

                    this.installLiveMonster();

                    updateMap();
                }
                else if (viewStack1.SelectedIndex == 2 && listViewPlant.SelectedItems.Count > 0)
                {
                    String[] data = listViewPlant.SelectedItems[0].Text.Split('.');
                    int pid = int.Parse(data[0]);

                    OldDataPack pack = OldDataPackBook.GetPack("cfg_map_plant");
                    List<String> datas = new List<string>();
                    datas.Add((++pack.maxid).ToString());
                    datas.Add(selectMapId.ToString());
                    datas.Add(pid.ToString());
                    Point p = TilePixelConvertor.PixelToTile(e.X + baseOffside.X, e.Y + baseOffside.Y);
                    datas.Add(p.X.ToString());
                    datas.Add(p.Y.ToString());
                    pack.AddPackData(pack.maxid, datas);

                    this.installLivePlant();

                    updateMap();
                }
            }
        }

        private void pictureBoxMiniMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (selectMapSize.Width == 0 || selectMapSize.Height == 0)
                return;

            int ratex = panelMap.Width * 100 / selectMapSize.Width;
            int ratey = panelMap.Height * 100 / selectMapSize.Height;

            int minix = e.X - pictureBoxMiniMap.Width * ratex / 200;
            int miniy = e.Y - pictureBoxMiniMap.Height * ratey / 200;
            if (minix < 0) minix = 0;
            if (miniy < 0) miniy = 0;

            setBasePoint(minix * 100 / pictureBoxMiniMap.Width * selectMapSize.Width / 100,
                miniy * 100 / pictureBoxMiniMap.Height * selectMapSize.Height / 100);

            updateMap();
            pictureBoxMiniMap.Focus();
        }

        private void listViewLives_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewLives.SelectedItems.Count <= 0)
                return;

            searchBase = listViewLives.SelectedIndices[0];

            String[] data = listViewLives.SelectedItems[0].Text.Split('.');
            if (lastSelectedLive != null)
            {
                lastSelectedLive.BackColor = System.Drawing.SystemColors.Window;
            }
            listViewLives.SelectedItems[0].BackColor = Color.Gold;
            lastSelectedLive = listViewLives.SelectedItems[0];
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewLives.SelectedItems.Count <= 0)
                return;

            int sel = listViewLives.SelectedIndices[0] - 1;
            if (sel < 0) sel = 0;

            OldDataPack pack = OldDataPackBook.GetPack("cfg_map_monster");
            foreach (ListViewItem lvm in listViewLives.SelectedItems)
            {
                String[] data = lvm.Text.Split('.');
                int monid = int.Parse(data[0]);

                pack.RemovePackData(monid);
            }

            this.installLiveMonster();
            foreach (ListViewItem lvm in listViewLives.Items)
            {
                if (lvm.Index == sel)
                {
                    lvm.Selected = true;
                    lvm.EnsureVisible();
                }
            }

            updateMap();
        }

        private void listViewVegas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewVegas.SelectedItems.Count <= 0)
                return;

            String[] data = listViewVegas.SelectedItems[0].Text.Split('.');
            if (lastSelectedVegas != null)
            {
                lastSelectedVegas.BackColor = System.Drawing.SystemColors.Window;
            }
            listViewVegas.SelectedItems[0].BackColor = Color.Gold;
            lastSelectedVegas = listViewVegas.SelectedItems[0];
        }

        private void toolStripMenuItemRemove2_Click(object sender, EventArgs e)
        {
            if (listViewVegas.SelectedItems.Count <= 0)
                return;

            int sel = listViewVegas.SelectedIndices[0] - 1;
            if (sel < 0) sel = 0;

            OldDataPack pack = OldDataPackBook.GetPack("cfg_map_plant");
            foreach (ListViewItem lvm in listViewVegas.SelectedItems)
            {
                String[] data = lvm.Text.Split('.');
                int pid = int.Parse(data[0]);

                pack.RemovePackData(pid);
            }

            this.installLivePlant();
            foreach (ListViewItem lvm in listViewVegas.Items)
            {
                if (lvm.Index == sel)
                {
                    lvm.Selected = true;
                    lvm.EnsureVisible();
                }
            }

            updateMap();
        }

        #region toolbar
        private void toolStripButtonMap_Click(object sender, EventArgs e)
        {
            toolStripButtonMap.Checked = true;
            toolStripButtonMonster.Checked = false;
            toolStripButtonPlant.Checked = false;
            viewStack1.SelectedIndex = 0;
        }

        private void toolStripButtonMonster_Click(object sender, EventArgs e)
        {
            toolStripButtonMap.Checked = false;
            toolStripButtonMonster.Checked = true;
            toolStripButtonPlant.Checked = false;
            viewStack1.SelectedIndex = 1;
        }

        private void toolStripButtonPlant_Click(object sender, EventArgs e)
        {
            toolStripButtonMap.Checked = false;
            toolStripButtonMonster.Checked = false;
            toolStripButtonPlant.Checked = true;
            viewStack1.SelectedIndex = 2;
        }

        private void toolStripButtonGrid_Click(object sender, EventArgs e)
        {
            showGridLine = !showGridLine;
            toolStripButtonGrid.Checked = showGridLine;

            updateMap();
        }

        private void toolStripButtonBarrier_Click(object sender, EventArgs e)
        {
            showBarrier = !showBarrier;
            toolStripButtonBarrier.Checked = showBarrier;

            updateMap();
        }

        private void toolStripButtonMon_Click(object sender, EventArgs e)
        {
            showMonster = !showMonster;
            toolStripButtonMon.Checked = showMonster;

            updateMap();
        }

        private void toolStripButtonSkin_Click(object sender, EventArgs e)
        {
            showSkin = !showSkin;
            toolStripButtonSkin.Checked = showSkin;

            updateMap();
        }

        private void toolStripButtonNpc_Click(object sender, EventArgs e)
        {
            showNpc = !showNpc;
            toolStripButtonNpc.Checked = showNpc;

            updateMap();
        }

        private void toolStripButtonPlt_Click(object sender, EventArgs e)
        {
            showPlant = !showPlant;
            toolStripButtonPlt.Checked = showPlant;

            updateMap();
        }
        #endregion

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (selectMapId ==0)
                return;

            int xdiff = 0;
            int ydiff = 0;
            switch(e.KeyCode)
            {
                case Keys.Left: SetBaseOff(-100, ydiff); break;
                case Keys.Right: SetBaseOff(100, ydiff); break;
                case Keys.Up: SetBaseOff(xdiff, -100); break;
                case Keys.Down: SetBaseOff(xdiff, 100); break;
                case Keys.C: targets.Add(new Point(baseOffside.X + mouseX, baseOffside.Y + mouseY)); break;
                default: return;
            }

            updateMap();
        }

        private void SetBaseOff(int xdiff, int ydiff)
        {
            int xtar = baseOffside.X + xdiff;
            int ytar = baseOffside.Y + ydiff;

            setBasePoint(xtar, ytar);
        }

        private void oldMapFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OldDataEditForm edit = new OldDataEditForm();
            edit.PackName = (sender as ToolStripMenuItem).Tag.ToString();
            edit.Show();
        }

        private void mapFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataEditForm edit = new DataEditForm();
            edit.PackName = (sender as ToolStripMenuItem).Tag.ToString();
            edit.Show();
        }

        private void item2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataViewForm dvf = new DataViewForm();
            dvf.PackName = (sender as ToolStripMenuItem).Tag.ToString();
            dvf.Show();
        }

        private void oldDropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OldBiDataEditForm edit = new OldBiDataEditForm();
            edit.PackName = (sender as ToolStripMenuItem).Tag.ToString();
            edit.Show();
        }

        private void dropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BiDataEditForm edit = new BiDataEditForm();
            edit.PackName = (sender as ToolStripMenuItem).Tag.ToString();
            edit.Show();
        }

        private void oldGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void groupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriDataEditForm edit = new TriDataEditForm();
            edit.PackName = (sender as ToolStripMenuItem).Tag.ToString();
            edit.Show();
        }

        private void movieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MovieEditForm edit = new MovieEditForm();
            edit.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region Search Blok
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            selectNext();
        }

        private void textBoxInfo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                selectNext();
            }
        }

        private void selectNext()
        {
            if (textBoxInfo.Text == "")
                return;

            int count = listViewLives.Items.Count;
            for (int i = 0; i < count; i++)
            {
                int newindex = (searchBase + i) % count;
                if (listViewLives.Items[newindex].Text.Contains(textBoxInfo.Text))
                {
                    listViewLives.Items[newindex].Selected = true;
                    listViewLives.Items[newindex].EnsureVisible();
                    searchBase = (newindex + 1) % count;
                    break;
                }
            }
        }
        #endregion

        private void toolStripSeparator1_Click(object sender, EventArgs e)
        {

        }

        private void guessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OldDataEditForm edit = new OldDataEditForm();
            edit.PackName = (sender as ToolStripMenuItem).Tag.ToString();
            edit.Show();
        }

        private void 普通活动扩展ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OldDataEditForm edit = new OldDataEditForm();
            edit.PackName = (sender as ToolStripMenuItem).Tag.ToString();
            edit.Show();
        }

        private void formsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 个性话名字ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mapForToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
   
}
