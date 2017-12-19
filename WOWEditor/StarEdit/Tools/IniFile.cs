using System.Runtime.InteropServices;
using System.Text;

namespace StarEdit.Tools
{
    class IniFile
    {
        private string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public IniFile(string iniPath)
        {
            path = iniPath;
        }

        public void IniWriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, this.path);
        }

        public string IniReadValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(8192);
            long i = GetPrivateProfileString(section, key, "", temp, 8192, this.path);
            return temp.ToString();
        }
    }
}
