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

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            ExcelConvert.ToTxt(sheetDic, PathManager.filePath.OutputBinPath, () => MessageBox.Show("导出完成"));
            //TableManager.InitTxtTable(TxtUtil.ReadAllFile(PathManager.filePath.OutputPath));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PathManager.LoadFilePath(() => sheetDic = ExcelUtil.LoadExcel(PathManager.filePath.ImportPath));
            label1.Text = "输入路径：" + PathManager.filePath.ImportPath;
            label2.Text = "源文件(txt/bin)输出路径：" + PathManager.filePath.OutputBinPath;
            label3.Text = "依赖文件路径：" + PathManager.filePath.DependentFilePath;
            label4.Text = "源文件(cs)输出路径：" + PathManager.filePath.OutputCSPath;
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

        #region 路径设置
        private void 读取ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PathManager.SelectPath(path =>
            {
                PathManager.filePath.ImportPath = path;
                sheetDic = ExcelUtil.LoadExcel(PathManager.filePath.ImportPath);
                label1.Text = "输入路径：" + path;
                RefreshMenuItem();
            });
        }

        private void 设置依赖文件路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PathManager.SelectPath(path =>
            {
                PathManager.filePath.DependentFilePath = path;
                label3.Text = "依赖文件路径：" + path;
            });
        }

        private void 设置输出目录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PathManager.SelectPath(path =>
            {
                PathManager.filePath.OutputBinPath = path;
                label2.Text = "源文件(txt/bin)输出路径：" + path;
            });
        }

        private void 设置源文件输出路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PathManager.SelectPath(path =>
            {
                PathManager.filePath.OutputCSPath = path;
                label4.Text = "源文件输出(cs)路径：" + path;
            });
        }

        #endregion

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
