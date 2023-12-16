namespace CodeGeneratorForm
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            txt_connstr = new TextBox();
            label1 = new Label();
            btn_conn_db = new Button();
            cbx_dbtype = new ComboBox();
            dgv_tables = new DataGridView();
            optionck = new DataGridViewCheckBoxColumn();
            tableName = new DataGridViewTextBoxColumn();
            tableDesc = new DataGridViewTextBoxColumn();
            cbox_all = new CheckBox();
            txt_search = new TextBox();
            btn_generator = new Button();
            cbx_template = new ComboBox();
            label2 = new Label();
            btn_open_template = new Button();
            label3 = new Label();
            txt_filefirst = new TextBox();
            txt_filelast = new TextBox();
            label4 = new Label();
            txt_dir_path = new TextBox();
            btn_open_browse_dir = new Button();
            btn_open_dir = new Button();
            txt_namespace = new TextBox();
            label5 = new Label();
            btn_save_solution = new Button();
            dgv_solution = new DataGridView();
            dataGridViewCheckBoxColumn1 = new DataGridViewCheckBoxColumn();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            btn_generator_solution = new Button();
            txt_solution_name = new TextBox();
            label6 = new Label();
            button1 = new Button();
            button2 = new Button();
            splitContainer1 = new SplitContainer();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            panel2 = new Panel();
            splitContainer2 = new SplitContainer();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel3 = new Panel();
            button5 = new Button();
            tableLayoutPanel3 = new TableLayoutPanel();
            textEditorControl1 = new ICSharpCode.TextEditor.TextEditorControl();
            panel4 = new Panel();
            button4 = new Button();
            button3 = new Button();
            ((System.ComponentModel.ISupportInitialize)dgv_tables).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgv_solution).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel3.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // txt_connstr
            // 
            txt_connstr.Location = new Point(43, 6);
            txt_connstr.Name = "txt_connstr";
            txt_connstr.Size = new Size(253, 23);
            txt_connstr.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(7, 9);
            label1.Name = "label1";
            label1.Size = new Size(32, 17);
            label1.TabIndex = 1;
            label1.Text = "连接";
            // 
            // btn_conn_db
            // 
            btn_conn_db.Location = new Point(128, 35);
            btn_conn_db.Name = "btn_conn_db";
            btn_conn_db.Size = new Size(46, 24);
            btn_conn_db.TabIndex = 2;
            btn_conn_db.Text = "连接";
            btn_conn_db.UseVisualStyleBackColor = true;
            btn_conn_db.Click += btn_conn_db_Click;
            // 
            // cbx_dbtype
            // 
            cbx_dbtype.FormattingEnabled = true;
            cbx_dbtype.Items.AddRange(new object[] { "MySql", "PostgreSQL", "SqlServer", "Oracle" });
            cbx_dbtype.Location = new Point(43, 34);
            cbx_dbtype.Name = "cbx_dbtype";
            cbx_dbtype.Size = new Size(79, 25);
            cbx_dbtype.TabIndex = 3;
            // 
            // dgv_tables
            // 
            dgv_tables.AllowUserToAddRows = false;
            dgv_tables.AllowUserToDeleteRows = false;
            dgv_tables.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_tables.Columns.AddRange(new DataGridViewColumn[] { optionck, tableName, tableDesc });
            dgv_tables.Dock = DockStyle.Fill;
            dgv_tables.Location = new Point(0, 0);
            dgv_tables.Name = "dgv_tables";
            dgv_tables.ReadOnly = true;
            dgv_tables.RowTemplate.Height = 25;
            dgv_tables.Size = new Size(302, 574);
            dgv_tables.TabIndex = 4;
            dgv_tables.CellClick += dgv_tables_CellClick;
            // 
            // optionck
            // 
            optionck.HeaderText = "";
            optionck.Name = "optionck";
            optionck.ReadOnly = true;
            optionck.Width = 20;
            // 
            // tableName
            // 
            tableName.HeaderText = "表名";
            tableName.Name = "tableName";
            tableName.ReadOnly = true;
            // 
            // tableDesc
            // 
            tableDesc.HeaderText = "表描述";
            tableDesc.Name = "tableDesc";
            tableDesc.ReadOnly = true;
            // 
            // cbox_all
            // 
            cbox_all.AutoSize = true;
            cbox_all.Location = new Point(10, 67);
            cbox_all.Name = "cbox_all";
            cbox_all.Size = new Size(51, 21);
            cbox_all.TabIndex = 5;
            cbox_all.Text = "全选";
            cbox_all.UseVisualStyleBackColor = true;
            cbox_all.CheckedChanged += cbox_all_CheckedChanged;
            // 
            // txt_search
            // 
            txt_search.Location = new Point(67, 65);
            txt_search.Name = "txt_search";
            txt_search.Size = new Size(229, 23);
            txt_search.TabIndex = 6;
            txt_search.TextChanged += txt_search_TextChanged;
            // 
            // btn_generator
            // 
            btn_generator.Location = new Point(199, 124);
            btn_generator.Name = "btn_generator";
            btn_generator.Size = new Size(89, 25);
            btn_generator.TabIndex = 7;
            btn_generator.Text = "生成";
            btn_generator.UseVisualStyleBackColor = true;
            btn_generator.Click += btn_generator_Click;
            // 
            // cbx_template
            // 
            cbx_template.FormattingEnabled = true;
            cbx_template.Location = new Point(70, 6);
            cbx_template.Name = "cbx_template";
            cbx_template.Size = new Size(168, 25);
            cbx_template.TabIndex = 9;
            cbx_template.MouseDown += cbx_template_MouseDown;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(9, 9);
            label2.Name = "label2";
            label2.Size = new Size(56, 17);
            label2.TabIndex = 10;
            label2.Text = "选择模板";
            // 
            // btn_open_template
            // 
            btn_open_template.Location = new Point(296, 5);
            btn_open_template.Name = "btn_open_template";
            btn_open_template.Size = new Size(46, 25);
            btn_open_template.TabIndex = 11;
            btn_open_template.Text = "打开";
            btn_open_template.UseVisualStyleBackColor = true;
            btn_open_template.Click += btn_open_template_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(8, 69);
            label3.Name = "label3";
            label3.Size = new Size(56, 17);
            label3.TabIndex = 13;
            label3.Text = "文件前缀";
            // 
            // txt_filefirst
            // 
            txt_filefirst.Location = new Point(70, 66);
            txt_filefirst.Name = "txt_filefirst";
            txt_filefirst.Size = new Size(99, 23);
            txt_filefirst.TabIndex = 14;
            // 
            // txt_filelast
            // 
            txt_filelast.Location = new Point(243, 65);
            txt_filelast.Name = "txt_filelast";
            txt_filelast.Size = new Size(99, 23);
            txt_filelast.TabIndex = 16;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(182, 68);
            label4.Name = "label4";
            label4.Size = new Size(56, 17);
            label4.TabIndex = 15;
            label4.Text = "文件后缀";
            // 
            // txt_dir_path
            // 
            txt_dir_path.Location = new Point(70, 95);
            txt_dir_path.Name = "txt_dir_path";
            txt_dir_path.Size = new Size(272, 23);
            txt_dir_path.TabIndex = 17;
            // 
            // btn_open_browse_dir
            // 
            btn_open_browse_dir.Location = new Point(5, 94);
            btn_open_browse_dir.Name = "btn_open_browse_dir";
            btn_open_browse_dir.Size = new Size(59, 25);
            btn_open_browse_dir.TabIndex = 18;
            btn_open_browse_dir.Text = "目录.....";
            btn_open_browse_dir.UseVisualStyleBackColor = true;
            btn_open_browse_dir.Click += btn_open_browse_dir_Click;
            // 
            // btn_open_dir
            // 
            btn_open_dir.Location = new Point(70, 124);
            btn_open_dir.Name = "btn_open_dir";
            btn_open_dir.Size = new Size(88, 25);
            btn_open_dir.TabIndex = 19;
            btn_open_dir.Text = "打开目录";
            btn_open_dir.UseVisualStyleBackColor = true;
            btn_open_dir.Click += btn_open_dir_Click;
            // 
            // txt_namespace
            // 
            txt_namespace.Location = new Point(70, 36);
            txt_namespace.Name = "txt_namespace";
            txt_namespace.Size = new Size(272, 23);
            txt_namespace.TabIndex = 21;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(8, 39);
            label5.Name = "label5";
            label5.Size = new Size(56, 17);
            label5.TabIndex = 20;
            label5.Text = "命名空间";
            // 
            // btn_save_solution
            // 
            btn_save_solution.Location = new Point(70, 185);
            btn_save_solution.Name = "btn_save_solution";
            btn_save_solution.Size = new Size(85, 25);
            btn_save_solution.TabIndex = 22;
            btn_save_solution.Text = "保存为方案";
            btn_save_solution.UseVisualStyleBackColor = true;
            btn_save_solution.Click += btn_save_solution_Click;
            // 
            // dgv_solution
            // 
            dgv_solution.AllowUserToAddRows = false;
            dgv_solution.AllowUserToDeleteRows = false;
            dgv_solution.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_solution.Columns.AddRange(new DataGridViewColumn[] { dataGridViewCheckBoxColumn1, dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2 });
            dgv_solution.Dock = DockStyle.Fill;
            dgv_solution.Location = new Point(3, 223);
            dgv_solution.Name = "dgv_solution";
            dgv_solution.ReadOnly = true;
            dgv_solution.RowTemplate.Height = 25;
            dgv_solution.Size = new Size(376, 454);
            dgv_solution.TabIndex = 23;
            dgv_solution.CellClick += dgv_solution_CellClick;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            dataGridViewCheckBoxColumn1.HeaderText = "";
            dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            dataGridViewCheckBoxColumn1.ReadOnly = true;
            dataGridViewCheckBoxColumn1.Width = 20;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "方案名";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn2.HeaderText = "模板";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // btn_generator_solution
            // 
            btn_generator_solution.Location = new Point(199, 185);
            btn_generator_solution.Name = "btn_generator_solution";
            btn_generator_solution.Size = new Size(89, 25);
            btn_generator_solution.TabIndex = 24;
            btn_generator_solution.Text = "生成方案";
            btn_generator_solution.UseVisualStyleBackColor = true;
            btn_generator_solution.Click += btn_generator_solution_Click;
            // 
            // txt_solution_name
            // 
            txt_solution_name.Location = new Point(70, 156);
            txt_solution_name.Name = "txt_solution_name";
            txt_solution_name.Size = new Size(272, 23);
            txt_solution_name.TabIndex = 26;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(7, 159);
            label6.Name = "label6";
            label6.Size = new Size(56, 17);
            label6.TabIndex = 25;
            label6.Text = "方案名称";
            // 
            // button1
            // 
            button1.Location = new Point(250, 35);
            button1.Name = "button1";
            button1.Size = new Size(46, 24);
            button1.TabIndex = 27;
            button1.Text = "导出";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(180, 35);
            button2.Name = "button2";
            button2.Size = new Size(64, 24);
            button2.TabIndex = 28;
            button2.Text = "导出设置";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(694, 680);
            splitContainer1.SplitterDistance = 308;
            splitContainer1.TabIndex = 29;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(308, 680);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(button1);
            panel1.Controls.Add(btn_conn_db);
            panel1.Controls.Add(txt_connstr);
            panel1.Controls.Add(txt_search);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(cbx_dbtype);
            panel1.Controls.Add(cbox_all);
            panel1.Controls.Add(button2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(302, 94);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(dgv_tables);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 103);
            panel2.Name = "panel2";
            panel2.Size = new Size(302, 574);
            panel2.TabIndex = 1;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.FixedPanel = FixedPanel.Panel1;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(tableLayoutPanel2);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(tableLayoutPanel3);
            splitContainer2.Panel2Collapsed = true;
            splitContainer2.Size = new Size(382, 680);
            splitContainer2.SplitterDistance = 349;
            splitContainer2.TabIndex = 27;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(panel3, 0, 0);
            tableLayoutPanel2.Controls.Add(dgv_solution, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 220F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(382, 680);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(button5);
            panel3.Controls.Add(txt_namespace);
            panel3.Controls.Add(label2);
            panel3.Controls.Add(btn_open_browse_dir);
            panel3.Controls.Add(btn_generator_solution);
            panel3.Controls.Add(txt_dir_path);
            panel3.Controls.Add(btn_open_dir);
            panel3.Controls.Add(txt_solution_name);
            panel3.Controls.Add(txt_filelast);
            panel3.Controls.Add(btn_generator);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(label6);
            panel3.Controls.Add(label4);
            panel3.Controls.Add(cbx_template);
            panel3.Controls.Add(txt_filefirst);
            panel3.Controls.Add(btn_open_template);
            panel3.Controls.Add(btn_save_solution);
            panel3.Controls.Add(label3);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(376, 214);
            panel3.TabIndex = 0;
            // 
            // button5
            // 
            button5.Location = new Point(248, 5);
            button5.Name = "button5";
            button5.Size = new Size(42, 25);
            button5.TabIndex = 27;
            button5.Text = "添加";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(textEditorControl1, 0, 1);
            tableLayoutPanel3.Controls.Add(panel4, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(96, 100);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // textEditorControl1
            // 
            textEditorControl1.Dock = DockStyle.Fill;
            textEditorControl1.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            textEditorControl1.IsReadOnly = false;
            textEditorControl1.Location = new Point(3, 43);
            textEditorControl1.Name = "textEditorControl1";
            textEditorControl1.Size = new Size(90, 54);
            textEditorControl1.TabIndex = 2;
            // 
            // panel4
            // 
            panel4.Controls.Add(button4);
            panel4.Controls.Add(button3);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(3, 3);
            panel4.Name = "panel4";
            panel4.Size = new Size(90, 34);
            panel4.TabIndex = 0;
            // 
            // button4
            // 
            button4.Location = new Point(3, 5);
            button4.Name = "button4";
            button4.Size = new Size(47, 25);
            button4.TabIndex = 13;
            button4.Text = "保存";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.Location = new Point(56, 5);
            button3.Name = "button3";
            button3.Size = new Size(46, 25);
            button3.TabIndex = 12;
            button3.Text = "关闭";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(694, 680);
            Controls.Add(splitContainer1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FormMain";
            Text = "代码生成器";
            Load += FormMain_Load;
            ((System.ComponentModel.ISupportInitialize)dgv_tables).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgv_solution).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TextBox txt_connstr;
        private Label label1;
        private Button btn_conn_db;
        private ComboBox cbx_dbtype;
        private DataGridView dgv_tables;
        private DataGridViewCheckBoxColumn optionck;
        private DataGridViewTextBoxColumn tableName;
        private DataGridViewTextBoxColumn tableDesc;
        private CheckBox cbox_all;
        private TextBox txt_search;
        private Button btn_generator;
        private ComboBox cbx_template;
        private Label label2;
        private Button btn_open_template;
        private Label label3;
        private TextBox txt_filefirst;
        private TextBox txt_filelast;
        private Label label4;
        private TextBox txt_dir_path;
        private Button btn_open_browse_dir;
        private Button btn_open_dir;
        private TextBox txt_namespace;
        private Label label5;
        private Button btn_save_solution;
        private DataGridView dgv_solution;
        private Button btn_generator_solution;
        private TextBox txt_solution_name;
        private Label label6;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private Button button1;
        private Button button2;
        private SplitContainer splitContainer1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private Panel panel2;
        private SplitContainer splitContainer2;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel3;
        private TableLayoutPanel tableLayoutPanel3;
        private Panel panel4;
        private ICSharpCode.TextEditor.TextEditorControl textEditorControl1;
        private Button button4;
        private Button button3;
        private Button button5;
    }
}