namespace ePubFixer
{
    partial class frmCover
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
            this.pbCover = new System.Windows.Forms.PictureBox();
            this.openDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnFile = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnFromBook = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblWidth = new System.Windows.Forms.Label();
            this.lblHeigth = new System.Windows.Forms.Label();
            this.btnFromFolder = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnMassUpdate = new System.Windows.Forms.Button();
            this.cbAspectRatio = new System.Windows.Forms.ComboBox();
            this.lblScreenRes = new System.Windows.Forms.Label();
            this.cbPreserveRatio = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbCover)).BeginInit();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbCover
            // 
            this.pbCover.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbCover.Location = new System.Drawing.Point(0, 0);
            this.pbCover.Name = "pbCover";
            this.pbCover.Size = new System.Drawing.Size(500, 668);
            this.pbCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCover.TabIndex = 0;
            this.pbCover.TabStop = false;
            // 
            // openDialog
            // 
            this.openDialog.Filter = "Image Files | *.jpg;*.gif;*.png;*.jpeg;*.bmp";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(28, 649);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(28, 620);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(28, 12);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(88, 23);
            this.btnFile.TabIndex = 4;
            this.btnFile.Text = "Select File";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pbCover);
            this.panel1.Location = new System.Drawing.Point(142, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(502, 670);
            this.panel1.TabIndex = 7;
            // 
            // btnFromBook
            // 
            this.btnFromBook.Location = new System.Drawing.Point(28, 41);
            this.btnFromBook.Name = "btnFromBook";
            this.btnFromBook.Size = new System.Drawing.Size(88, 23);
            this.btnFromBook.TabIndex = 3;
            this.btnFromBook.Text = "From Book";
            this.btnFromBook.UseVisualStyleBackColor = true;
            this.btnFromBook.Click += new System.EventHandler(this.btnFromBook_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Width :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Heigth :";
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(68, 148);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(0, 13);
            this.lblWidth.TabIndex = 11;
            // 
            // lblHeigth
            // 
            this.lblHeigth.AutoSize = true;
            this.lblHeigth.Location = new System.Drawing.Point(68, 170);
            this.lblHeigth.Name = "lblHeigth";
            this.lblHeigth.Size = new System.Drawing.Size(0, 13);
            this.lblHeigth.TabIndex = 12;
            // 
            // btnFromFolder
            // 
            this.btnFromFolder.Location = new System.Drawing.Point(28, 70);
            this.btnFromFolder.Name = "btnFromFolder";
            this.btnFromFolder.Size = new System.Drawing.Size(88, 23);
            this.btnFromFolder.TabIndex = 2;
            this.btnFromFolder.Text = "From Folder";
            this.btnFromFolder.UseVisualStyleBackColor = true;
            this.btnFromFolder.Click += new System.EventHandler(this.btnFromFolder_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 696);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(668, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.lblStatus.Size = new System.Drawing.Size(653, 17);
            this.lblStatus.Spring = true;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 8000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            // 
            // btnMassUpdate
            // 
            this.btnMassUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMassUpdate.Location = new System.Drawing.Point(28, 591);
            this.btnMassUpdate.Name = "btnMassUpdate";
            this.btnMassUpdate.Size = new System.Drawing.Size(88, 23);
            this.btnMassUpdate.TabIndex = 17;
            this.btnMassUpdate.Text = "Update All Files";
            this.btnMassUpdate.UseVisualStyleBackColor = true;
            this.btnMassUpdate.Click += new System.EventHandler(this.btnMassUpdate_Click);
            // 
            // cbAspectRatio
            // 
            this.cbAspectRatio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAspectRatio.DropDownWidth = 200;
            this.cbAspectRatio.FormattingEnabled = true;
            this.cbAspectRatio.Items.AddRange(new object[] {
            "600 x 800 - 6\" screens (3 : 4)",
            "600 x 1024 - 7\" screens (75 : 128)",
            "824 x 1200 (103 : 150)",
            "Original (Don\'t Resize)"});
            this.cbAspectRatio.Location = new System.Drawing.Point(12, 291);
            this.cbAspectRatio.Name = "cbAspectRatio";
            this.cbAspectRatio.Size = new System.Drawing.Size(118, 21);
            this.cbAspectRatio.TabIndex = 18;
            this.cbAspectRatio.SelectedIndexChanged += new System.EventHandler(this.cbAspectRatio_SelectedIndexChanged);
            // 
            // lblScreenRes
            // 
            this.lblScreenRes.Location = new System.Drawing.Point(9, 259);
            this.lblScreenRes.Name = "lblScreenRes";
            this.lblScreenRes.Size = new System.Drawing.Size(119, 29);
            this.lblScreenRes.TabIndex = 19;
            this.lblScreenRes.Text = "Screen Size (only for Ratio Calculation)";
            // 
            // cbPreserveRatio
            // 
            this.cbPreserveRatio.Checked = global::ePubFixer.Properties.Settings.Default.CoverKeepAspectRatio;
            this.cbPreserveRatio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPreserveRatio.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ePubFixer.Properties.Settings.Default, "CoverKeepAspectRatio", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbPreserveRatio.Location = new System.Drawing.Point(12, 205);
            this.cbPreserveRatio.Name = "cbPreserveRatio";
            this.cbPreserveRatio.Size = new System.Drawing.Size(119, 35);
            this.cbPreserveRatio.TabIndex = 15;
            this.cbPreserveRatio.Text = "Preserve Aspect Ratio";
            this.cbPreserveRatio.UseVisualStyleBackColor = true;
            this.cbPreserveRatio.CheckedChanged += new System.EventHandler(this.cbPreserveRatio_CheckedChanged);
            // 
            // frmCover
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(668, 718);
            this.Controls.Add(this.lblScreenRes);
            this.Controls.Add(this.cbAspectRatio);
            this.Controls.Add(this.btnMassUpdate);
            this.Controls.Add(this.cbPreserveRatio);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnFromFolder);
            this.Controls.Add(this.lblHeigth);
            this.Controls.Add(this.lblWidth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnFromBook);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmCover";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cover Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCover_FormClosing);
            this.Load += new System.EventHandler(this.frmCover_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbCover)).EndInit();
            this.panel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbCover;
        private System.Windows.Forms.OpenFileDialog openDialog;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnFromBook;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.Label lblHeigth;
        private System.Windows.Forms.Button btnFromFolder;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox cbPreserveRatio;
        private System.Windows.Forms.Button btnMassUpdate;
        private System.Windows.Forms.ComboBox cbAspectRatio;
        private System.Windows.Forms.Label lblScreenRes;
    }
}