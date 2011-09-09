namespace ePubFixer
{
    partial class BaseForm
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
            this.tree = new Aga.Controls.Tree.TreeViewAdv();
            this.TextCol = new Aga.Controls.Tree.TreeColumn();
            this.CheckCol = new Aga.Controls.Tree.TreeColumn();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Check = new System.Windows.Forms.ToolStripMenuItem();
            this.unCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.previewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nodeTextBox = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.nodeCheckBox = new Aga.Controls.Tree.NodeControls.NodeCheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Progress = new System.Windows.Forms.ToolStripProgressBar();
            this.cbSplit = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.contextMenu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tree
            // 
            this.tree.AllowDrop = true;
            this.tree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tree.BackColor = System.Drawing.SystemColors.Window;
            this.tree.Columns.Add(this.TextCol);
            this.tree.Columns.Add(this.CheckCol);
            this.tree.ContextMenuStrip = this.contextMenu;
            this.tree.DefaultToolTipProvider = null;
            this.tree.DisplayDraggingNodes = true;
            this.tree.DragDropMarkColor = System.Drawing.Color.Black;
            this.tree.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tree.LineColor = System.Drawing.SystemColors.ControlDark;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Model = null;
            this.tree.Name = "tree";
            this.tree.NodeControls.Add(this.nodeTextBox);
            this.tree.NodeControls.Add(this.nodeCheckBox);
            this.tree.SelectedNode = null;
            this.tree.SelectionMode = Aga.Controls.Tree.TreeSelectionMode.Multi;
            this.tree.ShowNodeToolTips = true;
            this.tree.Size = new System.Drawing.Size(492, 597);
            this.tree.TabIndex = 0;
            this.tree.TabStop = false;
            this.tree.Text = "treeViewAdv1";
            this.tree.UseColumns = true;
            this.tree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tree_ItemDrag);
            this.tree.NodeMouseClick += new System.EventHandler<Aga.Controls.Tree.TreeNodeAdvMouseEventArgs>(this.tree_NodeMouseClick);
            this.tree.Collapsed += new System.EventHandler<Aga.Controls.Tree.TreeViewAdvEventArgs>(this.tree_Collapsed);
            this.tree.DragDrop += new System.Windows.Forms.DragEventHandler(this.tree_DragDrop);
            this.tree.DragOver += new System.Windows.Forms.DragEventHandler(this.tree_DragOver);
            this.tree.DoubleClick += new System.EventHandler(this.tree_DoubleClick);
            this.tree.MouseHover += new System.EventHandler(this.tree_MouseHover);
            // 
            // TextCol
            // 
            this.TextCol.Header = "Title";
            this.TextCol.SortOrder = System.Windows.Forms.SortOrder.None;
            this.TextCol.TooltipText = null;
            this.TextCol.Width = 300;
            // 
            // CheckCol
            // 
            this.CheckCol.Header = "";
            this.CheckCol.MinColumnWidth = 35;
            this.CheckCol.SortOrder = System.Windows.Forms.SortOrder.None;
            this.CheckCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CheckCol.TooltipText = null;
            this.CheckCol.Width = 35;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.Check,
            this.unCheck,
            this.previewToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(186, 114);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_Opening);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.addToolStripMenuItem.Text = "Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.removeToolStripMenuItem.Text = "Delete";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // Check
            // 
            this.Check.Name = "Check";
            this.Check.Size = new System.Drawing.Size(185, 22);
            this.Check.Text = "Check All Selected";
            this.Check.Click += new System.EventHandler(this.checkAllSelectedToolStripMenuItem_Click);
            // 
            // unCheck
            // 
            this.unCheck.Name = "unCheck";
            this.unCheck.Size = new System.Drawing.Size(185, 22);
            this.unCheck.Text = "UnCheck All Selected";
            this.unCheck.Click += new System.EventHandler(this.unCheckAllSelectedToolStripMenuItem_Click);
            // 
            // previewToolStripMenuItem
            // 
            this.previewToolStripMenuItem.Name = "previewToolStripMenuItem";
            this.previewToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.previewToolStripMenuItem.Text = "Preview";
            this.previewToolStripMenuItem.Click += new System.EventHandler(this.previewToolStripMenuItem_Click);
            // 
            // nodeTextBox
            // 
            this.nodeTextBox.DataPropertyName = "Text";
            this.nodeTextBox.EditEnabled = true;
            this.nodeTextBox.IncrementalSearchEnabled = true;
            this.nodeTextBox.LeftMargin = 5;
            this.nodeTextBox.ParentColumn = this.TextCol;
            // 
            // nodeCheckBox
            // 
            this.nodeCheckBox.DataPropertyName = "IsChecked";
            this.nodeCheckBox.EditEnabled = true;
            this.nodeCheckBox.LeftMargin = 10;
            this.nodeCheckBox.ParentColumn = this.CheckCol;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(405, 621);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(324, 621);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(243, 621);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.Progress});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 658);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(492, 24);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(223, 19);
            this.statusLabel.Spring = true;
            this.statusLabel.Text = "                                                                        ";
            // 
            // Progress
            // 
            this.Progress.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.Progress.MarqueeAnimationSpeed = 50;
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(150, 18);
            this.Progress.Step = 2;
            this.Progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // cbSplit
            // 
            this.cbSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbSplit.AutoSize = true;
            this.cbSplit.Location = new System.Drawing.Point(12, 625);
            this.cbSplit.Name = "cbSplit";
            this.cbSplit.Size = new System.Drawing.Size(143, 17);
            this.cbSplit.TabIndex = 4;
            this.cbSplit.Text = "Split Chapter on Anchors";
            this.cbSplit.UseVisualStyleBackColor = true;
            // 
            // timer
            // 
            this.timer.Interval = 5000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // BaseForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(492, 682);
            this.Controls.Add(this.tree);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cbSplit);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnAdd);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(350, 400);
            this.Name = "BaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TOC Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BaseForm_FormClosing);
            this.Load += new System.EventHandler(this.frmTocEdit_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BaseForm_KeyUp);
            this.Resize += new System.EventHandler(this.BaseForm_Resize);
            this.contextMenu.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Aga.Controls.Tree.TreeColumn TextCol;
        private Aga.Controls.Tree.TreeColumn CheckCol;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAdd;
        protected System.Windows.Forms.ToolStripMenuItem Check;
        protected System.Windows.Forms.ToolStripMenuItem unCheck;
        protected System.Windows.Forms.ToolStripMenuItem previewToolStripMenuItem;
        protected System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        protected Aga.Controls.Tree.NodeControls.NodeTextBox nodeTextBox;
        protected Aga.Controls.Tree.NodeControls.NodeCheckBox nodeCheckBox;
        protected Aga.Controls.Tree.TreeViewAdv tree;
        protected System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        internal System.Windows.Forms.ToolStripProgressBar Progress;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        protected System.Windows.Forms.CheckBox cbSplit;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Timer timer;


    }
}