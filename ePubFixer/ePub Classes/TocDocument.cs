using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Xml;

namespace ePubFixer
{
    public class TocDocument : Document
    {
        #region Properties
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
        public TocDocument()
            : base()
        {
            fileExtractStream = base.GetStreamOPF(Variables.NCXFile);
            nsIfEmpty = "\"http://www.daisy.org/z3986/2005/ncx/\" version=\"2005-1\" xml:lang=\"en\"";
        }
        #endregion

        #region Update Files
        internal override void UpdateFile()
        {
            fileExtractStream = base.GetStreamOPF(Variables.NCXFile);
            XElement xml = GetXmlElement("navMap");

#if !CLI
            using (frmTocEdit frm = new frmTocEdit(xml))
            {
                //frmTocEdit frm = new frmTocEdit(xml);
                frm.Save += new EventHandler<ExportTocEventArgs>(frm_Save);
                frm.ShowDialog();
            } 
#endif
        }

        #region Save
        private void Save(XElement xml)
        {
            NewNavMap = xml;

            if (NewNavMap == null)
                return;

            ReplaceNavMap(NewNavMap);
            WriteXML();

            base.UpdateZip(fileOutStream);

        }

        void frm_Save(object sender, ExportTocEventArgs e)
        {
            Save(e.XML);
            if (e.CreateHtmlTOC)
            {
                HtmlTOCDocument doc = new HtmlTOCDocument(NewNavMap);
                doc.UpdateFile();
            }

            e.Message = SaveMessage;

        }
        #endregion

        #endregion

        #region Replace
        internal XElement ReplaceNavMap(XElement newNavMap)
        {
            OpfDocument doc = new OpfDocument();

            //TOC File does not exist or is not Present in manifest
            if (fileExtractStream == null || fileExtractStream.Length == 0 || string.IsNullOrEmpty(Variables.NCXFile))
            {
                //System.Windows.Forms.MessageBox.Show("No TOC file Present", Variables.BookName);
                //return;
                fileOutName = "toc.ncx";
                doc.AddTocEntry();
            }


            if (OldTOC != null)
            {
                NewTOC = new XElement(OldTOC);
                NewTOC.Element(ns + "navMap").ReplaceWith(newNavMap);
                SaveOpfFixToFile();
            } else
            {
                //Create a new TOC
                NewTOC =
                new XElement(ns + "ncx",
                    new XElement(ns + "head", ""),
                    new XElement(ns + "docTitle",
                        new XElement(ns + "text", Variables.BookName)),
                    new XElement(newNavMap));
            }
            return NewTOC;
        }
        #endregion


    }
}
