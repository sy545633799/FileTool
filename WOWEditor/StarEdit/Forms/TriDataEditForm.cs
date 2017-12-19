using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using StarEdit.Interface;
using StarEdit.MyComponents;
using StarEdit.DataServices;
using StarEdit.Tools;
using MessageBoxExLib;

namespace StarEdit.Forms
{
    public partial class TriDataEditForm : Form
    {
        TriDataPack pack;
        int selectId;
        int searchBase;
        string packName;
        ListViewItem lastSelected;
        int nameIndex = 1;

        public string PackName
        {
            get { return packName; }
            set { packName = value; }
        }

        public TriDataEditForm()
        {
            InitializeComponent();
        }

        private void DataEditForm_Load(object sender, EventArgs e)
        {
            pack = TriDataPackBook.GetPack(packName);

            refreshList(0);
        }

        private void refreshList(int selid)
        {
            int index = 0;
            listViewIds.Items.Clear();

            IniFile configData = new IniFile(String.Format("config/db/{0}.ini", packName));
            string firstbind = configData.IniReadValue("main", "firstbind");
            string nameindex = configData.IniReadValue("main", "nameindex");
            if (nameindex != "")
            {
                nameIndex = int.Parse(nameindex);
            }

            foreach (int id in pack.keys.Keys)
            {
                string name = "";
                if (firstbind != "")
                {
                    name = DataPackBook.GetPack(firstbind).data[id][nameIndex];
                }
                ListViewItem lvm = new ListViewItem(String.Format("{0}.{1}({2}项)", id, name, pack.keys[id]));
                listViewIds.Items.Add(lvm);
                if ((index == selid && index == 0) || selid == id)
                {
                    lvm.Selected = true;
                    lvm.EnsureVisible();
                }
                index++;
            }
            this.Text = String.Format("{0} (总计{1}条)", packName, pack.keys.Count);
        }

        private void setControlValue(IAutoControl con, String value)
        {
            con.Value = value;
        }

        private String getControlValue(IAutoControl con)
        {
            return con.Value;
        }

        private void listViewIds_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewIds.SelectedItems.Count <= 0)
                return;

            panelEditZone.Controls.Clear();

            if (lastSelected != null)
            {
                lastSelected.BackColor = System.Drawing.SystemColors.Window;
            }
            listViewIds.SelectedItems[0].BackColor = Color.Gold;
            lastSelected = listViewIds.SelectedItems[0];

            IniFile configData = new IniFile(String.Format("config/db/{0}.ini", packName));
            String[] data = listViewIds.SelectedItems[0].Text.Split('.');
            selectId = int.Parse(data[0]);

            int index = 0;
            int row = 0;
            int col = 0;

            foreach (TriData bikey in pack.data.Keys)
            {
                if (bikey.a != selectId)
                    continue;

                index = 0;
                int wid = panelEditZone.Width / 2;
                Point offside = new Point(8, 20);
                Font fontsong = new Font("宋体", 10, FontStyle.Regular);
                foreach (String head in pack.headers)
                {
                    int indx = pack.GetPackIndexByName(head);
                    if (indx == 0)
                        continue;

                    #region 构建控件
                    Label lbl = new Label();
                    lbl.Location = new Point(col * wid + offside.X, row * 30 + offside.Y);
                    lbl.Text = head;
                    lbl.AutoSize = true;
                    lbl.Font = fontsong;
                    lbl.Name = "lbl" + head;
                    panelEditZone.Controls.Add(lbl);

                    Label lbl2 = new Label();
                    lbl2.ForeColor = Color.DarkBlue;
                    lbl2.Location = new Point(lbl.Location.X + lbl.Width + 5, row * 30 + 3 + offside.Y);
                    lbl2.Text = pack.comment[indx];
                    int wordscap = (165 - lbl.Width) / 16;
                    if (lbl2.Text.Length > wordscap)
                    {
                        if (wordscap <= 0)
                        {
                            lbl2.Text = "";
                        }
                        else
                        {
                            lbl2.Text = String.Format("{0}...", lbl2.Text.Substring(0, wordscap - 1));
                        }
                    }
                    lbl2.AutoSize = true;
                    panelEditZone.Controls.Add(lbl2);

                    string enums = configData.IniReadValue(head, "enums");
                    string bind = configData.IniReadValue(head, "bind");
                    string groupitem = configData.IniReadValue(head, "groupitem");
                    string itemgroup = configData.IniReadValue(head, "itemgroup");
                    string hide = configData.IniReadValue(head, "hide");

                    Control c;
                    if (enums != "") //combox
                    {
                        c = new UCComboBox();
                        (c as UCComboBox).Info = enums;
                    }
                    else if (bind != "") //boxlabel
                    {
                        c = new UCBoxLabel();
                        (c as UCBoxLabel).TableName = bind;
                    }
                    else if (groupitem != "")
                    {
                        c = new UCGroupItem();
                    }
                    else if (itemgroup != "")
                    {
                        c = new UCItem();
                        int indx2 = pack.GetPackIndexByName(itemgroup);
                        (c as UCItem).Type = int.Parse(pack.data[bikey][indx2]);
                    }
                    else if (hide != "")
                    {
                        row++;
                        col = 0;
                        lbl.Visible = false;
                        lbl2.Visible = false;
                        continue;
                    }
                    else //textbox
                    {
                        c = new UCTextBox();
                        if (pack.dataTypes[indx] == "char")
                        {
                            (c as UCTextBox).TextSize = pack.dataSizes[indx];
                        }
                    }
                    c.Enabled = (indx > 2);
                    c.Name = String.Format("{0}-{1}-{2}", head, bikey.b, bikey.c);
                    c.Location = new Point(col * wid + 170 + offside.X, row * 30 + offside.Y);
                    if (c.Height > 30)
                    {
                        row += c.Height / 30;
                        col = 0;
                    }
                    panelEditZone.Controls.Add(c);
                    #endregion

                    setControlValue(c as IAutoControl, pack.data[bikey][indx]);

                    index++;
                    if (col == 0 && row == 0)
                    {
                        row++;
                    }
                    else if (index < pack.dataTypes.Count && (pack.comment[index].Contains("[NL]") || pack.comment[index - 1].Contains("[NL]"))) //特殊标签
                    {
                        row++;
                        col = 0;
                    }
                    else if (indx < pack.dataTypes.Count && col == 0 && (pack.dataTypes[indx] != "char" || pack.dataSizes[indx] <= 8) && (pack.dataTypes[indx - 1] != "char" || pack.dataSizes[indx - 1] <= 8))
                    {
                        col++;
                    }
                    else
                    {
                        row++;
                        col = 0;
                    }
                }

                row += 2;
                col = 0;
                fontsong.Dispose();
            }

            Button tb = new Button();
            tb.Text = "新建";
            tb.Location = new Point(30, row * 30);
            tb.Click += buttonAdd_Click;
            panelEditZone.Controls.Add(tb);

            tb = new Button();
            tb.Text = "删除";
            tb.Location = new Point(130, row * 30);
            tb.Click += buttonDelete_Click;
            panelEditZone.Controls.Add(tb);

            tb = new Button();
            tb.Text = "保存";
            tb.Location = new Point(230, row * 30);
            tb.Click += buttonEdit_Click;
            panelEditZone.Controls.Add(tb);

            tb = new Button();
            tb.Text = "导出";
            tb.Location = new Point(330, row * 30);
            tb.Click += buttonExport_Click;
            panelEditZone.Controls.Add(tb);

            tb = new Button();
            tb.Text = "导入";
            tb.Location = new Point(430, row * 30);
            tb.Click += buttonImport_Click;
            panelEditZone.Controls.Add(tb);

            tb = new Button();
            tb.Text = "追加导入";
            tb.Location = new Point(530, row * 30);
            tb.Click += buttonImportAppend_Click;
            panelEditZone.Controls.Add(tb);

            row += 2;

            panelEditZone.Height = (row + 2) * 30;

            searchBase = listViewIds.SelectedIndices[0];

        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            foreach (Control c in panelEditZone.Controls)
            {
                if (!(c is IAutoControl))
                    continue;

                string[] strs = c.Name.Split('-');
                string head = strs[0];

                int index = pack.GetPackIndexByName(head);
                String value = getControlValue(c as IAutoControl);
                String matchstr = "";
                String hintstr = "";
                if (pack.dataTypes[index] == "int")
                {
                    matchstr = @"^(-)?\d+$";
                    hintstr = "只能是整数";
                }
                else if (pack.dataTypes[index] == "char")
                {
                    matchstr = "^.{0," + pack.dataSizes[index] + "}$";
                    hintstr = String.Format("只能是字符串，且最大长度为{0}", pack.dataSizes[index]);
                }

                if (!Regex.IsMatch(value, matchstr))
                {
                    errorProvider1.SetError(c, hintstr);
                    return;
                }
            }

            foreach (Control c in panelEditZone.Controls)
            {
                if (!(c is IAutoControl))
                    continue;

                string[] strs = c.Name.Split('-');

                string head = strs[0];
                int id = int.Parse(strs[1]);
                int id2 = int.Parse(strs[2]);

                int index = pack.GetPackIndexByName(head);
                String value = getControlValue(c as IAutoControl);
                TriData bk = new TriData(selectId, id, id2);
                if (value != pack.data[bk][index])
                {
                    pack.EditPackData(bk, head, value);
                }

            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddTriDataForm adf = new AddTriDataForm();
            adf.Id = selectId;
            adf.Text = "添加数据";
            adf.Label1 = pack.comment[0] != "" ? pack.comment[0] : pack.headers[0];
            adf.Label2 = pack.comment[1] != "" ? pack.comment[1] : pack.headers[1];
            adf.Label3 = pack.comment[2] != "" ? pack.comment[2] : pack.headers[2];
            adf.ShowDialog();
            if (adf.Result == System.Windows.Forms.DialogResult.OK)
            {
                TriData bd = new TriData(adf.Id, adf.Id2, adf.Id3);
                if (!pack.data.ContainsKey(bd))
                {
                    List<String> data = new List<string>();
                    data.Add(adf.Id.ToString());
                    data.Add(adf.Id2.ToString());
                    data.Add(adf.Id3.ToString());
                    for (int i = 3; i < pack.dataTypes.Count; i++)
                    {
                        if (pack.dataTypes[i] == "char")
                        {
                            data.Add("");
                        }
                        else
                        {
                            data.Add("0");
                        }
                    }

                    pack.AddPackData(data);
                    refreshList(adf.Id);
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            AddTriDataForm adf = new AddTriDataForm();
            adf.Id = selectId;
            adf.Text = "删除数据";
            adf.Label1 = pack.comment[0] != "" ? pack.comment[0] : pack.headers[0];
            adf.Label2 = pack.comment[1] != "" ? pack.comment[1] : pack.headers[1];
            adf.Label3 = pack.comment[2] != "" ? pack.comment[2] : pack.headers[2];
            adf.ShowDialog();
            if (adf.Result == System.Windows.Forms.DialogResult.OK)
            {
                TriData bd = new TriData(adf.Id, adf.Id2, adf.Id3);
                if (pack.data.ContainsKey(bd))
                {
                    pack.RemovePackData(bd);
                    refreshList(0);
                }
            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "选择保存地址";
            sfd.InitialDirectory = "F://";
            sfd.FileName = packName;
            sfd.Filter = "Csv files (*.csv)|*.csv|All files (*.*)|*.*";
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pack.ExportPackData(sfd.FileName);
            }
            sfd.Dispose();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Title = "选择文件";
            sfd.InitialDirectory = "F://";
            sfd.FileName = packName;
            sfd.Filter = "Csv files (*.csv)|*.csv|All files (*.*)|*.*";
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pack.ImportPackData(sfd.FileName, true);
                DataService.GetAllTriData(packName);
                MessageBox.Show("导入完成!");
                Close();
            }
            sfd.Dispose();
        }

        private void buttonImportAppend_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Title = "选择文件";
            sfd.InitialDirectory = "F://";
            sfd.FileName = packName;
            sfd.Filter = "Csv files (*.csv)|*.csv|All files (*.*)|*.*";
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pack.ImportPackData(sfd.FileName, false);
                DataService.GetAllTriData(packName);
                MessageBox.Show("导入完成!");
                Close();
            }
            sfd.Dispose();
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

            int count = listViewIds.Items.Count;
            for (int i = 0; i < count; i++)
            {
                int newindex = (searchBase + i) % count;
                if (listViewIds.Items[newindex].Text.Contains(textBoxInfo.Text))
                {
                    listViewIds.Items[newindex].Selected = true;
                    listViewIds.Items[newindex].EnsureVisible();
                    searchBase = (newindex + 1) % count;
                    break;
                }
            }
        }
        #endregion
    }
}
