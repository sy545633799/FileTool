using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileUtility;
using NPOI.SS.UserModel;

namespace TableExportTool
{
    public partial class Form1 : Form
    {
        private Dictionary<string, List<ISheet>> sheetDic;
        public static Action<bool, string> Log;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText("导表进程:\n");
            FilePath filePath = PathManager.filePath;
            if (Directory.Exists(filePath.OutputPath))
            {
                //导出cs文件
                ExportUtil.ExportCSAccordingMultipleTxt(sheetDic, Path.Combine(filePath.OutputPath, "CSFile"));
                //拷贝依赖文件
                string[] fileInfo = Directory.GetFiles(filePath.DependentFilePath);
                foreach (var path in fileInfo)
                    File.Copy(path, Path.Combine(filePath.OutputPath, "CSFile", Path.GetFileName(path)), true);
                //导出dll文件
                string dllPath = ExportUtil.ExportDllAccordingTxt(Path.Combine(filePath.OutputPath, "CSFile"), Path.Combine(filePath.OutputPath, "DLL"));
                //拷贝dll到项目文件夹
                foreach (var path in filePath.OutPutDll)
                    File.Copy(dllPath, Path.Combine(path, Path.GetFileName(dllPath)));
                //导出配置文件
                ExportUtil.ExportTxt(sheetDic, Path.Combine(filePath.OutputPath, "Config"));
                //拷贝txt配置问价到项目文件夹
                foreach (var path in filePath.OutPutBin)
                    DirectoryHelper.CopyContent(Path.Combine(filePath.OutputPath, "Config"), path);
            }
            else Log(false, "请配置文件输出目录");

            richTextBox1.AppendText("<-----------------------------完成---------------------------->\n");
            MessageBox.Show("导表完成");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Log = SerializableProgress;
            FilePath filePath = PathManager.LoadFilePath(() => sheetDic = ExcelHelper.LoadExcel(PathManager.filePath.ImportPath));
            label1.Text = "输入路径：" + filePath.ImportPath;
            label2.Text = "源文件输出路径：" + filePath.OutputPath;
            label3.Text = "依赖文件路径：" + filePath.DependentFilePath;

            foreach (var item in filePath.OutPutDll)
                listBox1.Items.Add(item);
            foreach (var item in filePath.OutPutBin)
                listBox2.Items.Add(item);
            
            RefreshMenuItem();
        }

        private void RefreshMenuItem()
        {
            checkedListBox1.Items.Clear();
            foreach (var sheets in sheetDic.Values)
            {
                foreach (var sheet in sheets)
                    checkedListBox1.Items.Add(sheet.SheetName);
            }
        }

        public static void SelectPath(Action<string> callback)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                callback(fbd.SelectedPath);
                PathManager.SaveFilePath();
            }
        }

        private void SerializableProgress(bool sucess, string msg)
        {
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = sucess ? Color.Green : Color.Red;
            richTextBox1.AppendText(msg + "\n");
            richTextBox1.SelectionColor = richTextBox1.ForeColor;
        }

        #region 路径设置
        private void 读取ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectPath(path =>
            {
                PathManager.filePath.ImportPath = path;
                sheetDic = ExcelHelper.LoadExcel(PathManager.filePath.ImportPath);
                label1.Text = "输入路径：" + path;
                RefreshMenuItem();
            });
        }

        private void 设置依赖文件路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectPath(path =>
            {
                PathManager.filePath.DependentFilePath = path;
                label3.Text = "依赖文件路径：" + path;
            });
        }

        private void 设置输出目录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectPath(path =>
            {
                PathManager.filePath.OutputPath = path;
                label2.Text = "源文件输出路径：" + path;
            });
        }

        #endregion

        //添加dll文件输出路径
        private void button2_Click(object sender, EventArgs e)
        {
            SelectPath(path =>
            {
                PathManager.filePath.OutPutDll.Add(path);
                listBox1.Items.Add(path);
            });
        }
        //删除dll文件输出路径
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string path = (string)listBox1.SelectedItem;
                PathManager.filePath.OutPutDll.Remove(path);
                listBox1.Items.Remove(path);
                PathManager.SaveFilePath();
            }
        }
        //添加txt/bin文件输出路径
        private void button4_Click(object sender, EventArgs e)
        {
            SelectPath(path =>
            {
                PathManager.filePath.OutPutBin.Add(path);
                listBox2.Items.Add(path);
            });
        }
        //删除txt/bin文件输出路径
        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                string path = (string)listBox2.SelectedItem;
                PathManager.filePath.OutPutBin.Remove(path);
                listBox2.Items.Remove(path);
                PathManager.SaveFilePath();
            }
        }
    }
}
