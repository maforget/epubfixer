using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Net;

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
