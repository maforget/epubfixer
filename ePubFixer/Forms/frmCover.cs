using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ePubFixer
{
    public partial class frmCover : Form
    {
        #region Fields & Constructor
        bool BackupDone = false;
        Image Cover;
        Image BackupCover;
        Image DefaultImage;
        string DefaultURL;
        internal event EventHandler<CoverChangedArgs> CoverChanged;
        private string SourceMessage;

        public frmCover(Image CoverFile)
        {
            InitializeComponent();
            this.Icon = Utils.GetIcon();
            WindowSave.RestoreWindows(Properties.Settings.Default.frmCover, this);
            DefaultImage = CoverFile == null ? null : CoverFile;
            BackupCover = DefaultImage;
            DefaultURL = CoverFile == null ? "" : "DefaultImageStream";
            this.Text += " - " + Variables.BookName;
            ChangeImage();
            SetTooltips();
            CheckMassUpdate();
        }

        private void SetTooltips()
        {
            toolTip.SetToolTip(btnFromFolder, "Uses the cover.jpg file that is in the same directory has the Book.\n" +
                                                "Useful when fetching new covers with Calibre and you just want to\n" +
                                                "update the cover without having to Convert the file again.");
            toolTip.SetToolTip(btnReset, "Changes back to the default Cover from the book");
            toolTip.SetToolTip(btnFile, "Chooses a image file to update the Cover");
            toolTip.SetToolTip(btnSave, "Will Update the image file and make sure that the cover is stretched to fit.\n" +
                                        "Also adds the cover to the guide if it was missing");
            toolTip.SetToolTip(btnMassUpdate, "Will Update the images from all the selected book with the settings currently selected");
        }
        #endregion

        #region Form Events
        private void frmCover_Load(object sender, EventArgs e)
        {
            MassUpdate();
        }

        private void frmCover_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Variables.MassUpdate)
            {
                Properties.Settings.Default.frmCover = WindowSave.SaveWindow(this);
                Properties.Settings.Default.Save();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                ChangeImage(openDialog.FileName);
                ResizeImage();
                SourceMessage = " Using Source " + openDialog.FileName;
            }
        }

        private void btnFromFolder_Click(object sender, EventArgs e)
        {
            FromFolderSource();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            source = CoverDocument.SourceOfCover.FromBook;
            ChangeImage();
            ResizeImage();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OnCoverChanged(new CoverChangedArgs(Cover, cbPreserveRatio.Checked));
        }

        private void cbPreserveRatio_CheckedChanged(object sender, EventArgs e)
        {
            ResizeImage(false);
        }

        private void btnMassUpdate_Click(object sender, EventArgs e)
        {
            Variables.MassUpdate = true;
            CoverDocument.MassPreserveRatio = cbPreserveRatio.Checked;
            CoverDocument.MassSource = source;

            OnCoverChanged(new CoverChangedArgs(Cover, cbPreserveRatio.Checked));

            this.Close();
        }

        private void cbAspectRatio_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetAspecRatioValue(cbAspectRatio.SelectedIndex);
            ResizeImage(false);
        }
        #endregion

        #region ChangeImage
        private void GetAspecRatioValue(int i)
        {
            double[] ARlist = { 0.75, 0.5859375, 0.686667};

            if (i >= 0)
            {
                CoverDocument.ImageRatio = ARlist[i];
            } else
            {
                CoverDocument.ImageRatio = 0.75;
            }

            int pbWidth = (int)(panel1.Height * CoverDocument.ImageRatio);
            int FormWidth = 172 + pbWidth;


            //Resize Form & PictureBox
            panel1.Width = pbWidth;
            this.Width = FormWidth;


        }

        private void ResizeImage(bool Backup = true)
        {
            BackupCover = Backup ? new Bitmap(Cover) : BackupCover;
            if (Cover != null && !cbPreserveRatio.Checked)
            {
                

                double ratio = (double)Cover.Width / (double)Cover.Height;
                if (Cover.Height > CoverDocument.MaxHeight | ratio != CoverDocument.ImageRatio)
                {
                    Image img = CoverDocument.ResizeImage(Cover, CoverDocument.ImageRatio, cbPreserveRatio.Checked);
                    ChangeImage(img);
                }
                BackupDone = true;
            } else if (BackupDone)
            {
                ChangeImage(BackupCover);
                BackupDone = false;
            }
        }

        private void ChangeImage(Image image)
        {
            if (image != null)
            {
                Cover = new Bitmap(image);

                pbCover.Image = Cover;
                lblHeigth.Text = Cover.Height.ToString();
                lblWidth.Text = Cover.Width.ToString();
            }
        }

        private void ChangeImage()
        {
            string uRL = DefaultURL;
            ChangeImage(uRL);
        }

        private void ChangeImage(string URL)
        {
            if (!string.IsNullOrEmpty(URL))
            {
                Image i = URL == DefaultURL ? DefaultImage : Image.FromFile(URL);
                ChangeImage(i);
            }
        }

        CoverDocument.SourceOfCover source = CoverDocument.SourceOfCover.FromBook;
        private void FromFolderSource()
        {
            source = CoverDocument.SourceOfCover.FromFolder;
            string CoverFromFolder = string.Empty;
            string[] PossibleNames = { "cover.jpg", "cover.jpeg" };
            foreach (var item in PossibleNames)
            {
                string url = Path.Combine(Path.GetDirectoryName(Variables.Filename), item);
                if (File.Exists(url))
                {
                    CoverFromFolder = url;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(CoverFromFolder))
            {
                ChangeImage(CoverFromFolder);
                BackupCover = new Bitmap(Cover);
                ResizeImage();
                SourceMessage = " Using File cover.jpg in folder " + Path.GetDirectoryName(Variables.Filename);
            }
        }
        #endregion

        private void OnCoverChanged(CoverChangedArgs e)
        {
            if (CoverChanged != null)
            {
                CoverChanged(this, e);
                lblStatus.Text = e.ChangedCoverFile ? e.Message + SourceMessage : e.Message;

                //Update the new DefaultImage
                DefaultImage = e.Cover;
                BackupCover = e.Cover;
            }
        }

        #region Mass Update
        private void CheckMassUpdate()
        {
            if (Variables.MassUpdate)
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                //this.ShowInTaskbar = false;
                this.Size = new Size(0, 0);
            } else
            {
                cbAspectRatio.SelectedIndex = 0;
            }
        }
        private void MassUpdate()
        {
            if (Variables.MassUpdate)
            {
                using (new HourGlass())
                {
                    cbPreserveRatio.Checked = CoverDocument.MassPreserveRatio;
                    
                    if (CoverDocument.MassSource == CoverDocument.SourceOfCover.FromFolder)
                        FromFolderSource();
                    else
                        ResizeImage(false);

                    OnCoverChanged(new CoverChangedArgs(Cover, cbPreserveRatio.Checked));
                    this.Close();
                }
            }
        }
        #endregion

    }
}
