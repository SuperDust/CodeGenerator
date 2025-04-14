using System.Data;
using System.Diagnostics;
using CodeGeneratorForm.Entity;
using Microsoft.VisualBasic;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SqlSugar;
using SqlSugar.Extensions;

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
            dgv_tables.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DbContext.Instance.CodeFirst.InitTables(typeof(DbConfig));
            DbConfig dbConfig = DbContext.Instance.Queryable<DbConfig>().First();
            if (dbConfig == null)
            {
                this.cbx_dbtype.SelectedIndex = 0;
            }
            else
            {
                this.txt_connstr.Text = dbConfig.ConnectionConfig;
                this.cbx_dbtype.SelectedText = dbConfig.ConnectionType;
            }
            string TemplateDirPath = Path.Combine(Environment.CurrentDirectory, "Templates");
            this.cbx_template.Items.Clear();
            this.cbx_template.Items.Add("");
            this.cbx_template.Items.AddRange(
                Directory
                    .GetFiles(TemplateDirPath, "*.cshtml", SearchOption.TopDirectoryOnly)
                    .Select(t => Path.GetFileName(t))
                    .ToArray()
            );
            this.cbx_template.SelectedIndex = 0;
        }

        /// <summary>
        /// 生成模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_generator_Click(object sender, EventArgs e)
        {
            if (DbContext.db != null)
            {
                List<DbTableInfo> list = new List<DbTableInfo>();
                foreach (DataGridViewRow row in dgv_tables.Rows)
                {
                    if (row.Cells[0].Value.ToString() == "1")
                    {
                        list.Add(row.Tag as DbTableInfo);
                    }
                }
                if (
                    list is { Count: > 0 }
                    && !string.IsNullOrEmpty(this.cbx_template.Text)
                    && !string.IsNullOrEmpty(this.txt_dir_path.Text)
                )
                {
                    Generator.Init(
                        list,
                        this.cbx_template.Text,
                        this.txt_filefirst.Text,
                        this.txt_filelast.Text,
                        this.txt_dir_path.Text,
                        this.txt_extend_name.Text,
                        this.checkBox2.Checked,
                        this.checkBox1.Checked
                    );
                }
                else
                {
                    if (list is { Count: > 0 })
                    {
                        MessageBox.Show("请选择你要生成的模板或路径！");
                    }
                    else
                    {
                        MessageBox.Show("请选择你要生成的数据！");
                    }
                    return;
                }
            }
            else
            {
                MessageBox.Show("请选择你要生成的数据！");
                return;
            }
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_conn_db_Click(object sender, EventArgs e)
        {
            try
            {
                DbContext.db = DbContext.Connection(this.txt_connstr.Text, this.cbx_dbtype.Text);
                DbContext.DbType = (SqlSugar.DbType)
                    Enum.Parse(typeof(SqlSugar.DbType), this.cbx_dbtype.Text);
                dbTableInfos = DbContext.db.DbMaintenance.GetTableInfoList();
                dgv_tables.Rows.Clear();
                dbTableInfos.ForEach(info =>
                {
                    int rowIndex = dgv_tables.Rows.Add();
                    dgv_tables.Rows[rowIndex].Cells[0].Value = 0;
                    dgv_tables.Rows[rowIndex].Cells[1].Value = info.Name;
                    dgv_tables.Rows[rowIndex].Cells[2].Value = info.Description;
                    dgv_tables.Rows[rowIndex].Tag = info;
                });
                DbContext.Instance.Deleteable<DbConfig>().ExecuteCommand();
                DbContext
                    .Instance.Insertable(
                        new DbConfig
                        {
                            ConnectionConfig = this.txt_connstr.Text,
                            ConnectionType = this.cbx_dbtype.Text,
                        }
                    )
                    .ExecuteCommand();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"连接失败:{ex.Message}!");
            }
        }

        /// <summary>
        /// 浏览文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_open_browse_dir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.txt_dir_path.Text = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_open_dir_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", this.txt_dir_path.Text);
        }

        /// <summary>
        /// 表数据选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_tables_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                dgv_tables.Rows[e.RowIndex].Cells[0].Value =
                    dgv_tables.Rows[e.RowIndex].Cells[0].Value.ToString() == "0" ? 1 : 0;
            }
        }

        /// <summary>
        /// 搜索表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            if (dbTableInfos == null)
            {
                return;
            }
            dgv_tables.Rows.Clear();
            var rows = dbTableInfos
                .Where(t => t.Name.ToLower().Contains(txt_search.Text.ToLower()))
                .ToList();
            if (rows is { Count: > 0 })
            {
                rows.ForEach(info =>
                {
                    int rowIndex = dgv_tables.Rows.Add();
                    dgv_tables.Rows[rowIndex].Cells[0].Value = 0;
                    dgv_tables.Rows[rowIndex].Cells[1].Value = info.Name;
                    dgv_tables.Rows[rowIndex].Cells[2].Value = info.Description;
                    dgv_tables.Rows[rowIndex].Tag = info;
                });
            }
        }

        /// <summary>
        /// 选中所有表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbox_all_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbox_all.Checked == true)
            {
                for (int i = 0; i < this.dgv_tables.Rows.Count; i++)
                {
                    this.dgv_tables.Rows[i].Cells[0].Value = 1;
                }
            }
            else
            {
                for (int i = 0; i < this.dgv_tables.Rows.Count; i++)
                {
                    this.dgv_tables.Rows[i].Cells[0].Value = 0;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DbContext.db != null)
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "请选择文件路径";
                string dirPath = "";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    dirPath = dialog.SelectedPath;
                }
                else
                {
                    return;
                }
                List<DbTableInfo> list = new List<DbTableInfo>();
                foreach (DataGridViewRow row in dgv_tables.Rows)
                {
                    if (row.Cells[0].Value.ToString() == "1")
                    {
                        list.Add(row.Tag as DbTableInfo);
                    }
                }
                string TemplateDirPath = Path.Combine(
                    Environment.CurrentDirectory,
                    "Excel",
                    "Excel.xlsx"
                );
                using (ExcelPackage package = new ExcelPackage(new FileInfo(TemplateDirPath)))
                {
                    var sheet = package.Workbook.Worksheets[0];
                    var sheetStyle = package.Workbook.Worksheets[1];
                    int rowIndex = 1;
                    INIFile ini = new INIFile(
                        Path.Combine(Environment.CurrentDirectory, "config.ini")
                    );

                    for (int i = 0; i < list.Count; i++)
                    {
                        sheetStyle.Cells[1, 1, 2, 8].Copy(sheet.Cells[rowIndex, 1]);
                        sheet.Cells[rowIndex, 1].Value = list[i].Name + " " + list[i].Description;
                        rowIndex++;
                        List<DbColumnInfo> columnInfos =
                            DbContext.db!.DbMaintenance.GetColumnInfosByTableName(
                                list[i].Name,
                                false
                            );
                        for (int j = 0; j < columnInfos.Count; j++)
                        {
                            string type = ini.IniReadValue(
                                "ExcelTypeConvort",
                                columnInfos[j].DataType
                            );
                            if (string.IsNullOrWhiteSpace(type))
                            {
                                type = columnInfos[j].DataType;
                            }
                            rowIndex++;
                            sheet.InsertRow(rowIndex, 1);
                            sheetStyle.Cells[3, 1, 3, 8].Copy(sheet.Cells[rowIndex, 1]);
                            sheet.Cells[rowIndex, 1].Value = j + 1;
                            sheet.Cells[rowIndex, 2].Value = columnInfos[j].DbColumnName;
                            sheet.Cells[rowIndex, 3].Value = type;
                            sheet.Cells[rowIndex, 4].Value =
                                "("
                                + (
                                    columnInfos[j].DecimalDigits > 0
                                        ? columnInfos[j].Length + "," + columnInfos[j].DecimalDigits
                                        : columnInfos[j].Length
                                )
                                + ")";
                            sheet.Cells[rowIndex, 5].Value = columnInfos[j].IsPrimarykey ? "√" : "";
                            sheet.Cells[rowIndex, 6].Value = columnInfos[j].IsNullable ? "√" : "";
                            sheet.Cells[rowIndex, 7].Value = columnInfos[j].DefaultValue;
                            sheet.Cells[rowIndex, 8].Value = columnInfos[j].ColumnDescription;
                        }
                        rowIndex++;
                        sheetStyle.Cells[4, 1, 4, 8].Copy(sheet.Cells[rowIndex, 1]);
                        rowIndex++;
                    }

                    package.SaveAs(
                        new FileInfo(
                            Path.Combine(
                                dirPath,
                                "DataDoc" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx"
                            )
                        )
                    );
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", Path.Combine(Environment.CurrentDirectory, "config.ini"));
        }

        private void cbx_template_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("编辑");
                contextMenu.Click += ContextMenu_Click;
                this.cbx_template.ContextMenuStrip = contextMenu;
            }
        }

        private void ContextMenu_Click(object? sender, EventArgs e)
        {
            Process.Start(
                "notepad.exe",
                Path.Combine(Environment.CurrentDirectory, "Templates", this.cbx_template.Text)
            );
        }

        private void btn_set_field_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", Path.Combine(Environment.CurrentDirectory, "field.ini"));
        }
    }
}
