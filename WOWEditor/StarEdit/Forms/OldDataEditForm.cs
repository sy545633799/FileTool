using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MessageBoxExLib;
using StarEdit.Interface;
using StarEdit.MyComponents;
using StarEdit.MysqlServices;
using StarEdit.Tools;

namespace StarEdit.Forms
{
    public partial class OldDataEditForm : Form
    {
        OldDataPack pack;
        int selectId;
        int searchBase;
        string packName;
        ListViewItem lastSelected;
        int nameIndex = 1;
        bool isInit = false;

        public string PackName
        {
            get { return packName; }
            set { packName = value; }
        }

        public OldDataEditForm()
        {
            InitializeComponent();
        }

        private void DataEditForm_Load(object sender, EventArgs e)
        {
            pack = OldDataPackBook.GetPack(packName);
            IniFile configData = new IniFile(String.Format("config/db/{0}.ini", packName));

            string nameindex = configData.IniReadValue("main", "nameindex");
            if (nameindex != "")
            {
                nameIndex = int.Parse(nameindex);
            }

            Label lbc = new Label();
            lbc.Location = new Point(0, 0);
            lbc.Text = "";
            lbc.Name = "lbltop";
            panelEditZone.Controls.Add(lbc);

            int index = 0;
            int row = 0;
            int col = 0;
            int wid = panelEditZone.Width / 2;
            Point offside = new Point(8, 20);
            Font fontsong = new Font("宋体", 10, FontStyle.Regular);
            foreach (String head in pack.header)
            {
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
                lbl2.Text = pack.comment[index];
                lbl2.Tag = pack.comment[index];
                lbl2.MouseHover += new EventHandler(lbl2_MouseHover);
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

                string urlpath = configData.IniReadValue(head, "path");
                string urlsize = configData.IniReadValue(head, "size");
                string enums = configData.IniReadValue(head, "enums");
                string ext = configData.IniReadValue(head, "ext");
                string bind = configData.IniReadValue(head, "bind");
                string checktxt = configData.IniReadValue(head, "checktxt");
                string dialogcheck = configData.IniReadValue(head, "dialogcheck");
                string attrcheck = configData.IniReadValue(head, "attrcheck");
                string taskcond = configData.IniReadValue(head, "taskcond");
                string icon = configData.IniReadValue(head, "icon");
                string icondes = configData.IniReadValue(head, "icondes");
                string unixtime = configData.IniReadValue(head, "unixtime");
                string itemlist = configData.IniReadValue(head, "itemlist");
                string rectgraph = configData.IniReadValue(head, "rectgraph");
                string movie = configData.IniReadValue(head, "movie");
                string timehour = configData.IniReadValue(head, "timehour");
                string wisebox = configData.IniReadValue(head, "wisebox");
                string wiseguidebox = configData.IniReadValue(head, "wiseguidebox");
                string colorwords = configData.IniReadValue(head, "colorwords");
                string effectpath = configData.IniReadValue(head, "effpath");

                Control c;
                if (urlpath != "") //图片combox
                {
                    c = new UCPictureComboBox();
                    (c as UCPictureComboBox).PictureSize = urlsize;
                    (c as UCPictureComboBox).SetPath(urlpath, ext);
                }
                else if (effectpath != "")
                {
                    c = new UCEffectBox();
                    (c as UCEffectBox).SetPath(effectpath, ext);
                }
                else if (enums != "") //combox
                {
                    c = new UCComboBox();
                    (c as UCComboBox).Info = enums;
                }
                else if (bind != "") //boxlabel
                {
                    c = new UCBoxLabel();
                    (c as UCBoxLabel).TableName = bind;
                }
                else if (checktxt != "")
                {
                    c = new UCCheckBoxGroup();
                    (c as UCCheckBoxGroup).Info = checktxt;
                }
                else if (dialogcheck != "")
                {
                    c = new UCDialogBox();
                }
                else if (attrcheck != "")
                {
                    c = new UCAttrBox();
                }
                else if (rectgraph != "")
                {
                    c = new UCRectGraph();
                    string[] info = rectgraph.Split('|');
                    (panelEditZone.Controls[info[0]] as UCTextBox).TextChanged += rectTextChanged;
                    (panelEditZone.Controls[info[1]] as UCTextBox).TextChanged += rectTextChanged;
                    (panelEditZone.Controls[info[2]] as UCTextBox).TextChanged += rectTextChanged;
                    (panelEditZone.Controls[info[3]] as UCTextBox).TextChanged += rectTextChanged;
                    (panelEditZone.Controls[info[4]] as UCBoxLabel).TextChanged += rectTextChanged;
                    (panelEditZone.Controls[info[5]] as UCBoxLabel).TextChanged += rectTextChanged;

                    (panelEditZone.Controls[info[0]] as UCTextBox).Tag = "x1";
                    (panelEditZone.Controls[info[1]] as UCTextBox).Tag = "y1";
                    (panelEditZone.Controls[info[2]] as UCTextBox).Tag = "x2";
                    (panelEditZone.Controls[info[3]] as UCTextBox).Tag = "y2";
                    (panelEditZone.Controls[info[4]] as UCBoxLabel).Tag = "m1";
                    (panelEditZone.Controls[info[5]] as UCBoxLabel).Tag = "m2";
                }
                else if (taskcond != "")
                {
                    c = new UCTaskRequest();
                    panelEditZone.Controls[taskcond].Tag = head;
                    (panelEditZone.Controls[taskcond] as UCComboBox).SelectedIndexChanged += comboBoxChanged;
                }
                else if (movie != "")
                {
                    c = new UCMovieData();
                    panelEditZone.Controls[movie].Tag = head;
                    (panelEditZone.Controls[movie] as UCComboBox).SelectedIndexChanged += comboBoxChanged;
                }
                else if (icon != "")
                {
                    c = new UCBoxIcon();
                    (c as UCBoxIcon).Icon = icon;
                    (c as UCBoxIcon).Des = icondes;
                }
                else if (unixtime != "")
                {
                    c = new UCUnixTimeLabel();
                }
                else if (itemlist != "")
                {
                    c = new UCItemList();
                }
                else if (timehour != "")
                {
                    c = new UCTimeHour();
                }
                else if (wisebox != "")
                {
                    c = new UCWiseBoxLabel();
                }
                else if (wiseguidebox != "")
                {
                    c = new UCWiseGuideBoxLabel();
                }
                else if (colorwords != "")
                {
                    c = new UCColorWordsBox();
                }
                else //textbox
                {
                    c = new UCTextBox();
                    if (pack.datatype[index] == "char")
                    {
                        (c as UCTextBox).TextSize = pack.datasize[index];
                    }
                }
                c.Enabled = (index > 0);
                c.Name = head;
                c.Location = new Point(col * wid + 170 + offside.X, row * 30 + offside.Y);
                if (c.Height > 30)
                {
                    row += c.Height / 30;
                    col = 0;
                }
                panelEditZone.Controls.Add(c);
                #endregion

                index++;
                if (col == 0 && row == 0)
                {
                    row++;
                }
                else if (index < pack.datatype.Count && (pack.comment[index].Contains("[NL]") || pack.comment[index-1].Contains("[NL]"))) //特殊标签
                {
                    row++;
                    col = 0;
                }
                else if (c.Height <= 30 && index < pack.datatype.Count && col == 0 && (pack.datatype[index] != "char" || pack.datasize[index] <= 8) && (pack.datatype[index - 1] != "char" || pack.datasize[index - 1] <= 8))
                {
                    col++;
                }
                else
                {
                    row++;
                    col = 0;
                }
            }
            fontsong.Dispose();
            panelEditZone.Height = (row + 2) * 30;

            refreshList(0);

            isInit = true;
        }

        private void lbl2_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(sender as Control, (sender as Control).Tag.ToString());
        }

        private void comboBoxChanged(object sender, EventArgs e)
        {
            UCComboBox ucb = sender as UCComboBox;
            if (panelEditZone.Controls[ucb.Tag.ToString()] is UCTaskRequest)
                (panelEditZone.Controls[ucb.Tag.ToString()] as UCTaskRequest).SelectIndex = int.Parse(ucb.Value);
            else if (panelEditZone.Controls[ucb.Tag.ToString()] is UCMovieData)
                (panelEditZone.Controls[ucb.Tag.ToString()] as UCMovieData).SelectIndex = int.Parse(ucb.Value);

            panelEditZone.Controls["lbltop"].Focus();
            listViewIds.Focus();
        }

        private void rectTextChanged(object sender, EventArgs e)
        {
            (panelEditZone.Controls["graphy"] as UCRectGraph).SetData((sender as Control).Tag.ToString(), int.Parse(getControlValue(sender as IAutoControl)));
        }

        private void refreshList(int selid)
        {
            int index = 0;
            listViewIds.Items.Clear();
            foreach (List<String> item in pack.data.Values)
            {
                ListViewItem lvm = new ListViewItem(String.Format("{0}.{1}", item[0], item[nameIndex]));
                int id = int.Parse(item[0]);
                listViewIds.Items.Add(lvm);
                if ((index == selid && index == 0) || selid == id)
                {
                    lvm.Selected = true;
                    lvm.EnsureVisible();
                }
                index++;
            }
            this.Text = String.Format("{0} (总计{1}条) (缓存时间{2})", packName, pack.data.Values.Count, pack.cachTime.ToString());
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

            if (isInit)
            {
                if (pack.data.ContainsKey(selectId)) //数据修改后直接切换页面时的提醒
                {
                    bool isdirty = false;
                    foreach (String head in pack.header)
                    {
                        String keyname = pack.header[0];
                        int index = pack.GetPackIndexByName(head);
                        String value = getControlValue(panelEditZone.Controls[head] as IAutoControl);
                        if (value != pack.data[selectId][index])
                        {
                            panelEditZone.Controls["lbl" + head].ForeColor = Color.Red; //dirty标记
                            isdirty = true;
                        }
                    }

                    if (isdirty)
                    {
                        if (MessageBoxEx.Show("数据修改后没有保存，确定不需要保存数据吗?", "警告", SystemIcons.Warning) != System.Windows.Forms.DialogResult.OK)
                        {
                            return;
                        }
                    }
                }

                foreach (String head in pack.header)
                {
                    panelEditZone.Controls["lbl" + head].ForeColor = Color.Black;//dirty标记还原
                }
            }

            searchBase = listViewIds.SelectedIndices[0];

            String[] data = listViewIds.SelectedItems[0].Text.Split('.');
            if (lastSelected != null)
            {
                lastSelected.BackColor = System.Drawing.SystemColors.Window;
            }
            listViewIds.SelectedItems[0].BackColor = Color.Gold;
            selectId = int.Parse(data[0]);
            lastSelected = listViewIds.SelectedItems[0];

            foreach (String head in pack.header)
            {
                int index = pack.GetPackIndexByName(head);
                setControlValue(panelEditZone.Controls[head] as IAutoControl, pack.data[selectId][index]);
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            foreach (String head in pack.header)
            {
                int index = pack.GetPackIndexByName(head);
                String value = getControlValue(panelEditZone.Controls[head] as IAutoControl);
                String matchstr = "";
                String hintstr = "";
                if (pack.datatype[index] == "int")
                {
                    matchstr = @"^(-)?\d+$";
                    hintstr = "只能是整数";
                }
                else if (pack.datatype[index] == "char")
                {
                    matchstr = "^.{0," + pack.datasize[index] + "}$";
                    hintstr = String.Format("只能是字符串，且最大长度为{0}", pack.datasize[index]);
                }

                if (!Regex.IsMatch(value, matchstr))
                {
                    errorProvider1.SetError(panelEditZone.Controls[head], hintstr);
                    return;
                }

                if (value.IndexOf('\n') >= 0)
                {
                    errorProvider1.SetError(panelEditZone.Controls[head], "数据中有回车");
                    return;
                }
            }

            bool needupdatename = false;
            foreach (String head in pack.header)
            {
                int index = pack.GetPackIndexByName(head);
                String value = getControlValue(panelEditZone.Controls[head] as IAutoControl);
                if (value != pack.data[selectId][index])
                {
                    pack.EditPackData(selectId, head, value);

                    if (index == nameIndex) //名称键更新需要刷新列表
                    {
                        needupdatename = true;
                    }
                }
            }

            if (needupdatename)
            {
                refreshList(selectId);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddDataForm adf = new AddDataForm();
            adf.Label1 = pack.comment[0] != "" ? pack.comment[0] : pack.header[0];
            adf.ShowDialog();
            if (adf.Result == System.Windows.Forms.DialogResult.OK)
            {
                if (!pack.data.ContainsKey(adf.Id))
                {
                    List<String> data = new List<string>();
                    data.Add(adf.Id.ToString());

                    bool doCopy = false;
                    if (pack.data.ContainsKey(selectId) && adf.NeedCopy)
                        doCopy = true;

                    for (int i = 1; i < pack.datatype.Count; i++)
                    {
                        if (doCopy)
                        {
                            data.Add(pack.data[selectId][i]);
                        }
                        else
                        {
                            if (pack.datatype[i] == "char")
                                data.Add("");
                            else
                                data.Add("0");
                        }
                    }

                    pack.AddPackData(adf.Id, data);
                    refreshList(adf.Id);
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.Show("确定要删除这条数据吗?", "警告", SystemIcons.Warning) == System.Windows.Forms.DialogResult.OK)
            {
                pack.RemovePackData(selectId);
                refreshList(0);
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
                MysqlService.GetAllData(packName);
                MessageBox.Show("导入完成!");
                Close();
            }
            sfd.Dispose();
        }

        private void buttonAppend_Click(object sender, EventArgs e)
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
                MysqlService.GetAllData(packName);
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
