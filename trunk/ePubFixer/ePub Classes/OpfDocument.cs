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
    public class OpfDocument : Document
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
        public OpfDocument()
            : base()
        {
            //fileOutStream = new MemoryStream();
            nsIfEmpty = "\"http://www.idpf.org/2007/opf\"";

            OldTOC = null;
            SetFile();
            //return;
        }
        #endregion

        #region Update
        internal override void UpdateFile()
        {
            SetFile();
            XElement xml = GetXmlElement("spine");
            //if (xml == null)
            //    return false;

            frmSpineEdit frm = new frmSpineEdit(xml);
            frm.Save += new EventHandler<ExportTocEventArgs>(frm_Save);
            frm.ShowDialog();
        }

        private void frm_Save(object sender, ExportTocEventArgs e)
        {
            e.Message = Save(e.XML);
        }

        private string Save(XElement xml)
        {
            NewNavMap = xml;

            if (NewNavMap == null)
                return string.Empty;

            ReplaceSpine(NewNavMap);

            return base.UpdateZip(fileOutStream);
        }


        #endregion

        #region Extract OPF File
        private bool SetFile()
        {
            fileExtractStream = base.GetStream(Variables.OPFfile);
            try
            {
                string Text = ByteToString(fileExtractStream);

                if (String.IsNullOrEmpty(Text))
                {
                    System.Windows.Forms.MessageBox.Show("Content File is Empty", Variables.BookName);
                    return false;
                }

                Text = Text.Replace("?xml version=\"1.1\"", "?xml version=\"1.0\"");

                OldTOC = XElement.Parse(Text.Trim());
                ns = OldTOC.Name.Namespace;
                return true;
            }
            catch (Exception ex)
            {

                System.Windows.Forms.MessageBox.Show("Invalid Content File\n" + ex.Message, Variables.BookName);
                return false;
            }
        }
        #endregion

        #region Create TOC Entry in OPF File
        internal void CreateTocEntry()
        {
            List<string> fileList = GetFilesList("application/x-dtbncx+xml");

            if (fileList.Count == 0)
            {
                XElement tocElement = new XElement(ns + "item",
                                        new XAttribute("href", "toc.ncx"),
                                        new XAttribute("media-type", "application/x-dtbncx+xml"),
                                        new XAttribute("id", "toc.ncx"));
                if (OldTOC != null)
                {
                    NewTOC = new XElement(OldTOC);
                    NewTOC.Element(ns + "manifest").Add(tocElement);
                    base.WriteXML();
                    base.UpdateZip(fileOutStream);

                } else
                {
                    return;
                }

            }
        }
        #endregion

        #region Replace
        internal XElement ReplaceSpine(XElement newSpine)
        {
            if (OldTOC != null)
            {
                NewTOC = new XElement(OldTOC);
                NewTOC.Element(ns + "spine").ReplaceWith(newSpine);
                base.WriteXML();

            } else
            {
                return null;
            }
            return NewTOC;
        }

        internal XElement ReplaceManifest(XElement newManifest)
        {
            if (OldTOC != null)
            {
                NewTOC = new XElement(OldTOC);
                NewTOC.Element(ns + "manifest").ReplaceWith(newManifest);
                base.WriteXML();
                base.UpdateZip(fileOutStream);
            } else
            {
                return null;
            }
            return NewTOC;
        }
        #endregion

        #region GetFilesList

        private XElement GetManifest()
        {
            XElement ret = new XElement(OldTOC.Element(ns + "manifest"));
            return ret;
        }

        private List<string> GetFilesList(Predicate<string> filter)
        {
            try
            {
                var ids = (GetXmlElement("spine").Elements(ns + "itemref").Attributes("idref"));
                Dictionary<XAttribute, int> dict = new Dictionary<XAttribute, int>();
                foreach (XAttribute item in ids)
                {
                    dict.Add(item, dict.Count + 1);
                }

                var ret = (from i in GetManifest().Elements(ns + "item")
                           join j in dict on i.Attribute("id").Value equals j.Key.Value into g
                           from k in g.DefaultIfEmpty(new KeyValuePair<XAttribute, int>(i.Attribute("id"), dict.Last().Value + 1))
                           where filter.Invoke(i.Attribute("media-type").Value.ToLower()) || filter.Invoke(i.Attribute("href").Value.ToLower())
                           orderby k.Value
                           select i.Attribute("href").Value).ToList();

                return ret;
            }
            catch (Exception)
            {
                return null;
            }


        }

        internal Dictionary<string, string> GetFilesList()
        {
            try
            {
                Dictionary<string, string> ret = (from i in GetManifest().Elements(ns + "item")
                                                  select i).ToDictionary(x => x.Attribute("id").Value, x => x.Attribute("href").Value);

                return ret;
            }
            catch (Exception)
            {

                return null;
            }
        }

        internal List<string> GetFilesList(string filter)
        {
            return GetFilesList(x => x.ToLower().Contains(filter.ToLower()) || x.ToLower().EndsWith(filter.ToLower()));
        }

        public string GetNCXfilename()
        {
            XElement spine = GetXmlElement("spine");
            XAttribute att = spine != null ? spine.Attribute("toc") : null;
            string nCXfile = "ncx";
            if (att != null)
            {
                string id = att.Value;
                nCXfile = Utils.GetSrc(id);
            } else
            {
                System.Windows.Forms.MessageBox.Show("There is no reference to your TOC in the \"spine\"\n" +
                    "Please open the reading order editor and save to fix the problem.\n" +
                    "If you do not your TOC might not be accesible in Calibre and/or Reader/Software", "TOC reference missing",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                //No reference of TOC in spine look for mime-type
                var list = GetFilesList("application/x-dtbncx+xml");
                string mime = list != null ? list.FirstOrDefault() : "";

                if (!String.IsNullOrEmpty(mime))
                    nCXfile = mime;

                //If all else fails uses the ncx extension to find the file
            }
            return nCXfile;
        }

        public string GetNCXid()
        {
            string mime = GetFilesList("application/x-dtbncx+xml").FirstOrDefault();

            if (!String.IsNullOrEmpty(mime))
            {
                return Utils.GetId(mime);
            }

            return "";
        }
        #endregion
    }

}

