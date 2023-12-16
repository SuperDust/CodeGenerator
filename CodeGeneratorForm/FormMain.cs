﻿using System.Data;
using System.Diagnostics;
using CodeGeneratorForm.Entity;
using Microsoft.VisualBasic;
using OfficeOpenXml;
using SqlSugar;

namespace CodeGeneratorForm
{
    public partial class FormMain : Form
    {


        private List<DbTableInfo>? dbTableInfos = null;

        private string readFilePath = null;
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            dgv_tables.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DbContext.Instance.CodeFirst.InitTables(typeof(GeneratorSolution), typeof(DbConfig));
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
            this.cbx_template.Items.AddRange(Directory.GetFiles(TemplateDirPath, "*.cshtml", SearchOption.TopDirectoryOnly).Select(t => Path.GetFileName(t)).ToArray());
            this.cbx_template.SelectedIndex = 0;
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            List<ToolStripMenuItem> toolStripMenus = new List<ToolStripMenuItem>();
            toolStripMenus.Add(new ToolStripMenuItem("删除"));
            contextMenu.Items.AddRange(toolStripMenus.ToArray());
            contextMenu.ItemClicked += ContextMenu_ItemClicked; ;
            dgv_solution.ContextMenuStrip = contextMenu;
        }

        private void ContextMenu_ItemClicked(object? sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "删除")
            {
                List<GeneratorSolution> generatorSolutions = new List<GeneratorSolution>();
                foreach (DataGridViewRow row in dgv_solution.Rows)
                {
                    if (row.Cells[0].Value.ToString() == "1")
                    {
                        generatorSolutions.Add(row.Tag as GeneratorSolution);
                    }
                }
                if (generatorSolutions is { Count: > 0 })
                {
                    DbContext.Instance.Deleteable<GeneratorSolution>().WhereColumns(generatorSolutions, it => new { it.Name }).ExecuteCommand();
                    InitSolution();
                }
            }
        }

        private void InitSolution()
        {
            List<GeneratorSolution> generatorSolutions = DbContext.Instance.Queryable<GeneratorSolution>().ToList();
            dgv_solution.Rows.Clear();
            if (generatorSolutions is { Count: > 0 })
            {
                generatorSolutions.ForEach(info =>
                {
                    int rowIndex = dgv_solution.Rows.Add();
                    dgv_solution.Rows[rowIndex].Cells[0].Value = 0;
                    dgv_solution.Rows[rowIndex].Cells[1].Value = info.Name;
                    dgv_solution.Rows[rowIndex].Cells[2].Value = info.Template;
                    dgv_solution.Rows[rowIndex].Tag = info;
                });
            }
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
                if (list is { Count: > 0 }
                    && !string.IsNullOrEmpty(this.cbx_template.Text)
                    && !string.IsNullOrEmpty(this.txt_dir_path.Text))
                {
                    Generator.Init(list, this.cbx_template.Text, this.txt_filefirst.Text, this.txt_filelast.Text, this.txt_dir_path.Text, this.txt_namespace.Text);
                }
            }
        }

        /// <summary>
        /// 打开模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_open_template_Click(object sender, EventArgs e)
        {
            string TemplateDirPath = Path.Combine(Environment.CurrentDirectory, "Templates");
            readFilePath = Directory.GetFiles(TemplateDirPath, "*.cshtml",
               SearchOption.TopDirectoryOnly).Where(t => Path.GetFileName(t) == this.cbx_template.Text).FirstOrDefault();
            if (!string.IsNullOrEmpty(readFilePath) && File.Exists(readFilePath))
            {
                if (this.splitContainer2.Panel2Collapsed)
                {
                    textEditorControl1.SetHighlighting("C#");
                    textEditorControl1.Text = File.ReadAllText(readFilePath);
                    this.splitContainer2.Panel2Collapsed = false;
                    this.Width += 400;
                }

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
                DbContext.DbType = (SqlSugar.DbType)Enum.Parse(typeof(SqlSugar.DbType), this.cbx_dbtype.Text);
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
                DbContext.Instance.Insertable(new DbConfig
                {
                    ConnectionConfig = this.txt_connstr.Text,
                    ConnectionType = this.cbx_dbtype.Text
                }).ExecuteCommand();
                InitSolution();
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
                dgv_tables.Rows[e.RowIndex].Cells[0].Value = dgv_tables.Rows[e.RowIndex].Cells[0].Value.ToString() == "0" ? 1 : 0;
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
            var rows = dbTableInfos.Where(t => t.Name.ToLower().Contains(txt_search.Text.ToLower())).ToList();
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

        /// <summary>
        /// 方案数据选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_solution_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                dgv_solution.Rows[e.RowIndex].Cells[0].Value = dgv_solution.Rows[e.RowIndex].Cells[0].Value.ToString() == "0" ? 1 : 0;
            }
        }

        /// <summary>
        /// 生成方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_generator_solution_Click(object sender, EventArgs e)
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
                List<GeneratorSolution> generatorSolutions = new List<GeneratorSolution>();
                foreach (DataGridViewRow row in dgv_solution.Rows)
                {
                    if (row.Cells[0].Value.ToString() == "1")
                    {
                        generatorSolutions.Add(row.Tag as GeneratorSolution);
                    }
                }
                if (list is { Count: > 0 } && generatorSolutions is { Count: > 0 })
                {
                    foreach (var item in generatorSolutions)
                    {
                        Generator.Init(list, item.Template, item.FileFirst, item.Filelast, item.DirPath, item.NamespaceName);

                    }
                }
            }
        }

        private void btn_save_solution_Click(object sender, EventArgs e)
        {
            DbContext.Instance.Insertable(new GeneratorSolution
            {
                Name = this.txt_solution_name.Text,
                Template = this.cbx_template.Text,
                FileFirst = this.txt_filefirst.Text,
                Filelast = this.txt_filelast.Text,
                DirPath = this.txt_dir_path.Text,
                NamespaceName = this.txt_namespace.Text
            }).ExecuteCommand();
            InitSolution();
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
                string TemplateDirPath = Path.Combine(Environment.CurrentDirectory, "Excel", "Excel.xlsx");
                using (ExcelPackage package = new ExcelPackage(new FileInfo(TemplateDirPath)))
                {
                    var sheet = package.Workbook.Worksheets[0];
                    var sheetStyle = package.Workbook.Worksheets[1];
                    int rowIndex = 1;
                    INIFile ini = new INIFile(Path.Combine(Environment.CurrentDirectory, "config.ini"));

                    for (int i = 0; i < list.Count; i++)
                    {

                        sheetStyle.Cells[1, 1, 2, 8].Copy(sheet.Cells[rowIndex, 1]);
                        sheet.Cells[rowIndex, 1].Value = list[i].Name + " " + list[i].Description;
                        rowIndex++;
                        List<DbColumnInfo> columnInfos =
                               DbContext.db!.DbMaintenance.GetColumnInfosByTableName(list[i].Name, false);
                        for (int j = 0; j < columnInfos.Count; j++)
                        {
                            string type = ini.IniReadValue("ExcelTypeConvort", columnInfos[j].DataType);
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
                            sheet.Cells[rowIndex, 4].Value = "(" + (columnInfos[j].DecimalDigits > 0 ? columnInfos[j].Length + "," + columnInfos[j].DecimalDigits : columnInfos[j].Length) + ")";
                            sheet.Cells[rowIndex, 5].Value = columnInfos[j].IsPrimarykey ? "√" : "";
                            sheet.Cells[rowIndex, 6].Value = columnInfos[j].IsNullable ? "√" : "";
                            sheet.Cells[rowIndex, 7].Value = columnInfos[j].DefaultValue;
                            sheet.Cells[rowIndex, 8].Value = columnInfos[j].ColumnDescription;
                        }
                        rowIndex++;
                        sheetStyle.Cells[4, 1, 4, 8].Copy(sheet.Cells[rowIndex, 1]);
                        rowIndex++;
                    }

                    package.SaveAs(new FileInfo(Path.Combine(dirPath, "DataDoc" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx")));
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", Path.Combine(Environment.CurrentDirectory, "config.ini"));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.splitContainer2.Panel2Collapsed = true;
            this.Width = 710;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            File.WriteAllText(readFilePath, textEditorControl1.Text);
            this.splitContainer2.Panel2Collapsed = true;
            this.Width = 710;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string name = Interaction.InputBox("请输入模板文件名称", "添加模板", "", -1, -1);
            if (name.Length == 0)
            {
                return;
            }

            string filePath = Path.Combine(Environment.CurrentDirectory, "Templates", name + ".cshtml");
            try
            {
                File.WriteAllText(filePath, "@using  System\r\n@using  System.Collections.Generic\r\n@using  System.IO\r\n@using  System.Linq\r\n@using  SqlSugar\r\n@using  CodeGeneratorForm\r\n@{\r\n\t/*\r\n    *表名 Table->TableName\r\n    *类名 Table->ClassName\r\n    *备注 Table->TableComment\r\n    */\r\n    Table table =Model;\r\n   \r\n   /*@foreach (var column in columnInfos)\r\n    *{\r\n    *@:public void set@(column.ColumnName)(){}\r\n    *}\r\n    *表名 ColumnInfo->TableName\r\n    *列名 ColumnInfo->ColumnName\r\n    *列备注 ColumnInfo->ColumnComment\r\n    *默认值 ColumnInfo->ColumnDefault\r\n    *数据类型 ColumnInfo->DataType\r\n    *是否自增 ColumnInfo->IsIdentity\r\n    *是否为null ColumnInfo->IsNullable\r\n    *是否主键 ColumnInfo->IsPrimaryKey\r\n    *属性名 ColumnInfo->PropertyName\r\n    *属性类型 ColumnInfo->PropertyType\r\n    */\r\n    List<ColumnInfo> columnInfos = table.ColumnInfos;\r\n}");
                string TemplateDirPath = Path.Combine(Environment.CurrentDirectory, "Templates");
                this.cbx_template.Items.Clear();
                this.cbx_template.Items.Add("");
                this.cbx_template.Items.AddRange(Directory.GetFiles(TemplateDirPath, "*.cshtml", SearchOption.TopDirectoryOnly).Select(t => Path.GetFileName(t)).ToArray());
                this.cbx_template.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        private void cbx_template_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("删除");
                contextMenu.Click += ContextMenu_Click;
                this.cbx_template.ContextMenuStrip = contextMenu;
            }
        }

        private void ContextMenu_Click(object? sender, EventArgs e)
        {
            string TemplateDirPath = Path.Combine(Environment.CurrentDirectory, "Templates");
            File.Delete(Directory.GetFiles(TemplateDirPath, "*.cshtml",
               SearchOption.TopDirectoryOnly).Where(t => Path.GetFileName(t) == this.cbx_template.Text).First());
            this.cbx_template.Items.Clear();
            this.cbx_template.Items.Add("");
            this.cbx_template.Items.AddRange(Directory.GetFiles(TemplateDirPath, "*.cshtml", SearchOption.TopDirectoryOnly).Select(t => Path.GetFileName(t)).ToArray());
            this.cbx_template.SelectedIndex = 0;
        }
    }
}
