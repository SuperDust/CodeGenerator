using CodeGeneratorForm.Entity;
using SqlSugar;
using System.Data;
using System.Diagnostics;

namespace CodeGeneratorForm
{
    public partial class FormMain : Form
    {


        private List<DbTableInfo>? dbTableInfos = null;
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DbConfig dbConfig = DbContext.Instance.Queryable<DbConfig>().First();
            if (dbConfig == null)
            {
                this.comboBox1.SelectedIndex = 0;
            }
            else
            {
                this.textBox1.Text = dbConfig.ConnectionConfig;
                this.comboBox1.SelectedText = dbConfig.ConnectionType;
            }
            string TemplateDirPath = Path.Combine(Environment.CurrentDirectory, "Templates");
            this.comboBox2.Items.AddRange(Directory.GetFiles(TemplateDirPath, "*.cshtml", SearchOption.TopDirectoryOnly).Select(t => Path.GetFileName(t)).ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DbContext.db = DbContext.Connection(this.textBox1.Text, this.comboBox1.Text);
                DbContext.DbType = (SqlSugar.DbType)Enum.Parse(typeof(SqlSugar.DbType), this.comboBox1.Text);
                dbTableInfos = DbContext.db.DbMaintenance.GetTableInfoList();
                dataGridView1.Rows.Clear();
                dbTableInfos.ForEach(info =>
                {
                    int rowIndex = dataGridView1.Rows.Add();
                    dataGridView1.Rows[rowIndex].Cells[0].Value = 0;
                    dataGridView1.Rows[rowIndex].Cells[1].Value = info.Name;
                    dataGridView1.Rows[rowIndex].Cells[2].Value = info.Description;
                    dataGridView1.Rows[rowIndex].Tag = info;
                });
                DbContext.Instance.Deleteable<DbConfig>().ExecuteCommand();
                DbContext.Instance.Insertable(new DbConfig
                {
                    ConnectionConfig = this.textBox1.Text,
                    ConnectionType = this.comboBox1.Text
                }).ExecuteCommand();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"连接失败:{ex.Message}!");
            }
        }

        private void cbox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbox1.Checked == true)
            {
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    this.dataGridView1.Rows[i].Cells[0].Value = 1;
                }
            }
            else
            {
                for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
                {
                    this.dataGridView1.Rows[i].Cells[0].Value = 0;
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (dbTableInfos == null)
            {
                return;
            }
            dataGridView1.Rows.Clear();
            var rows = dbTableInfos.Where(t => t.Name.Contains(textBox2.Text))
                  .ToList();
            if (rows is { Count: > 0 })
            {
                rows.ForEach(info =>
                {
                    int rowIndex = dataGridView1.Rows.Add();
                    dataGridView1.Rows[rowIndex].Cells[0].Value = 0;
                    dataGridView1.Rows[rowIndex].Cells[1].Value = info.Name;
                    dataGridView1.Rows[rowIndex].Cells[2].Value = info.Description;
                    dataGridView1.Rows[rowIndex].Tag = info;
                });
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (DbContext.db != null)
            {
                List<DbTableInfo> list = new List<DbTableInfo>();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value.ToString() == "1")
                    {
                        list.Add(row.Tag as DbTableInfo);
                    }
                }
                if (list is { Count: > 0 }
                    && !string.IsNullOrEmpty(this.comboBox2.Text)
                    && !string.IsNullOrEmpty(this.textBox5.Text))
                {
                    Generator.Init(list, this.comboBox2.Text, this.textBox3.Text, this.textBox4.Text, this.textBox5.Text);
                    Process.Start("explorer.exe", this.textBox5.Text);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                dataGridView1.Rows[e.RowIndex].Cells[0].Value = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() == "0" ? 1 : 0;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", this.textBox5.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string TemplateDirPath = Path.Combine(Environment.CurrentDirectory, "Templates");
            Process.Start("notepad.exe", Directory.GetFiles(TemplateDirPath, "*.cshtml",
                SearchOption.TopDirectoryOnly).Where(t => Path.GetFileName(t) == this.comboBox2.Text).First());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox5.Text = dialog.SelectedPath;
            }
        }
    }
}
