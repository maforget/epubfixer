using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Windows.Forms;
using System.Web;

namespace ePubFixer
{
    public class SigilDocument : Document
    {
        #region Properties & Fields
        private List<string> FileList;
        protected internal override string fileOutName { get; set; }
        protected internal override Stream fileOutStream { get; set; }
        protected internal override Stream fileExtractStream { get; set; }
        protected override string nsIfEmpty { get; set; }
        protected override XElement NewTOC { get; set; }
        protected override XElement NewNavMap { get; set; }
        protected override XElement OldTOC { get; set; }
        protected override XNamespace ns { get; set; }
        #endregion

        #region Constructor
        public SigilDocument()
            : base()
        {
            FileList = new List<string>();
        } 
        #endregion

        #region Update File
        internal override void UpdateFile()
        {
            //if (File.Exists(Variables.SigilDefaultPath))
            //{
            //    Properties.Settings.Default.SigilPath = Variables.SigilDefaultPath;
            //    Properties.Settings.Default.Save();
            //}

            if (Properties.Settings.Default.SigilPath == "")
            {
                System.Windows.Forms.MessageBox.Show("Sigil path not set!");
                return;
            }

            if (!File.Exists(Properties.Settings.Default.SigilPath))
            {
                System.Windows.Forms.MessageBox.Show("Sigil not found");
                return;
            }

            //GetBackupTOC
            string ncxFile = Variables.NCXFile;
            Stream BackupTOC = GetStream(ncxFile);

            base.SaveBackup();
            ProcessStartInfo start = new ProcessStartInfo(Properties.Settings.Default.SigilPath);
            start.Arguments = "\"" + Variables.Filename + "\"";
            start.UseShellExecute = false;

            Cursor.Current = Cursors.WaitCursor;
            Process proc = Process.Start(start);
            Cursor.Current = Cursors.Default;
            proc.WaitForExit();

            OpfDocument doc = new OpfDocument();
            FileList = doc.GetFilesList("html");

            XElement NavMap = GetXmlElement(BackupTOC, "navMap");
            RearrangeTOC(NavMap);
            if (!ReplaceNavMap(NavMap))
                return;


            WriteXML();

            //Find toc.ncx path in Zip
            fileOutName = Utils.GetFilePathInsideZip(ncxFile);

            UpdateZip(fileOutStream);

        } 
        #endregion

        #region Recreate TOC
        private bool ReplaceNavMap(XElement xElement)
        {
            if (OldTOC != null && xElement != null)
            {
                NewTOC = new XElement(OldTOC);
                NewTOC.Element(ns + "navMap").ReplaceWith(xElement);
                return true;
            }

            return false;
        }

        private void RearrangeTOC(XElement BackupTOC)
        {
            if (BackupTOC != null)
            {
                foreach (var nav in BackupTOC.Elements(ns + "navPoint"))
                {
                    XElement el = nav.Element(ns + "content");
                    string item = el == null ? string.Empty : el.Attribute("src").Value;

                    if (nav.Name.LocalName == "navPoint")
                    {
                        string[] split = item.Split('/');
                        string filename = split[split.Length - 1];

                        string[] split2 = filename.Split('#');
                        string Anchor = split2[split2.Length - 1];
                        string filenameONLY = split2[0];
                        Anchor = Anchor == filename ? string.Empty : "#" + Anchor;

                        string s = (from f in FileList
                                    where f.ToLower().EndsWith(filenameONLY.ToLower())
                                    select f).FirstOrDefault();

                        s += Anchor;

                        if (!Utils.GetFileList().ContainsValue(s))
                        {
                            string str = HttpUtility.UrlDecode(s);
                            s = str;
                        }

                        nav.Element(ns + "content").SetAttributeValue("src", s);
                        RearrangeTOC(nav);
                    }
                }
            }

        } 
        #endregion

    }
}
