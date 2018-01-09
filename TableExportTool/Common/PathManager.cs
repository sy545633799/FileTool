using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using FileUtility;
using System;

namespace TableExportTool
{
    public class PathManager
    {
        private static string configPath = Path.Combine(Application.StartupPath, "pathconfig.txt");
        public static FilePath filePath;

        public static void LoadFilePath(Action callback)
        {
            try
            {
                filePath = TxtUtil.ReadFromPath<FilePath>(configPath);
            }
            catch
            {
                filePath = new FilePath();
            }
            callback?.Invoke();
        }

        public static void SaveFilePath()
        {
            if (filePath != null)
                TxtUtil.SaveToPath(configPath, filePath, false);
        }

        public static void SelectPath(Action<string> callback)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                callback(fbd.SelectedPath);
                SaveFilePath();
            }
        }
    }
}
