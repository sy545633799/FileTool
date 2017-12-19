using System;
using System.Windows.Forms;
using StarEdit.MysqlServices;

namespace StarEdit.Forms
{
    public partial class DataViewForm : Form
    {
        string packName = "cfg_test";
        OldDataPack pack;
        String pastedValue;
        bool isLoad = false;

        public DataViewForm()
        {
            InitializeComponent();
        }

        public string PackName
        {
            get { return packName; }
            set { packName = value; }
        }

        private void DataViewForm_Load(object sender, EventArgs e)
        {
            pack = OldDataPackBook.GetPack(packName);

            foreach (String head in pack.header)
            {
                dataGridView1.Columns.Add(head, head);
                int index = pack.GetPackIndexByName(head);
                if(pack.datatype[index] == "int")
                {
                    dataGridView1.Columns[head].Width = 40;
                }
                dataGridView1.Columns[head].HeaderText = pack.comment[index];
            }
            
            foreach (int key in pack.data.Keys)
            {
                dataGridView1.Rows.Add(pack.data[key].ToArray());
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].HeaderCell.Value = key.ToString();
            }

            this.Text = String.Format("{0} (总计{1}条) (缓存时间{2})", packName, pack.data.Values.Count, pack.cachTime.ToString());

            isLoad = true;
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                if (dataGridView1.SelectedCells.Count == 1)
                {
                    pastedValue = dataGridView1.SelectedCells[0].Value.ToString();
                }
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                if(pastedValue == "")
                    return;

                foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
                {
                    cell.Value = pastedValue;
                }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!isLoad)
            {
                return;
            }

            string head = pack.header[e.ColumnIndex];
            int index = int.Parse(dataGridView1.Rows[e.RowIndex].HeaderCell.Value.ToString());
            string value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            int off = pack.GetPackIndexByName(head);
            if (value != pack.data[index][off])
            {
                pack.EditPackData(index, head, value);
            }
        }
 
    }
}
