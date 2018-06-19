namespace InventoryScanner.UI
{
    partial class ScanningUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScanningUI));
            this.MainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ScanItemsGrid = new System.Windows.Forms.DataGridView();
            this.RightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ResolveDupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.StartScanButton = new System.Windows.Forms.Button();
            this.ScanEmployeeTextBox = new System.Windows.Forms.TextBox();
            this.ScanDateTimeTextBox = new System.Windows.Forms.TextBox();
            this.ScanLocationCombo = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.ScanLayoutTable = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ClearButton = new System.Windows.Forms.Button();
            this.SubmitScanButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.AssetTagTextBox = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.MunisPurchaseDtTextBox = new System.Windows.Forms.TextBox();
            this.MunisDepartmentTextBox = new System.Windows.Forms.TextBox();
            this.MunisLocationTextBox = new System.Windows.Forms.TextBox();
            this.MunisSerialTextBox = new System.Windows.Forms.TextBox();
            this.MunisDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.AssetStatusTextBox = new System.Windows.Forms.TextBox();
            this.AssetCurUserTextBox = new System.Windows.Forms.TextBox();
            this.AssetLastLocationTextBox = new System.Windows.Forms.TextBox();
            this.AssetTypeTextBox = new System.Windows.Forms.TextBox();
            this.AssetLocationTextBox = new System.Windows.Forms.TextBox();
            this.AssetSerialTextBox = new System.Windows.Forms.TextBox();
            this.AssetDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectPreviousScanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processWorksheetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filtersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FilterAllMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FilterAdminMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FilterFRSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FilterPROMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FilterOCMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FilterACMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FilterDUMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scannerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectScannerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rebuildCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ScannerStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.SyncStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainLayoutPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScanItemsGrid)).BeginInit();
            this.RightClickMenu.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.ScanLayoutTable.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainLayoutPanel
            // 
            this.MainLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainLayoutPanel.ColumnCount = 1;
            this.MainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayoutPanel.Controls.Add(this.groupBox1, 0, 1);
            this.MainLayoutPanel.Controls.Add(this.groupBox2, 0, 0);
            this.MainLayoutPanel.Controls.Add(this.ScanLayoutTable, 0, 2);
            this.MainLayoutPanel.Location = new System.Drawing.Point(0, 26);
            this.MainLayoutPanel.Name = "MainLayoutPanel";
            this.MainLayoutPanel.RowCount = 3;
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 315F));
            this.MainLayoutPanel.Size = new System.Drawing.Size(1273, 809);
            this.MainLayoutPanel.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.groupBox1.Controls.Add(this.ScanItemsGrid);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox1.Location = new System.Drawing.Point(3, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1267, 398);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Items To Scan";
            // 
            // ScanItemsGrid
            // 
            this.ScanItemsGrid.AllowUserToResizeRows = false;
            this.ScanItemsGrid.ColumnHeadersHeight = 30;
            this.ScanItemsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.ScanItemsGrid.ContextMenuStrip = this.RightClickMenu;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ScanItemsGrid.DefaultCellStyle = dataGridViewCellStyle1;
            this.ScanItemsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScanItemsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.ScanItemsGrid.Location = new System.Drawing.Point(3, 21);
            this.ScanItemsGrid.MultiSelect = false;
            this.ScanItemsGrid.Name = "ScanItemsGrid";
            this.ScanItemsGrid.ReadOnly = true;
            this.ScanItemsGrid.RowHeadersVisible = false;
            this.ScanItemsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ScanItemsGrid.ShowCellErrors = false;
            this.ScanItemsGrid.ShowCellToolTips = false;
            this.ScanItemsGrid.ShowEditingIcon = false;
            this.ScanItemsGrid.ShowRowErrors = false;
            this.ScanItemsGrid.Size = new System.Drawing.Size(1261, 374);
            this.ScanItemsGrid.TabIndex = 0;
            this.ScanItemsGrid.VirtualMode = true;
            this.ScanItemsGrid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ScanItemsGrid_CellMouseClick);
            this.ScanItemsGrid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ScanItemsGrid_CellMouseDown);
            this.ScanItemsGrid.SelectionChanged += new System.EventHandler(this.ScanItemsGrid_SelectionChanged);
            this.ScanItemsGrid.Sorted += new System.EventHandler(this.ScanItemsGrid_Sorted);
            this.ScanItemsGrid.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ScanItemsGrid_KeyUp);
            // 
            // RightClickMenu
            // 
            this.RightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ResolveDupMenuItem});
            this.RightClickMenu.Name = "RightClickMenu";
            this.RightClickMenu.Size = new System.Drawing.Size(168, 26);
            // 
            // ResolveDupMenuItem
            // 
            this.ResolveDupMenuItem.Name = "ResolveDupMenuItem";
            this.ResolveDupMenuItem.Size = new System.Drawing.Size(167, 22);
            this.ResolveDupMenuItem.Text = "Resolve Duplicate";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.StartScanButton);
            this.groupBox2.Controls.Add(this.ScanEmployeeTextBox);
            this.groupBox2.Controls.Add(this.ScanDateTimeTextBox);
            this.groupBox2.Controls.Add(this.ScanLocationCombo);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1267, 84);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Scan Info";
            // 
            // StartScanButton
            // 
            this.StartScanButton.BackColor = System.Drawing.Color.DimGray;
            this.StartScanButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartScanButton.Location = new System.Drawing.Point(742, 41);
            this.StartScanButton.Name = "StartScanButton";
            this.StartScanButton.Size = new System.Drawing.Size(165, 27);
            this.StartScanButton.TabIndex = 8;
            this.StartScanButton.Text = "Start New Scan";
            this.StartScanButton.UseVisualStyleBackColor = false;
            this.StartScanButton.Click += new System.EventHandler(this.StartScanButton_Click);
            // 
            // ScanEmployeeTextBox
            // 
            this.ScanEmployeeTextBox.Location = new System.Drawing.Point(518, 43);
            this.ScanEmployeeTextBox.Name = "ScanEmployeeTextBox";
            this.ScanEmployeeTextBox.Size = new System.Drawing.Size(200, 25);
            this.ScanEmployeeTextBox.TabIndex = 7;
            // 
            // ScanDateTimeTextBox
            // 
            this.ScanDateTimeTextBox.Location = new System.Drawing.Point(303, 43);
            this.ScanDateTimeTextBox.Name = "ScanDateTimeTextBox";
            this.ScanDateTimeTextBox.Size = new System.Drawing.Size(200, 25);
            this.ScanDateTimeTextBox.TabIndex = 6;
            // 
            // ScanLocationCombo
            // 
            this.ScanLocationCombo.FormattingEnabled = true;
            this.ScanLocationCombo.Location = new System.Drawing.Point(12, 43);
            this.ScanLocationCombo.Name = "ScanLocationCombo";
            this.ScanLocationCombo.Size = new System.Drawing.Size(276, 26);
            this.ScanLocationCombo.TabIndex = 3;
            this.ScanLocationCombo.SelectedIndexChanged += new System.EventHandler(this.ScanLocationCombo_SelectedIndexChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(515, 22);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(72, 18);
            this.label16.TabIndex = 2;
            this.label16.Text = "Employee";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(300, 21);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(80, 18);
            this.label15.TabIndex = 1;
            this.label15.Text = "Date\\Time";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(9, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(72, 18);
            this.label14.TabIndex = 0;
            this.label14.Text = "Location";
            // 
            // ScanLayoutTable
            // 
            this.ScanLayoutTable.ColumnCount = 2;
            this.ScanLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 409F));
            this.ScanLayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ScanLayoutTable.Controls.Add(this.groupBox3, 0, 0);
            this.ScanLayoutTable.Controls.Add(this.groupBox6, 1, 0);
            this.ScanLayoutTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ScanLayoutTable.Location = new System.Drawing.Point(3, 497);
            this.ScanLayoutTable.Name = "ScanLayoutTable";
            this.ScanLayoutTable.RowCount = 1;
            this.ScanLayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ScanLayoutTable.Size = new System.Drawing.Size(1267, 309);
            this.ScanLayoutTable.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ClearButton);
            this.groupBox3.Controls.Add(this.SubmitScanButton);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.AssetTagTextBox);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(403, 303);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Scanning";
            // 
            // ClearButton
            // 
            this.ClearButton.BackColor = System.Drawing.Color.DimGray;
            this.ClearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClearButton.Location = new System.Drawing.Point(39, 240);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(310, 35);
            this.ClearButton.TabIndex = 3;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = false;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // SubmitScanButton
            // 
            this.SubmitScanButton.BackColor = System.Drawing.Color.DimGray;
            this.SubmitScanButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SubmitScanButton.Location = new System.Drawing.Point(39, 127);
            this.SubmitScanButton.Name = "SubmitScanButton";
            this.SubmitScanButton.Size = new System.Drawing.Size(310, 49);
            this.SubmitScanButton.TabIndex = 2;
            this.SubmitScanButton.Text = "Submit Manual Scan";
            this.SubmitScanButton.UseVisualStyleBackColor = false;
            this.SubmitScanButton.Click += new System.EventHandler(this.SubmitScanButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Asset Tag:";
            // 
            // AssetTagTextBox
            // 
            this.AssetTagTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(230)))), ((int)(((byte)(119)))));
            this.AssetTagTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AssetTagTextBox.Font = new System.Drawing.Font("Consolas", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AssetTagTextBox.Location = new System.Drawing.Point(39, 60);
            this.AssetTagTextBox.Name = "AssetTagTextBox";
            this.AssetTagTextBox.Size = new System.Drawing.Size(310, 51);
            this.AssetTagTextBox.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.tableLayoutPanel1);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox6.Location = new System.Drawing.Point(412, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(852, 303);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Current Item Info";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.32576F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.67424F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox5, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 21);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(846, 279);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(54)))), ((int)(((byte)(107)))));
            this.groupBox4.Controls.Add(this.MunisPurchaseDtTextBox);
            this.groupBox4.Controls.Add(this.MunisDepartmentTextBox);
            this.groupBox4.Controls.Add(this.MunisLocationTextBox);
            this.groupBox4.Controls.Add(this.MunisSerialTextBox);
            this.groupBox4.Controls.Add(this.MunisDescriptionTextBox);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(385, 273);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Munis";
            // 
            // MunisPurchaseDtTextBox
            // 
            this.MunisPurchaseDtTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MunisPurchaseDtTextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MunisPurchaseDtTextBox.Location = new System.Drawing.Point(18, 237);
            this.MunisPurchaseDtTextBox.Name = "MunisPurchaseDtTextBox";
            this.MunisPurchaseDtTextBox.ReadOnly = true;
            this.MunisPurchaseDtTextBox.Size = new System.Drawing.Size(351, 26);
            this.MunisPurchaseDtTextBox.TabIndex = 9;
            // 
            // MunisDepartmentTextBox
            // 
            this.MunisDepartmentTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MunisDepartmentTextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MunisDepartmentTextBox.Location = new System.Drawing.Point(19, 189);
            this.MunisDepartmentTextBox.Name = "MunisDepartmentTextBox";
            this.MunisDepartmentTextBox.ReadOnly = true;
            this.MunisDepartmentTextBox.Size = new System.Drawing.Size(350, 26);
            this.MunisDepartmentTextBox.TabIndex = 8;
            // 
            // MunisLocationTextBox
            // 
            this.MunisLocationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MunisLocationTextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MunisLocationTextBox.Location = new System.Drawing.Point(19, 140);
            this.MunisLocationTextBox.Name = "MunisLocationTextBox";
            this.MunisLocationTextBox.ReadOnly = true;
            this.MunisLocationTextBox.Size = new System.Drawing.Size(350, 26);
            this.MunisLocationTextBox.TabIndex = 7;
            // 
            // MunisSerialTextBox
            // 
            this.MunisSerialTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MunisSerialTextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MunisSerialTextBox.Location = new System.Drawing.Point(19, 92);
            this.MunisSerialTextBox.Name = "MunisSerialTextBox";
            this.MunisSerialTextBox.ReadOnly = true;
            this.MunisSerialTextBox.Size = new System.Drawing.Size(350, 26);
            this.MunisSerialTextBox.TabIndex = 6;
            // 
            // MunisDescriptionTextBox
            // 
            this.MunisDescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MunisDescriptionTextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MunisDescriptionTextBox.Location = new System.Drawing.Point(18, 44);
            this.MunisDescriptionTextBox.Name = "MunisDescriptionTextBox";
            this.MunisDescriptionTextBox.ReadOnly = true;
            this.MunisDescriptionTextBox.Size = new System.Drawing.Size(351, 26);
            this.MunisDescriptionTextBox.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 168);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 18);
            this.label6.TabIndex = 4;
            this.label6.Text = "Department";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 216);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 18);
            this.label5.TabIndex = 3;
            this.label5.Text = "Purchase Date";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 18);
            this.label4.TabIndex = 2;
            this.label4.Text = "Location";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 18);
            this.label3.TabIndex = 1;
            this.label3.Text = "Description";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Serial";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(91)))), ((int)(((byte)(40)))));
            this.groupBox5.Controls.Add(this.AssetStatusTextBox);
            this.groupBox5.Controls.Add(this.AssetCurUserTextBox);
            this.groupBox5.Controls.Add(this.AssetLastLocationTextBox);
            this.groupBox5.Controls.Add(this.AssetTypeTextBox);
            this.groupBox5.Controls.Add(this.AssetLocationTextBox);
            this.groupBox5.Controls.Add(this.AssetSerialTextBox);
            this.groupBox5.Controls.Add(this.AssetDescriptionTextBox);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox5.Location = new System.Drawing.Point(394, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(449, 273);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Asset Manager";
            // 
            // AssetStatusTextBox
            // 
            this.AssetStatusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AssetStatusTextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AssetStatusTextBox.Location = new System.Drawing.Point(193, 237);
            this.AssetStatusTextBox.Name = "AssetStatusTextBox";
            this.AssetStatusTextBox.ReadOnly = true;
            this.AssetStatusTextBox.Size = new System.Drawing.Size(236, 26);
            this.AssetStatusTextBox.TabIndex = 18;
            // 
            // AssetCurUserTextBox
            // 
            this.AssetCurUserTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AssetCurUserTextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AssetCurUserTextBox.Location = new System.Drawing.Point(193, 189);
            this.AssetCurUserTextBox.Name = "AssetCurUserTextBox";
            this.AssetCurUserTextBox.ReadOnly = true;
            this.AssetCurUserTextBox.Size = new System.Drawing.Size(236, 26);
            this.AssetCurUserTextBox.TabIndex = 17;
            // 
            // AssetLastLocationTextBox
            // 
            this.AssetLastLocationTextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AssetLastLocationTextBox.Location = new System.Drawing.Point(17, 237);
            this.AssetLastLocationTextBox.Name = "AssetLastLocationTextBox";
            this.AssetLastLocationTextBox.ReadOnly = true;
            this.AssetLastLocationTextBox.Size = new System.Drawing.Size(147, 26);
            this.AssetLastLocationTextBox.TabIndex = 16;
            // 
            // AssetTypeTextBox
            // 
            this.AssetTypeTextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AssetTypeTextBox.Location = new System.Drawing.Point(17, 189);
            this.AssetTypeTextBox.Name = "AssetTypeTextBox";
            this.AssetTypeTextBox.ReadOnly = true;
            this.AssetTypeTextBox.Size = new System.Drawing.Size(147, 26);
            this.AssetTypeTextBox.TabIndex = 15;
            // 
            // AssetLocationTextBox
            // 
            this.AssetLocationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AssetLocationTextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AssetLocationTextBox.Location = new System.Drawing.Point(17, 140);
            this.AssetLocationTextBox.Name = "AssetLocationTextBox";
            this.AssetLocationTextBox.ReadOnly = true;
            this.AssetLocationTextBox.Size = new System.Drawing.Size(412, 26);
            this.AssetLocationTextBox.TabIndex = 14;
            // 
            // AssetSerialTextBox
            // 
            this.AssetSerialTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AssetSerialTextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AssetSerialTextBox.Location = new System.Drawing.Point(17, 92);
            this.AssetSerialTextBox.Name = "AssetSerialTextBox";
            this.AssetSerialTextBox.ReadOnly = true;
            this.AssetSerialTextBox.Size = new System.Drawing.Size(412, 26);
            this.AssetSerialTextBox.TabIndex = 13;
            // 
            // AssetDescriptionTextBox
            // 
            this.AssetDescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AssetDescriptionTextBox.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AssetDescriptionTextBox.Location = new System.Drawing.Point(17, 44);
            this.AssetDescriptionTextBox.Name = "AssetDescriptionTextBox";
            this.AssetDescriptionTextBox.ReadOnly = true;
            this.AssetDescriptionTextBox.Size = new System.Drawing.Size(412, 26);
            this.AssetDescriptionTextBox.TabIndex = 12;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(191, 216);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 18);
            this.label13.TabIndex = 11;
            this.label13.Text = "Status";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(191, 168);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(104, 18);
            this.label12.TabIndex = 10;
            this.label12.Text = "Current User";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 168);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 18);
            this.label8.TabIndex = 9;
            this.label8.Text = "Device Type";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 216);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(152, 18);
            this.label7.TabIndex = 8;
            this.label7.Text = "Last Seen Location";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 119);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 18);
            this.label9.TabIndex = 7;
            this.label9.Text = "Location";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 18);
            this.label10.TabIndex = 6;
            this.label10.Text = "Description";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 71);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 18);
            this.label11.TabIndex = 5;
            this.label11.Text = "Serial";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.filtersToolStripMenuItem,
            this.scannerToolStripMenuItem,
            this.databaseToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1273, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectPreviousScanToolStripMenuItem,
            this.processWorksheetToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // selectPreviousScanToolStripMenuItem
            // 
            this.selectPreviousScanToolStripMenuItem.Name = "selectPreviousScanToolStripMenuItem";
            this.selectPreviousScanToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.selectPreviousScanToolStripMenuItem.Text = "Select Previous Scan";
            this.selectPreviousScanToolStripMenuItem.Click += new System.EventHandler(this.selectPreviousScanToolStripMenuItem_Click);
            // 
            // processWorksheetToolStripMenuItem
            // 
            this.processWorksheetToolStripMenuItem.Name = "processWorksheetToolStripMenuItem";
            this.processWorksheetToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.processWorksheetToolStripMenuItem.Text = "Process Worksheet";
            this.processWorksheetToolStripMenuItem.Click += new System.EventHandler(this.processWorksheetToolStripMenuItem_Click);
            // 
            // filtersToolStripMenuItem
            // 
            this.filtersToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.filtersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FilterAllMenuItem,
            this.FilterAdminMenuItem,
            this.FilterFRSMenuItem,
            this.FilterPROMenuItem,
            this.FilterOCMenuItem,
            this.FilterACMenuItem,
            this.FilterDUMenuItem});
            this.filtersToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.filtersToolStripMenuItem.Name = "filtersToolStripMenuItem";
            this.filtersToolStripMenuItem.Size = new System.Drawing.Size(99, 20);
            this.filtersToolStripMenuItem.Text = "Location Filters";
            // 
            // FilterAllMenuItem
            // 
            this.FilterAllMenuItem.Checked = true;
            this.FilterAllMenuItem.CheckOnClick = true;
            this.FilterAllMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FilterAllMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FilterAllMenuItem.Name = "FilterAllMenuItem";
            this.FilterAllMenuItem.Size = new System.Drawing.Size(225, 22);
            this.FilterAllMenuItem.Tag = "All";
            this.FilterAllMenuItem.Text = "All";
            // 
            // FilterAdminMenuItem
            // 
            this.FilterAdminMenuItem.CheckOnClick = true;
            this.FilterAdminMenuItem.Name = "FilterAdminMenuItem";
            this.FilterAdminMenuItem.Size = new System.Drawing.Size(225, 22);
            this.FilterAdminMenuItem.Tag = "750";
            this.FilterAdminMenuItem.Text = "Admin";
            // 
            // FilterFRSMenuItem
            // 
            this.FilterFRSMenuItem.CheckOnClick = true;
            this.FilterFRSMenuItem.Name = "FilterFRSMenuItem";
            this.FilterFRSMenuItem.Size = new System.Drawing.Size(225, 22);
            this.FilterFRSMenuItem.Tag = "753";
            this.FilterFRSMenuItem.Text = "Forest Rose School";
            // 
            // FilterPROMenuItem
            // 
            this.FilterPROMenuItem.CheckOnClick = true;
            this.FilterPROMenuItem.Name = "FilterPROMenuItem";
            this.FilterPROMenuItem.Size = new System.Drawing.Size(225, 22);
            this.FilterPROMenuItem.Tag = "758";
            this.FilterPROMenuItem.Text = "Pickerington Regional Office";
            // 
            // FilterOCMenuItem
            // 
            this.FilterOCMenuItem.CheckOnClick = true;
            this.FilterOCMenuItem.Name = "FilterOCMenuItem";
            this.FilterOCMenuItem.Size = new System.Drawing.Size(225, 22);
            this.FilterOCMenuItem.Tag = "754";
            this.FilterOCMenuItem.Text = "Opportunity Center";
            // 
            // FilterACMenuItem
            // 
            this.FilterACMenuItem.CheckOnClick = true;
            this.FilterACMenuItem.Name = "FilterACMenuItem";
            this.FilterACMenuItem.Size = new System.Drawing.Size(225, 22);
            this.FilterACMenuItem.Tag = "757";
            this.FilterACMenuItem.Text = "Art && Clay";
            // 
            // FilterDUMenuItem
            // 
            this.FilterDUMenuItem.CheckOnClick = true;
            this.FilterDUMenuItem.Name = "FilterDUMenuItem";
            this.FilterDUMenuItem.Size = new System.Drawing.Size(225, 22);
            this.FilterDUMenuItem.Tag = "759";
            this.FilterDUMenuItem.Text = "Discover U";
            // 
            // scannerToolStripMenuItem
            // 
            this.scannerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectScannerToolStripMenuItem});
            this.scannerToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.scannerToolStripMenuItem.Name = "scannerToolStripMenuItem";
            this.scannerToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.scannerToolStripMenuItem.Text = "Scanner";
            // 
            // selectScannerToolStripMenuItem
            // 
            this.selectScannerToolStripMenuItem.Name = "selectScannerToolStripMenuItem";
            this.selectScannerToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.selectScannerToolStripMenuItem.Text = "Connect Scanner";
            this.selectScannerToolStripMenuItem.Click += new System.EventHandler(this.selectScannerToolStripMenuItem_Click);
            // 
            // databaseToolStripMenuItem
            // 
            this.databaseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rebuildCacheToolStripMenuItem});
            this.databaseToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
            this.databaseToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.databaseToolStripMenuItem.Text = "Database";
            // 
            // rebuildCacheToolStripMenuItem
            // 
            this.rebuildCacheToolStripMenuItem.Name = "rebuildCacheToolStripMenuItem";
            this.rebuildCacheToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.rebuildCacheToolStripMenuItem.Text = "Rebuild Cache";
            this.rebuildCacheToolStripMenuItem.Click += new System.EventHandler(this.rebuildCacheToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.ScannerStatusLabel,
            this.SyncStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 837);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1273, 45);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(886, 40);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // ScannerStatusLabel
            // 
            this.ScannerStatusLabel.ForeColor = System.Drawing.Color.Black;
            this.ScannerStatusLabel.Name = "ScannerStatusLabel";
            this.ScannerStatusLabel.Size = new System.Drawing.Size(221, 40);
            this.ScannerStatusLabel.Text = "Scanner: Not Connected";
            // 
            // SyncStatusLabel
            // 
            this.SyncStatusLabel.ForeColor = System.Drawing.Color.DarkRed;
            this.SyncStatusLabel.Name = "SyncStatusLabel";
            this.SyncStatusLabel.Size = new System.Drawing.Size(153, 40);
            this.SyncStatusLabel.Text = "Last Sync: Never";
            // 
            // ScanningUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.ClientSize = new System.Drawing.Size(1273, 882);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.MainLayoutPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1023, 604);
            this.Name = "ScanningUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inventory Scanner";
            this.MainLayoutPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ScanItemsGrid)).EndInit();
            this.RightClickMenu.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ScanLayoutTable.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainLayoutPanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView ScanItemsGrid;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel ScanLayoutTable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox AssetTagTextBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button SubmitScanButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox MunisDescriptionTextBox;
        private System.Windows.Forms.TextBox MunisPurchaseDtTextBox;
        private System.Windows.Forms.TextBox MunisDepartmentTextBox;
        private System.Windows.Forms.TextBox MunisLocationTextBox;
        private System.Windows.Forms.TextBox MunisSerialTextBox;
        private System.Windows.Forms.TextBox AssetStatusTextBox;
        private System.Windows.Forms.TextBox AssetCurUserTextBox;
        private System.Windows.Forms.TextBox AssetLastLocationTextBox;
        private System.Windows.Forms.TextBox AssetTypeTextBox;
        private System.Windows.Forms.TextBox AssetLocationTextBox;
        private System.Windows.Forms.TextBox AssetSerialTextBox;
        private System.Windows.Forms.TextBox AssetDescriptionTextBox;
        private System.Windows.Forms.ComboBox ScanLocationCombo;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox ScanEmployeeTextBox;
        private System.Windows.Forms.TextBox ScanDateTimeTextBox;
        private System.Windows.Forms.Button StartScanButton;
        private System.Windows.Forms.ContextMenuStrip RightClickMenu;
        private System.Windows.Forms.ToolStripMenuItem ResolveDupMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processWorksheetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filtersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FilterAllMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FilterAdminMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FilterFRSMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FilterPROMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FilterOCMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FilterACMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FilterDUMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectPreviousScanToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel ScannerStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem scannerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectScannerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rebuildCacheToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel SyncStatusLabel;
    }
}

