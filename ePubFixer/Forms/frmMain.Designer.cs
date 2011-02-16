namespace ePubFixer
{
    partial class frmMain
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnFiles = new System.Windows.Forms.Button();
            this.btnFolder = new System.Windows.Forms.Button();
            this.cbFixMargins = new System.Windows.Forms.CheckBox();
            this.cbTocEdit = new System.Windows.Forms.CheckBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbSigil = new System.Windows.Forms.CheckBox();
            this.cbReadingOrder = new System.Windows.Forms.CheckBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.SettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sigilPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.about = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.cbBackup = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = ".epub | *.epub";
            this.openFileDialog1.Multiselect = true;
            // 
            // btnFiles
            // 
            this.btnFiles.Location = new System.Drawing.Point(217, 32);
            this.btnFiles.Name = "btnFiles";
            this.btnFiles.Size = new System.Drawing.Size(85, 28);
            this.btnFiles.TabIndex = 1;
            this.btnFiles.Text = "Select Files";
            this.btnFiles.UseVisualStyleBackColor = true;
            this.btnFiles.Click += new System.EventHandler(this.btnFiles_Click);
            // 
            // btnFolder
            // 
            this.btnFolder.Location = new System.Drawing.Point(217, 66);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(85, 28);
            this.btnFolder.TabIndex = 2;
            this.btnFolder.Text = "Select Folder";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // cbFixMargins
            // 
            this.cbFixMargins.AutoSize = true;
            this.cbFixMargins.Location = new System.Drawing.Point(10, 23);
            this.cbFixMargins.Name = "cbFixMargins";
            this.cbFixMargins.Size = new System.Drawing.Size(79, 17);
            this.cbFixMargins.TabIndex = 0;
            this.cbFixMargins.Tag = "css";
            this.cbFixMargins.Text = "Fix Margins";
            this.cbFixMargins.UseVisualStyleBackColor = true;
            // 
            // cbTocEdit
            // 
            this.cbTocEdit.AutoSize = true;
            this.cbTocEdit.Checked = true;
            this.cbTocEdit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTocEdit.Location = new System.Drawing.Point(10, 69);
            this.cbTocEdit.Name = "cbTocEdit";
            this.cbTocEdit.Size = new System.Drawing.Size(131, 17);
            this.cbTocEdit.TabIndex = 1;
            this.cbTocEdit.Tag = "toc";
            this.cbTocEdit.Text = "Edit Table of Contents";
            this.cbTocEdit.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(217, 157);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(85, 27);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbFixMargins);
            this.groupBox1.Controls.Add(this.cbSigil);
            this.groupBox1.Controls.Add(this.cbTocEdit);
            this.groupBox1.Controls.Add(this.cbReadingOrder);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(166, 121);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // cbSigil
            // 
            this.cbSigil.AutoSize = true;
            this.cbSigil.Location = new System.Drawing.Point(10, 46);
            this.cbSigil.Name = "cbSigil";
            this.cbSigil.Size = new System.Drawing.Size(78, 17);
            this.cbSigil.TabIndex = 3;
            this.cbSigil.Tag = "sigil";
            this.cbSigil.Text = "Edit In Sigil";
            this.cbSigil.UseVisualStyleBackColor = true;
            // 
            // cbReadingOrder
            // 
            this.cbReadingOrder.AutoSize = true;
            this.cbReadingOrder.Location = new System.Drawing.Point(10, 92);
            this.cbReadingOrder.Name = "cbReadingOrder";
            this.cbReadingOrder.Size = new System.Drawing.Size(116, 17);
            this.cbReadingOrder.TabIndex = 2;
            this.cbReadingOrder.Tag = "opf";
            this.cbReadingOrder.Text = "Edit Reading Order";
            this.cbReadingOrder.UseVisualStyleBackColor = true;
            // 
            // btnGo
            // 
            this.btnGo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnGo.Enabled = false;
            this.btnGo.Location = new System.Drawing.Point(126, 157);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(85, 27);
            this.btnGo.TabIndex = 0;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SettingToolStripMenuItem,
            this.about});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(314, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // SettingToolStripMenuItem
            // 
            this.SettingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sigilPathToolStripMenuItem,
            this.checkVersionToolStripMenuItem});
            this.SettingToolStripMenuItem.Name = "SettingToolStripMenuItem";
            this.SettingToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.SettingToolStripMenuItem.Text = "Settings";
            // 
            // sigilPathToolStripMenuItem
            // 
            this.sigilPathToolStripMenuItem.Name = "sigilPathToolStripMenuItem";
            this.sigilPathToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.sigilPathToolStripMenuItem.Text = "Set Sigil Path ...";
            this.sigilPathToolStripMenuItem.Click += new System.EventHandler(this.sigilPathToolStripMenuItem_Click_1);
            // 
            // checkVersionToolStripMenuItem
            // 
            this.checkVersionToolStripMenuItem.Checked = true;
            this.checkVersionToolStripMenuItem.CheckOnClick = true;
            this.checkVersionToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkVersionToolStripMenuItem.Name = "checkVersionToolStripMenuItem";
            this.checkVersionToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.checkVersionToolStripMenuItem.Text = "Check Version";
            this.checkVersionToolStripMenuItem.CheckedChanged += new System.EventHandler(this.checkVersionToolStripMenuItem1_CheckedChanged);
            // 
            // about
            // 
            this.about.Name = "about";
            this.about.Size = new System.Drawing.Size(48, 20);
            this.about.Text = "About";
            this.about.Click += new System.EventHandler(this.about_Click);
            // 
            // cbBackup
            // 
            this.cbBackup.AutoSize = true;
            this.cbBackup.Location = new System.Drawing.Point(22, 163);
            this.cbBackup.Name = "cbBackup";
            this.cbBackup.Size = new System.Drawing.Size(93, 17);
            this.cbBackup.TabIndex = 7;
            this.cbBackup.Text = "Backup File(s)";
            this.cbBackup.UseVisualStyleBackColor = true;
            this.cbBackup.CheckedChanged += new System.EventHandler(this.cbBackup_CheckedChanged);
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // frmMain
            // 
            this.AcceptButton = this.btnGo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(314, 196);
            this.Controls.Add(this.cbBackup);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnFolder);
            this.Controls.Add(this.btnFiles);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ePub Fixer";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnFiles;
        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.CheckBox cbFixMargins;
        private System.Windows.Forms.CheckBox cbTocEdit;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.CheckBox cbReadingOrder;
        private System.Windows.Forms.CheckBox cbSigil;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.CheckBox cbBackup;
        private System.Windows.Forms.ToolStripMenuItem SettingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sigilPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkVersionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem about;
        private System.Windows.Forms.ToolTip toolTip;
    }
}