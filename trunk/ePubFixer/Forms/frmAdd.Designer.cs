namespace ePubFixer
{
    partial class frmAdd
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
            this.components = new System.ComponentModel.Container();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.treeView1 = new Aga.Controls.Tree.TreeViewAdv();
            this.colFile = new Aga.Controls.Tree.TreeColumn();
            this.colDetectedText = new Aga.Controls.Tree.TreeColumn();
            this.colCheckbox = new Aga.Controls.Tree.TreeColumn();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.previewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.check = new System.Windows.Forms.ToolStripMenuItem();
            this.unCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNextTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nodeFile = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeDetectedTexts = new Aga.Controls.Tree.NodeControls.NodeComboBox();
            this.nodeCheckBox1 = new Aga.Controls.Tree.NodeControls.NodeCheckBox();
            this.cbShowAll = new System.Windows.Forms.CheckBox();
            this.cbShowAnchors = new System.Windows.Forms.CheckBox();
            this.btnSearchNet = new System.Windows.Forms.Button();
            this.selectNextTextIncremental = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(591, 528);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 25);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(496, 528);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(87, 25);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Add";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.BackColor = System.Drawing.SystemColors.Window;
            this.treeView1.Columns.Add(this.colFile);
            this.treeView1.Columns.Add(this.colDetectedText);
            this.treeView1.Columns.Add(this.colCheckbox);
            this.treeView1.ContextMenuStrip = this.contextMenu;
            this.treeView1.DefaultToolTipProvider = null;
            this.treeView1.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeView1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Model = null;
            this.treeView1.Name = "treeView1";
            this.treeView1.NodeControls.Add(this.nodeFile);
            this.treeView1.NodeControls.Add(this.nodeDetectedTexts);
            this.treeView1.NodeControls.Add(this.nodeCheckBox1);
            this.treeView1.SelectedNode = null;
            this.treeView1.SelectionMode = Aga.Controls.Tree.TreeSelectionMode.Multi;
            this.treeView1.Size = new System.Drawing.Size(692, 496);
            this.treeView1.TabIndex = 2;
            this.treeView1.Text = "treeViewAdv1";
            this.treeView1.UseColumns = true;
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tree_ItemDrag);
            this.treeView1.ColumnClicked += new System.EventHandler<Aga.Controls.Tree.TreeColumnEventArgs>(this.treeView1_ColumnClicked);
            this.treeView1.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView1_DragOver);
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // colFile
            // 
            this.colFile.Header = "File";
            this.colFile.MinColumnWidth = 250;
            this.colFile.Sortable = true;
            this.colFile.SortOrder = System.Windows.Forms.SortOrder.None;
            this.colFile.TooltipText = null;
            this.colFile.Width = 300;
            // 
            // colDetectedText
            // 
            this.colDetectedText.Header = "Detected Text";
            this.colDetectedText.MinColumnWidth = 300;
            this.colDetectedText.Sortable = true;
            this.colDetectedText.SortOrder = System.Windows.Forms.SortOrder.None;
            this.colDetectedText.TooltipText = null;
            this.colDetectedText.Width = 300;
            // 
            // colCheckbox
            // 
            this.colCheckbox.Header = "";
            this.colCheckbox.MaxColumnWidth = 35;
            this.colCheckbox.MinColumnWidth = 35;
            this.colCheckbox.SortOrder = System.Windows.Forms.SortOrder.None;
            this.colCheckbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colCheckbox.TooltipText = null;
            this.colCheckbox.Width = 35;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.previewToolStripMenuItem,
            this.expandAllToolStripMenuItem,
            this.collapseAllToolStripMenuItem,
            this.check,
            this.unCheck,
            this.selectNextTextToolStripMenuItem,
            this.selectNextTextIncremental,
            this.deleteFilesToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(232, 202);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // previewToolStripMenuItem
            // 
            this.previewToolStripMenuItem.Name = "previewToolStripMenuItem";
            this.previewToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.previewToolStripMenuItem.Text = "Preview";
            this.previewToolStripMenuItem.Click += new System.EventHandler(this.previewToolStripMenuItem_Click);
            // 
            // expandAllToolStripMenuItem
            // 
            this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
            this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.expandAllToolStripMenuItem.Text = "Expand All";
            this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.expandAllToolStripMenuItem_Click);
            // 
            // collapseAllToolStripMenuItem
            // 
            this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
            this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.collapseAllToolStripMenuItem.Text = "Collapse All";
            this.collapseAllToolStripMenuItem.Click += new System.EventHandler(this.collapseAllToolStripMenuItem_Click);
            // 
            // check
            // 
            this.check.Name = "check";
            this.check.Size = new System.Drawing.Size(231, 22);
            this.check.Text = "Check All Selected";
            this.check.Click += new System.EventHandler(this.check_Click);
            // 
            // unCheck
            // 
            this.unCheck.Name = "unCheck";
            this.unCheck.Size = new System.Drawing.Size(231, 22);
            this.unCheck.Text = "unCheck All Selected";
            this.unCheck.Click += new System.EventHandler(this.unCheck_Click);
            // 
            // selectNextTextToolStripMenuItem
            // 
            this.selectNextTextToolStripMenuItem.Name = "selectNextTextToolStripMenuItem";
            this.selectNextTextToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.selectNextTextToolStripMenuItem.Text = "Select Next Text";
            this.selectNextTextToolStripMenuItem.Click += new System.EventHandler(this.selectNextTextToolStripMenuItem_Click);
            // 
            // deleteFilesToolStripMenuItem
            // 
            this.deleteFilesToolStripMenuItem.Name = "deleteFilesToolStripMenuItem";
            this.deleteFilesToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.deleteFilesToolStripMenuItem.Text = "Delete Files";
            this.deleteFilesToolStripMenuItem.Click += new System.EventHandler(this.deleteFilesToolStripMenuItem_Click);
            // 
            // nodeFile
            // 
            this.nodeFile.DataPropertyName = "Text";
            this.nodeFile.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nodeFile.IncrementalSearchEnabled = true;
            this.nodeFile.LeftMargin = 3;
            this.nodeFile.ParentColumn = this.colFile;
            // 
            // nodeDetectedTexts
            // 
            this.nodeDetectedTexts.DataPropertyName = "DetectedText";
            this.nodeDetectedTexts.EditEnabled = true;
            this.nodeDetectedTexts.EditOnClick = true;
            this.nodeDetectedTexts.EditorHeight = 150;
            this.nodeDetectedTexts.EditorWidth = 300;
            this.nodeDetectedTexts.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nodeDetectedTexts.IncrementalSearchEnabled = true;
            this.nodeDetectedTexts.LeftMargin = 3;
            this.nodeDetectedTexts.ParentColumn = this.colDetectedText;
            this.nodeDetectedTexts.TrimMultiLine = true;
            // 
            // nodeCheckBox1
            // 
            this.nodeCheckBox1.DataPropertyName = "IsChecked";
            this.nodeCheckBox1.EditEnabled = true;
            this.nodeCheckBox1.LeftMargin = 10;
            this.nodeCheckBox1.ParentColumn = this.colCheckbox;
            // 
            // cbShowAll
            // 
            this.cbShowAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbShowAll.AutoSize = true;
            this.cbShowAll.Location = new System.Drawing.Point(12, 532);
            this.cbShowAll.Name = "cbShowAll";
            this.cbShowAll.Size = new System.Drawing.Size(73, 18);
            this.cbShowAll.TabIndex = 3;
            this.cbShowAll.Text = "Show All";
            this.cbShowAll.UseVisualStyleBackColor = true;
            this.cbShowAll.CheckedChanged += new System.EventHandler(this.cbShowAll_CheckedChanged);
            // 
            // cbShowAnchors
            // 
            this.cbShowAnchors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbShowAnchors.AutoSize = true;
            this.cbShowAnchors.Checked = global::ePubFixer.Properties.Settings.Default.ShowAnchor;
            this.cbShowAnchors.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ePubFixer.Properties.Settings.Default, "ShowAnchor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbShowAnchors.Location = new System.Drawing.Point(91, 532);
            this.cbShowAnchors.Name = "cbShowAnchors";
            this.cbShowAnchors.Size = new System.Drawing.Size(105, 18);
            this.cbShowAnchors.TabIndex = 4;
            this.cbShowAnchors.Text = "Show Anchors";
            this.cbShowAnchors.UseVisualStyleBackColor = true;
            this.cbShowAnchors.CheckedChanged += new System.EventHandler(this.cbShowAnchors_CheckedChanged);
            // 
            // btnSearchNet
            // 
            this.btnSearchNet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchNet.Location = new System.Drawing.Point(403, 528);
            this.btnSearchNet.Name = "btnSearchNet";
            this.btnSearchNet.Size = new System.Drawing.Size(87, 25);
            this.btnSearchNet.TabIndex = 5;
            this.btnSearchNet.Text = "Download";
            this.btnSearchNet.UseVisualStyleBackColor = true;
            this.btnSearchNet.Click += new System.EventHandler(this.btnSearchNet_Click);
            // 
            // selectNextTextIncremental
            // 
            this.selectNextTextIncremental.Name = "selectNextTextIncremental";
            this.selectNextTextIncremental.Size = new System.Drawing.Size(231, 22);
            this.selectNextTextIncremental.Text = "Select Next Text (Incremental)";
            this.selectNextTextIncremental.Click += new System.EventHandler(this.selectNextTextIncremental_Click);
            // 
            // frmAdd
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(692, 566);
            this.Controls.Add(this.cbShowAll);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnSearchNet);
            this.Controls.Add(this.cbShowAnchors);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "frmAdd";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAdd_FormClosing);
            this.Load += new System.EventHandler(this.frmAdd_Load);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private Aga.Controls.Tree.TreeViewAdv treeView1;
        private Aga.Controls.Tree.TreeColumn colFile;
        private Aga.Controls.Tree.TreeColumn colCheckbox;
        private Aga.Controls.Tree.NodeControls.NodeTextBox nodeFile;
        private Aga.Controls.Tree.NodeControls.NodeCheckBox nodeCheckBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem previewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem check;
        private System.Windows.Forms.ToolStripMenuItem unCheck;
        private System.Windows.Forms.CheckBox cbShowAll;
        private System.Windows.Forms.CheckBox cbShowAnchors;
        private Aga.Controls.Tree.TreeColumn colDetectedText;
        private Aga.Controls.Tree.NodeControls.NodeComboBox nodeDetectedTexts;
        private System.Windows.Forms.ToolStripMenuItem selectNextTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFilesToolStripMenuItem;
        private System.Windows.Forms.Button btnSearchNet;
        private System.Windows.Forms.ToolStripMenuItem selectNextTextIncremental;
    }
}