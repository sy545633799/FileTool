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

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PathManager.LoadFilePath(() => ExcelManager.LoadExcel());
            label1.Text = "输入路径：" + PathManager.filePath.ImportPath;
            label2.Text = "输出路径：" + PathManager.filePath.OutputPath;
            label3.Text = "依赖文件路径：" + PathManager.filePath.DependentFilePath;
            label4.Text = "源文件输出路径：" + PathManager.filePath.OutputPath;
        }

#region 路径设置
        private void 读取ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PathManager.SelectPath(path =>
            {
                PathManager.filePath.ImportPath = path;
                List<ISheet> tables = ExcelManager.LoadExcel();
                label1.Text = "输入路径：" + path;
                //刷新列表
                if (tables != null)
                {
                    checkedListBox1.Items.Clear();
                    foreach (var table in tables)
                    {
                        checkedListBox1.Items.Add(table.SheetName);
                    }
                }
            });
        }

        private void 设置输出目录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PathManager.SelectPath(path =>
            {
                PathManager.filePath.OutputPath = path;
                label2.Text = "输出路径：" + path;
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

        private void 设置源文件输出路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PathManager.SelectPath(path =>
            {
                PathManager.filePath.OutputSourcePath = path;
                label4.Text = "源文件路径：" + path;
            });
        }
        #endregion

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
