using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ePubFixer
{
    public partial class frmPreview : Form
    {
        #region Fields
        private string RequestedAnchor;
        private string[] SplitFileName; 
        #endregion

        #region Constructor
        public frmPreview(string filename, string Chapter)
        {
            InitializeComponent();
            filename = Utils.VerifyFilenameEncoding(filename);
            file.Text = filename;
            WindowSave.RestoreWindows(Properties.Settings.Default.frmPreview, this);
            this.Icon = Utils.GetIcon();

            SplitFileName = filename.Split('#');
            if (SplitFileName.Length > 1)
            {
                RequestedAnchor = SplitFileName[1];
            }
            string CleanedFileName = SplitFileName[0];
            string Path = Utils.GetTempFilePath(CleanedFileName);

            webBrowser1.Navigate(new Uri(Path + "#" + RequestedAnchor));

            this.Text = Chapter;
        } 
        #endregion

        #region Closing (Save Position)
        private void frmPreview_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.frmPreview = WindowSave.SaveWindow(this);
            Properties.Settings.Default.Save();
        } 
        #endregion

    }
}
