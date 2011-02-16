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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMassRename));
            this.txtInput = new System.Windows.Forms.TextBox();
            this.cbConvertToWords = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbUpperCase = new System.Windows.Forms.CheckBox();
            this.cbRoman = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(12, 150);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(323, 20);
            this.txtInput.TabIndex = 0;
            // 
            // cbConvertToWords
            // 
            this.cbConvertToWords.AutoSize = true;
            this.cbConvertToWords.Location = new System.Drawing.Point(12, 176);
            this.cbConvertToWords.Name = "cbConvertToWords";
            this.cbConvertToWords.Size = new System.Drawing.Size(109, 17);
            this.cbConvertToWords.TabIndex = 1;
            this.cbConvertToWords.Text = "Number to Words";
            this.cbConvertToWords.UseVisualStyleBackColor = true;
            this.cbConvertToWords.CheckedChanged += new System.EventHandler(this.cbConvertToWords_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(176, 199);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(78, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Ok";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(260, 199);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(326, 138);
            this.label1.TabIndex = 4;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // cbUpperCase
            // 
            this.cbUpperCase.AutoSize = true;
            this.cbUpperCase.Enabled = false;
            this.cbUpperCase.Location = new System.Drawing.Point(12, 199);
            this.cbUpperCase.Name = "cbUpperCase";
            this.cbUpperCase.Size = new System.Drawing.Size(135, 17);
            this.cbUpperCase.TabIndex = 5;
            this.cbUpperCase.Text = "Words Are Upper Case";
            this.cbUpperCase.UseVisualStyleBackColor = true;
            // 
            // cbRoman
            // 
            this.cbRoman.AutoSize = true;
            this.cbRoman.Location = new System.Drawing.Point(176, 176);
            this.cbRoman.Name = "cbRoman";
            this.cbRoman.Size = new System.Drawing.Size(159, 17);
            this.cbRoman.TabIndex = 0;
            this.cbRoman.Text = "Number to Roman Numerals";
            this.cbRoman.UseVisualStyleBackColor = true;
            this.cbRoman.CheckedChanged += new System.EventHandler(this.cbRoman_CheckedChanged);
            // 
            // frmMassRename
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(347, 232);
            this.Controls.Add(this.cbRoman);
            this.Controls.Add(this.cbUpperCase);
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbUpperCase;
        private System.Windows.Forms.CheckBox cbRoman;

    }
}