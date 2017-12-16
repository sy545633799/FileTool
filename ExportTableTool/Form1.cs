using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ExportTableTool.ExecleOperation;
using NPOI.SS.UserModel;

namespace ExportTableTool
{
    public enum GenerateType
    {
        Null,
        Client = 1,
        Server = 2,
        PB = 3
    }

    public partial class Form1 : Form
    {

        public Action<List<ISheet>> completeCallback;

        public Form1()
        {
            completeCallback = ExecleData;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Dictionary<string,string> paths = ExecleManager.Instance.LoadDefultConfig(Application.StartupPath);
            Dictionary<string,List<string>> pathsNew = ExecleManager.Instance.LoadDefultConfigNew(Application.StartupPath);

            if(null != pathsNew)
            {
                if (pathsNew.ContainsKey(ConfigConst.ClientDllOutPath))
                {
                    List<string> pList = pathsNew[ConfigConst.ClientDllOutPath];
                    foreach(var p in pList)
                    {
                        listBox1.Items.Add(p);
                    }
                }
                if (pathsNew.ContainsKey(ConfigConst.ClientBinOutPath))
                {
                    List<string> pList = pathsNew[ConfigConst.ClientBinOutPath];
                    foreach (var p in pList)
                    {
                        listBox2.Items.Add(p);
                    }
                }
                if (pathsNew.ContainsKey(ConfigConst.BattleConfigFileOutPath))
                {
                    List<string> pList = pathsNew[ConfigConst.BattleConfigFileOutPath];
                    foreach (var p in pList)
                    {
                        listBox3.Items.Add(p);
                    }
                }
                if (pathsNew.ContainsKey(ConfigConst.BattleBinOutPath))
                {
                    List<string> pList = pathsNew[ConfigConst.BattleBinOutPath];
                    foreach (var p in pList)
                    {
                        listBox4.Items.Add(p);
                    }
                }
            }

            if (null != paths)
            {
                if (paths.ContainsKey(ConfigConst.InputPath))
                {
                    string path = paths[ConfigConst.InputPath];
                    label3.Text = path;
                    ExecleManager.Instance.LoadExecle(path, completeCallback);
                }
                if (paths.ContainsKey(ConfigConst.OutPath))
                {
                    string path = paths[ConfigConst.OutPath];
                    label4.Text = path;
                }
                if (paths.ContainsKey(ConfigConst.DependPath))
                {
                    string path = paths[ConfigConst.DependPath];
                    label6.Text = path;
                }
                if (paths.ContainsKey(ConfigConst.OutPathSources))
                {
                    string path = paths[ConfigConst.OutPathSources];
                    label8.Text = path;
                }
                if (paths.ContainsKey(ConfigConst.ProtoBuffPath))
                {
                    label15.Text = paths[ConfigConst.ProtoBuffPath];
                }
            }
            Init();
        }

        private void Init()
        {
            comboBox1.SelectedIndex = 0;
            checkBox2.CheckState = CheckState.Checked;
            progressBar1.Visible = false;
        }

        private void ExecleData(List<ISheet> tables)
        {
            checkedListBox1.Items.Clear();
            foreach (var table in tables)
            {
                checkedListBox1.Items.Add(table.SheetName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GenerateType generateType = GenerateType.Null;

            if (checkBox2.CheckState == CheckState.Checked && checkBox3.CheckState == CheckState.Checked)
            {
                generateType = GenerateType.Client | GenerateType.Server;

            }else if (checkBox2.CheckState == CheckState.Checked)
            {
                generateType = GenerateType.Client;

            }else if (checkBox3.CheckState == CheckState.Checked)
            {
                generateType = GenerateType.Server;

            }else if (checkBox2.CheckState == CheckState.Unchecked && checkBox3.CheckState == CheckState.Unchecked && checkBox4.CheckState == CheckState.Unchecked)
            {
                MessageBox.Show("请选择导出");
                return;
            }

            AgainCheckExeclData();

            richTextBox1.AppendText("导表进程:\n");
            ExecleManager.Instance.Generate(GenerateProgress, generateType, GenerateComplete, SerializableProgress);
        }

        private void AgainCheckExeclData()
        {
            ExecleManager.Instance.Clear();
            checkedListBox1.Items.Clear();
            string inputPath = ExecleManager.Instance.GetInputPath();
            if (!string.IsNullOrEmpty(inputPath))
            {
                ExecleManager.Instance.LoadExecle(inputPath, completeCallback);
                SelectGenerateItem(checkBox1.CheckState = CheckState.Checked);
            }            
        }


        private void GenerateProgress(ISheet sheet)
        {
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = Color.Green;
            richTextBox1.AppendText(sheet.SheetName + "\t表结构生成---------------->\tOK\n");
            richTextBox1.SelectionColor = richTextBox1.ForeColor;
        }

        private void SerializableProgress(string msg)
        {
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = Color.Red;
            richTextBox1.AppendText(msg+"\n");
            richTextBox1.SelectionColor = richTextBox1.ForeColor;
        }

        private void GenerateComplete()
        {
            richTextBox1.AppendText("<-----------------------------完成---------------------------->\n");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SelectGenerateItem(checkBox1.CheckState);
        }

        private void SelectGenerateItem(CheckState cs)
        {
            for (var i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemCheckState(i, cs);
                ItemClickState(checkedListBox1.GetItemCheckState(i), checkedListBox1.Items[i].ToString());
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var clickItem = (CheckedListBox) sender;
            ItemClickState(clickItem.GetItemCheckState(clickItem.SelectedIndex), (string) clickItem.SelectedItem);
        }

        private void ItemClickState(CheckState cs, string table)
        {
            if (cs == CheckState.Checked)
                ExecleManager.Instance.AddGenerate(table);
            else
                ExecleManager.Instance.RemoveGenerate(table);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
          
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ExecleManager.Instance.SavePath(Application.StartupPath,ConfigConst.InputPath, fbd.SelectedPath);
                label3.Text = fbd.SelectedPath;
                ExecleManager.Instance.LoadExecle(fbd.SelectedPath, completeCallback);
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void 输出目录路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                label4.Text = fbd.SelectedPath;
                ExecleManager.Instance.SavePath(Application.StartupPath, ConfigConst.OutPath, fbd.SelectedPath);
            }
        }

        private void 输入目录依赖ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileDialog fd = new OpenFileDialog();
            fd.FileOk += FileOkDialog;
            fd.ShowDialog();

        }

        private void FileOkDialog(object sender, CancelEventArgs e)
        {
            label6.Text = ((OpenFileDialog)sender).FileName;
            ExecleManager.Instance.SavePath(Application.StartupPath, ConfigConst.DependPath, label6.Text);
            ((OpenFileDialog) sender).FileOk -= FileOkDialog;
        }

        private void 源文件输出路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                label8.Text = fbd.SelectedPath;
                ExecleManager.Instance.SavePath(Application.StartupPath, ConfigConst.OutPathSources, fbd.SelectedPath);
            }
        }
        //增加客户端DLL输出路径
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
               string path =  fbd.SelectedPath;
               listBox1.Items.Add(path);
               ExecleManager.Instance.SavePathNew(Application.StartupPath,ConfigConst.ClientDllOutPath, path);
            }
        }
        //删除客户端DLL输出路径
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string p = (string)listBox1.SelectedItem;
                ExecleManager.Instance.RemovePath(Application.StartupPath,ConfigConst.ClientDllOutPath,p);
                listBox1.Items.Remove(p);
            }
        }
        //增加客户端Bin输出路径
        private void button7_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string path = fbd.SelectedPath;
                listBox2.Items.Add(path);
                ExecleManager.Instance.SavePathNew(Application.StartupPath, ConfigConst.ClientBinOutPath, path);
            }
        }
        //删除客户端Bin输出路径
        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                string p = (string)listBox2.SelectedItem;
                ExecleManager.Instance.RemovePath(Application.StartupPath, ConfigConst.ClientBinOutPath, p);
                listBox2.Items.Remove(p);
                
            }
        }
        //增加战斗服结构文件输出路径
        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string path = fbd.SelectedPath;
                listBox3.Items.Add(path);
                ExecleManager.Instance.SavePathNew(Application.StartupPath, ConfigConst.BattleConfigFileOutPath, path);
            }
        }
        //删除战斗服结构文件输出路径
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                string p = (string)listBox3.SelectedItem;
                listBox3.Items.Remove(p);
                ExecleManager.Instance.RemovePath(Application.StartupPath, ConfigConst.BattleConfigFileOutPath, p);
            }
        }
        //增加战斗服Bin文件输出路径
        private void button9_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string path = fbd.SelectedPath;
                listBox4.Items.Add(path);
                ExecleManager.Instance.SavePathNew(Application.StartupPath, ConfigConst.BattleBinOutPath, path);
            }
        }
        //删除战斗服Bin文件输出路径
        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex != -1)
            {
                string p = (string)listBox4.SelectedItem;
                listBox4.Items.RemoveAt(listBox4.SelectedIndex);
                ExecleManager.Instance.RemovePath(Application.StartupPath, ConfigConst.BattleBinOutPath, p);
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {            
            ExecleManager.Instance.GenerateProtobuf = checkBox4.Checked;
        }

        private void pB路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                label15.Text = ofd.FileName;
                ExecleManager.Instance.SavePath(Application.StartupPath, ConfigConst.ProtoBuffPath, ofd.FileName);
            }
        }
    }
}