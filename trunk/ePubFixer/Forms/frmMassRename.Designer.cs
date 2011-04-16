namespace ePubFixer
{
    partial class frmMassRename
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
            this.txtInput = new System.Windows.Forms.TextBox();
            this.cbConvertToWords = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbUpperCase = new System.Windows.Forms.CheckBox();
            this.cbRoman = new System.Windows.Forms.CheckBox();
            this.cbTitleCase = new System.Windows.Forms.CheckBox();
            this.btnShowHelp = new System.Windows.Forms.Button();
            this.tipHelp = new System.Windows.Forms.ToolTip(this.components);
            this.LabelHelp = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(12, 12);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(323, 20);
            this.txtInput.TabIndex = 0;
            // 
            // cbConvertToWords
            // 
            this.cbConvertToWords.AutoSize = true;
            this.cbConvertToWords.Location = new System.Drawing.Point(12, 38);
            this.cbConvertToWords.Name = "cbConvertToWords";
            this.cbConvertToWords.Size = new System.Drawing.Size(109, 17);
            this.cbConvertToWords.TabIndex = 1;
            this.cbConvertToWords.Text = "Number to Words";
            this.cbConvertToWords.UseVisualStyleBackColor = true;
            this.cbConvertToWords.CheckedChanged += new System.EventHandler(this.cbConvertToWords_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(176, 82);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(78, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Ok";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(260, 82);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbUpperCase
            // 
            this.cbUpperCase.AutoSize = true;
            this.cbUpperCase.Location = new System.Drawing.Point(12, 61);
            this.cbUpperCase.Name = "cbUpperCase";
            this.cbUpperCase.Size = new System.Drawing.Size(138, 17);
            this.cbUpperCase.TabIndex = 5;
            this.cbUpperCase.Text = "Convert To Upper Case";
            this.cbUpperCase.UseVisualStyleBackColor = true;
            this.cbUpperCase.CheckedChanged += new System.EventHandler(this.cbUpperCase_CheckedChanged);
            // 
            // cbRoman
            // 
            this.cbRoman.AutoSize = true;
            this.cbRoman.Location = new System.Drawing.Point(176, 38);
            this.cbRoman.Name = "cbRoman";
            this.cbRoman.Size = new System.Drawing.Size(159, 17);
            this.cbRoman.TabIndex = 0;
            this.cbRoman.Text = "Number to Roman Numerals";
            this.cbRoman.UseVisualStyleBackColor = true;
            this.cbRoman.CheckedChanged += new System.EventHandler(this.cbRoman_CheckedChanged);
            // 
            // cbTitleCase
            // 
            this.cbTitleCase.AutoSize = true;
            this.cbTitleCase.Location = new System.Drawing.Point(176, 59);
            this.cbTitleCase.Name = "cbTitleCase";
            this.cbTitleCase.Size = new System.Drawing.Size(129, 17);
            this.cbTitleCase.TabIndex = 6;
            this.cbTitleCase.Text = "Convert To Title Case";
            this.cbTitleCase.UseVisualStyleBackColor = true;
            this.cbTitleCase.CheckedChanged += new System.EventHandler(this.cbTitleCase_CheckedChanged);
            // 
            // btnShowHelp
            // 
            this.btnShowHelp.Image = global::ePubFixer.Properties.Resources.helpToolStripButton_Image;
            this.btnShowHelp.Location = new System.Drawing.Point(12, 84);
            this.btnShowHelp.Name = "btnShowHelp";
            this.btnShowHelp.Size = new System.Drawing.Size(24, 24);
            this.btnShowHelp.TabIndex = 7;
            this.btnShowHelp.UseVisualStyleBackColor = true;
            this.btnShowHelp.Click += new System.EventHandler(this.btnShowHelp_Click);
            // 
            // tipHelp
            // 
            this.tipHelp.AutomaticDelay = 1000;
            this.tipHelp.AutoPopDelay = 50000000;
            this.tipHelp.InitialDelay = 0;
            this.tipHelp.ReshowDelay = 200;
            this.tipHelp.ShowAlways = true;
            this.tipHelp.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.tipHelp.ToolTipTitle = "Help with Options";
            // 
            // LabelHelp
            // 
            this.LabelHelp.AutoSize = true;
            this.LabelHelp.Location = new System.Drawing.Point(42, 95);
            this.LabelHelp.Name = "LabelHelp";
            this.LabelHelp.Size = new System.Drawing.Size(0, 13);
            this.LabelHelp.TabIndex = 8;
            // 
            // frmMassRename
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(347, 117);
            this.Controls.Add(this.LabelHelp);
            this.Controls.Add(this.btnShowHelp);
            this.Controls.Add(this.cbTitleCase);
            this.Controls.Add(this.cbRoman);
            this.Controls.Add(this.cbUpperCase);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbConvertToWords);
            this.Controls.Add(this.txtInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMassRename";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rename";
            this.Activated += new System.EventHandler(this.frmMassRename_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.CheckBox cbConvertToWords;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox cbUpperCase;
        private System.Windows.Forms.CheckBox cbRoman;
        private System.Windows.Forms.CheckBox cbTitleCase;
        private System.Windows.Forms.Button btnShowHelp;
        private System.Windows.Forms.ToolTip tipHelp;
        private System.Windows.Forms.Label LabelHelp;

    }
}