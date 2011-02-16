namespace ePubFixer
{
    partial class frmTocEdit
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
            this.DuplicateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shiftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shiftChapterUp = new System.Windows.Forms.ToolStripMenuItem();
            this.shiftChapterDown = new System.Windows.Forms.ToolStripMenuItem();
            contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // NewToolStripMenuItem
            // 
            this.DuplicateToolStripMenuItem.Name = "DuplicateToolStripMenuItem";
            this.DuplicateToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.DuplicateToolStripMenuItem.Text = "Duplicate";
            this.DuplicateToolStripMenuItem.Click += new System.EventHandler(DuplicateToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // shiftToolStripMenuItem
            // 
            this.shiftToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shiftChapterUp,
            this.shiftChapterDown});
            this.shiftToolStripMenuItem.Name = "shiftToolStripMenuItem";
            this.shiftToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.shiftToolStripMenuItem.Text = "Shift";
            // 
            // shiftChapterUp
            // 
            this.shiftChapterUp.Name = "shiftChapterUp";
            this.shiftChapterUp.Size = new System.Drawing.Size(308, 22);
            this.shiftChapterUp.Text = "Take Following Chapter As Source (Shift Up)";
            this.shiftChapterUp.ToolTipText = "Each selected items will take the following chapter \r\nsource and assign it to the" +
                " current items.\r\n\r\n(Ex: Chapter 41 in the TOC points to Chapter 40 in the Previe" +
                "w)";
            this.shiftChapterUp.Click += new System.EventHandler(this.shiftChapterUpToolStripMenuItem_Click);
            // 
            // shiftChapterDown
            // 
            this.shiftChapterDown.Name = "shiftChapterDown";
            this.shiftChapterDown.Size = new System.Drawing.Size(308, 22);
            this.shiftChapterDown.Text = "Take Previous Chapter As Source (Shift Down)";
            this.shiftChapterDown.ToolTipText = "Each selected items will take the previous chapter \r\nsource and assign it to the " +
                "current items.\r\n\r\n(Ex: Chapter 40 in the TOC points to Chapter 41 in the Preview" +
                ")";
            this.shiftChapterDown.Click += new System.EventHandler(this.shiftChapterDownToolStripMenuItem_Click);
            // 
            // frmTocEdit
            // 
            this.Name = "frmTocEdit";
            this.Text = "TOC Editor";
            contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                DuplicateToolStripMenuItem,
                renameToolStripMenuItem, 
                shiftToolStripMenuItem});
            contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DuplicateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shiftChapterUp;
        private System.Windows.Forms.ToolStripMenuItem shiftChapterDown;
        private System.Windows.Forms.ToolStripMenuItem shiftToolStripMenuItem;

    }
}