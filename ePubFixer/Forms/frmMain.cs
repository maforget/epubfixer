using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ePubFixer
{
    public partial class frmMain : Form
    {

        #region Constructor
        public frmMain()
        {
            InitializeComponent();
            this.Icon = Utils.GetIcon();
            this.Text = Utils.GetTitle();
        }
        #endregion

        #region Files or Folder Button
        private void SelectFiles()
        {
            openFileDialog1.Filter = ".epub | *.epub";
            openFileDialog1.Multiselect = true;
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Variables.Filenames.Clear();
                string[] files = openFileDialog1.FileNames;
                Variables.Filenames.AddRange(files);
                RecentFiles.SaveRecentFiles();
                btnGo.Enabled = true;
            }
        }
        private void SelectFolders()
        {
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                Variables.Filenames.Clear();
                string[] files = Directory.GetFiles(folderBrowser.SelectedPath, "*.epub", SearchOption.AllDirectories);
                Variables.Filenames.AddRange(files);
                RecentFiles.SaveRecentFiles();
                btnGo.Enabled = true;
            }
        }


        private void btnFiles_Click(object sender, EventArgs e)
        {
            SelectFiles();
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            SelectFolders();
        }

        private void selectFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectFiles();
        }

        private void selectFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectFolders();
        }
        #endregion

        #region Form Events
        private void frmMain_Load(object sender, EventArgs e)
        {
            checkVersionToolStripMenuItem.Checked = Properties.Settings.Default.CheckVersion;

            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(VersionChecker));
            t.Start();

            if (Variables.Filenames.Count > 0)
            {
                btnGo.Enabled = true;
            }

            SetToolTips();

            #region Decrypt
            decryptFilesToolStripMenuItem.Visible = true;
            decryptFilesToolStripMenuItem.Enabled = true;
            #endregion

        }

        private void SetToolTips()
        {
            toolTip.SetToolTip(this.cbBackup, "Creates a Backup of the file(s) if any changes are made");
            toolTip.SetToolTip(this.cbTocEdit, "Create, Edit or Add new Chapters for your Table of Content\n" +
                                                "Change the Order or Level by Drag & Drop\n" +
                                                "Mass Rename by adding a number or Converting Number to Words\n" +
                                                "Split files on Chapters Anchor");
            toolTip.SetToolTip(this.cbSigil, "Makes a Backup of your TOC and opens Sigil\n" +
                                                "On closing it will restore your Old Table of Content.\n" +
                                                "You might want to use at the same time with the TOC Editor.\n\n"+
                                                "You can also use it with any other external program");
            toolTip.SetToolTip(this.cbFixMargins, "Changes all the left and right margins in the CSS to 0\n" +
                                    "Leaving only the body margins to 5pt (Also corrects negative ident)");
            toolTip.SetToolTip(this.cbReadingOrder, "Edit the order the page will be shown in your Book");
            toolTip.SetToolTip(this.cbCover, "Replace The File used for the Cover,\n"+
                                                "Also makes sure that the image is stretched to fit");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            UpdateFiles();
        }

        private void cbBackup_CheckedChanged(object sender, EventArgs e)
        {
            Variables.DoBackup = cbBackup.Checked;
        }

        private void about_Click(object sender, EventArgs e)
        {
            AboutBox f = new AboutBox();
            f.ShowDialog();
        }

        private void sigilPathToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.Filter = ".exe | *.exe";
            openFileDialog1.Multiselect = false;
            openFileDialog1.FileName = Variables.SigilDefaultPath;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.SigilPath = openFileDialog1.FileName;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        #region Version
        private void VersionChecker()
        {
            try
            {
                if (Properties.Settings.Default.CheckVersion)
                {
                    var version = XDocument.Load(Variables.VersionLocation).Element("information").Element("current-version");
                    Version newVersion = new Version(version.Value);

                    // get the running version
                    Version curVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                    // compare the versions
                    if (curVersion.CompareTo(newVersion) < 0)
                    {
                        // ask the user if he would like
                        // to download the new version
                        string title = "New version detected";
                        string question = "Download the new version " + newVersion.ToString() + "?";
                        if (DialogResult.Yes ==
                         MessageBox.Show(question, title,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question))
                        {
                            // navigate the default web url
                            System.Diagnostics.Process.Start(Variables.DownloadLocation);
                        }
                    }
                }

            }
            catch (Exception)
            {

            }
        }

        private void checkVersionToolStripMenuItem1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CheckVersion = checkVersionToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();

            if (Properties.Settings.Default.CheckVersion)
            {
                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(VersionChecker));
                t.Start();
            }
        }
        #endregion

        #region Update Files
        private void UpdateFiles()
        {
            if (Variables.Filenames.Count > 0)
            {
                this.Hide();
            } else
            {
                MessageBox.Show("No ePub Files Found");
            }

            Decryption.SaveDirectory = string.Empty;
            Variables.MassUpdate = false;

            for (int i = 0; i < Variables.Filenames.Count; i++)
            {
                string file = Variables.Filenames[i];
                Utils.NewFilename();
                Variables.Filename = file;
                if (!File.Exists(file))
                {
                    MessageBox.Show("The file \"" + Variables.BookName + "\" does not exists", "File does not exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                if (Decryption.IsEncrypted && !Decryption.DecryptFile())
                    continue;

                List<Document> doc = Factory.GetDocumentType(FindNeededType());
                doc.ForEach(x => x.UpdateFile());
            }
            this.Show();
        }

        private List<string> FindNeededType()
        {
            List<string> str = new List<string>();

            foreach (CheckBox cb in this.groupBox1.Controls)
            {
                if (cb.Checked)
                {
                    str.Add(cb.Tag as string);
                }
            }

            return str;
        }
        #endregion

        #region Decrypt
        private void decryptFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Decrypt = decryptFilesToolStripMenuItem.Checked ? true : false;
            Properties.Settings.Default.Save();
        }

        #region Hide Option
        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            //#if DRM
            //            switch (e.KeyCode)
            //            {
            //                case Keys.Shift:
            //                case Keys.LShiftKey:
            //                case Keys.RShiftKey:
            //                case Keys.ShiftKey:
            //                    decryptFilesToolStripMenuItem.Visible = true;
            //                    break;
            //                default:
            //                    break;
            //            }
            //#endif
        }

        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            //switch (e.KeyCode)
            //{
            //    case Keys.Shift:
            //    case Keys.LShiftKey:
            //    case Keys.RShiftKey:
            //    case Keys.ShiftKey:
            //        decryptFilesToolStripMenuItem.Visible = false;
            //        break;
            //    default:
            //        break;
            //}
        }

        private void SettingToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            //decryptFilesToolStripMenuItem.Visible = false;
        } 
        #endregion
        #endregion

        #region Recent Files
        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            recentFilesToolStripMenuItem.DropDownItems.Clear();
            List<string> LastFiles = null;

            if (Properties.Settings.Default.RecentFiles2 != null)
            {
                LastFiles = Properties.Settings.Default.RecentFiles2.LoadSettings().ToList();
                LastFiles = LastFiles.OrderByDescending(x => LastFiles.IndexOf(x)).ToList();
            }


            if (LastFiles != null)
            {
                foreach (string item in LastFiles)
                {
                    ToolStripMenuItem menuItem = new ToolStripMenuItem(item);
                    recentFilesToolStripMenuItem.DropDownItems.Add(menuItem);
                    menuItem.Click += new EventHandler(menuItem_Click);
                }
            }
        }

        void menuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = (ToolStripMenuItem)sender;

            Variables.Filenames.Clear();
            Variables.Filenames.Add(menu.Text);
            RecentFiles.SaveRecentFiles();
            btnGo.Enabled = true;
        }

        #endregion

        private void fixMarginsKoboFixToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.KoboFixMargins = fixMarginsKoboFixToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

    }
}
