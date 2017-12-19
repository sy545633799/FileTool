using System;
using System.Collections.Generic;
using System.IO;
using StarEdit.MysqlServices;
using StarEdit.PaintServices;

namespace StarEdit.Tools
{
    class DirectoryTool
    {
        public static List<String> GetAllFileName(String montherpath, String ext)
        {
            int nlenth = montherpath.Length;
            string urlstr = montherpath.Substring(4, nlenth - 4);
            string httpPath = MysqlService.getResPath();
            string path = string.Format("{0}{1}", httpPath, urlstr);

            List<String> names = new List<string>();
            WebPage page = new WebPage(path);
            foreach (Link lk in page.Links)
            {
                if (lk.Text.EndsWith(ext))
                    names.Add(lk.Text);
            }
            return names;
        }
    }
}
