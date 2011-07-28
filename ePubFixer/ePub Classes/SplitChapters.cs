using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using HtmlAgilityPack;
using System.Text;
using Tidy = TidyManaged;

namespace ePubFixer
{
    class SplitChapters
    {
        #region Properties & Fields
        internal Dictionary<string, string> list { get; set; }
        private bool PreviousContainedAnchor = true;
        private List<string> ZipFileNames = new List<string>();
        private Dictionary<string, string> Ids;
        private IEnumerable<string> FilesTodelete;
        private int splitNumber;
        private List<string> AnchorList;
        private bool Splitted = false;
        #endregion

        #region Constructor
        public SplitChapters()
            : base()
        {
        }

        public SplitChapters(List<string> list)
            : this()
        {
            try
            {
                this.list = list.ToDictionary(x => x);
                using (ZipFile zip = ZipFile.Read(Variables.Filename))
                {
                    ZipFileNames.AddRange(zip.EntryFileNames);
                }
            } catch (Exception)
            {

                this.list = null;
            }
        }
        #endregion

        #region Update Files
        internal void UpdateFiles()
        {
            using (new HourGlass())
            {
                if (list != null)
                {
                    SplitFiles();

                    if (Splitted)
                    {
                        DeleteOldFiles();
                        UpdateOPF();
                    }

                } else
                {
                    System.Windows.Forms.MessageBox.Show("Please make sure that they are no double entries",
                        "Splitting Cancelled",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
            }
        }

        private void UpdateOPF()
        {
            ReplaceManifest();
            ReplaceSpine();
        }
        #endregion

        #region Replace
        private void ReplaceSpine()
        {
            OpfDocument doc = new OpfDocument();
            XElement oldSpine = doc.GetXmlElement("spine");
            XNamespace ns = oldSpine.Name.Namespace;
            List<XElement> newItems = new List<XElement>();

            foreach (XElement item in oldSpine.Elements())
            {
                string id = item.Attribute("idref").Value;
                if (Ids.Values.Contains(id))
                {
                    List<string> IdsToAdd = (from i in Ids
                                             where i.Value == id
                                             select i.Key).ToList();


                    foreach (string ids in IdsToAdd)
                    {
                        XElement newEl =
                            new XElement(ns + "itemref",
                                new XAttribute("idref", ids));

                        newItems.Add(newEl);
                    }

                } else
                {
                    newItems.Add(item);
                }
            }

            IEnumerable<XAttribute> spine = oldSpine.Attributes();
            XElement newSpine = null;

            if (spine.Count() > 0)
            {
                newSpine = new XElement(ns + "spine", spine, newItems);
            } else
            {
                //There is no spine elements
                XAttribute TocRef = new XAttribute("toc", doc.GetNCXid());

                newSpine = new XElement(ns + "spine", TocRef, newItems);
            }

            doc.ReplaceSpine(newSpine);
        }

        private void ReplaceManifest()
        {
            OpfDocument doc = new OpfDocument();
            XElement oldManifest = doc.GetXmlElement("manifest");
            XNamespace ns = oldManifest.Name.Namespace;
            List<XElement> newItems = new List<XElement>();
            Ids = new Dictionary<string, string>();

            foreach (XElement item in oldManifest.Elements())
            {
                string href = Utils.VerifyFilenameEncoding(item.Attribute("href").Value);
                string id = item.Attribute("id").Value;
                string mediaType = item.Attribute("media-type").Value;
                if (FilesTodelete.Contains(href))
                {
                    List<string> filesToAdd = (from i in list
                                               where i.Key.Split('#')[0] == href
                                               select i.Value).ToList();


                    foreach (string files in filesToAdd)
                    {
                        string newID = id + "-" + filesToAdd.IndexOf(files);
                        if (!Ids.ContainsKey(newID))
                        {
                            Ids.Add(newID, id);

                            XElement newEl =
                                new XElement(ns + "item",
                                    new XAttribute("href", files),
                                    new XAttribute("id", newID),
                                    new XAttribute("media-type", mediaType));

                            newItems.Add(newEl);
                        }
                    }


                } else
                {
                    Ids.Add(id, id);
                    newItems.Add(item);
                }
            }

            XElement newManifest = new XElement(ns + "manifest", newItems);
            doc.ReplaceManifest(newManifest);
        }
        #endregion

        #region Delete Old Files
        private void DeleteOldFiles()
        {
            using (ZipFile zip = ZipFile.Read(Variables.Filename))
            {
                foreach (string file in FilesTodelete)
                {
                    ZipEntry entryToDel = zip.Where(x => x.FileName == Variables.OPFpath + file).Select(x => x).FirstOrDefault();
                    if (entryToDel != null)
                    {
                        zip.RemoveEntry(entryToDel);
                    }
                }
                zip.Save();
            }
        }
        #endregion

        #region Split Html FIles
        private void SplitFiles()
        {
            FilesTodelete = from i in list.Keys.Distinct()
                            group i by i.Split('#')[0] into g
                            where g.Count() >= 2
                            select g.Key;

            //There are no files that appears more than once.
            if (FilesTodelete.Count() == 0)
            {
                System.Windows.Forms.MessageBox.Show(
                    "The same file must appear at least 2 times in the TOC, to be Split", "No files to split",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return;
            }

            AnchorList = (from i in list
                          select i.Key).ToList();

            string splitFilename;
            splitNumber = 1;

            for (int i = 0; i < AnchorList.Count; i++)
            {
                var item = AnchorList[i];

                if (item.Contains("#"))
                {
                    string prevItem = i >= 1 && AnchorList[i - 1].Contains('#') ? AnchorList[i - 1] : string.Empty;
                    string id = item.Split('#')[1];
                    string prevId = prevItem != string.Empty ? prevItem.Split('#')[1] : string.Empty;
                    string filename = item.Split('#')[0];
                    string prevFilename = prevItem != string.Empty ? prevItem.Split('#')[0] : string.Empty;
                    string nextFilename = i < AnchorList.Count - 1 ? AnchorList[i + 1].Split('#')[0] : string.Empty;
                    string ext = Path.GetExtension(filename);

                    //File Only Appears once, No need to Split.
                    if (!FilesTodelete.Contains(filename))
                        continue;

                    splitNumber = filename == prevFilename ? splitNumber + 1 : 1;
                    prevId = filename == prevFilename ? prevId : "";
                    splitFilename = filename.Remove(filename.Length - ext.Length) + "-" + splitNumber + ext;

                    CutFiles(filename, id, prevId, splitFilename);

                    if (i > 0)
                    {
                        list[list[AnchorList[i - 1]]] = splitFilename;
                    }

                    if (filename != nextFilename && nextFilename != "" || i == AnchorList.Count - 1)
                    {
                        //Cut the Last File to the End or the on before changing filename
                        splitFilename = filename.Remove(filename.Length - ext.Length) + "-" + (splitNumber + 1) + ext;
                        CutFiles(filename, "", id, splitFilename);
                        list[list[AnchorList[i]]] = splitFilename;
                    }

                    Splitted = true;
                    PreviousContainedAnchor = true;
                } else
                {
                    PreviousContainedAnchor = false;
                }
            }

        }

        #region Get Tidy Html
        private Dictionary<string, string> HtmlCache = new Dictionary<string, string>();
        //string BodyHeading = "";
        private string GetHtml(string filename)
        {
            if (HtmlCache.ContainsKey(filename))
                return HtmlCache[filename];

            string html = "";
            MyHtmlDocument htmlDoc = new MyHtmlDocument();
            var Stream = htmlDoc.GetStreamOPF(filename);

            using (Tidy.Document doc = Tidy.Document.FromStream(Stream))
            {
                doc.ShowWarnings = false;
                doc.Quiet = true;
                doc.OutputXhtml = true;
                doc.InputCharacterEncoding = Tidy.EncodingType.Utf8;
                doc.OutputCharacterEncoding = Tidy.EncodingType.Utf8;
                doc.IndentAttributes = false;
                doc.IndentBlockElements = Tidy.AutoBool.Auto;
                doc.NewBlockLevelTags = "svg,image";
                doc.WrapAt = 0;
                doc.CleanAndRepair();
                html = doc.Save();
                HtmlCache.Add(filename, html);
            }


            return html;
        }
        private string GetHead(string html)
        {
            StringBuilder Head = new StringBuilder();

            using (StringReader sr = new StringReader(html))
            {
                string str;
                while ((str = sr.ReadLine()) != null)
                {
                    if (!str.StartsWith(@"<body"))
                    {
                        Head.AppendLine(str);
                    } else
                    {
                        break;
                    }
                }
            }

            return Head.ToString();
        }
        private List<string> GetHtmlBody(string Html)
        {
            List<string> Body = new List<string>();

            using (StringReader sr = new StringReader(Html))
            {
                bool StartBody = false;
                string str;
                while ((str = sr.ReadLine()) != null)
                {
                    if (str.StartsWith(@"<body"))
                    {
                        //BodyHeading = str;
                        StartBody = true;
                    }

                    if (StartBody)
                    {
                        Body.Add(str);
                    }
                }
            }

            return Body;
        }
        #endregion

        private void CutFiles(string filename, string id, string prevId, string splitFilename)
        {
            string html = GetHtml(filename);

            if (!String.IsNullOrEmpty(html))
            {
                List<string> File = GetHtmlBody(html);
                string Head = GetHead(html);
                List<string> ExtractedBody = new List<string>();
                id = id != "" ? "id=\"" + id : id;
                prevId = !String.IsNullOrEmpty(prevId) ? "id=\"" + prevId : prevId;

                if (String.IsNullOrEmpty(id))
                    // Just for the last file
                    ExtractedBody = File.SkipWhile(x => !x.Contains(prevId)).ToList();
                else
                    ExtractedBody = File.SkipWhile(x => !x.Contains(prevId))
                                    .TakeWhile(x => !x.Contains(id)).ToList();


                StringBuilder sb = new StringBuilder();
                sb.Append(Head);
                //sb.AppendLine(BodyHeading);
                ExtractedBody.ForEach(x => sb.AppendLine(x));
                string newHtml = sb.ToString();

                if (ExtractedBody.Count <= 2 || prevId == "" && PreviousContainedAnchor)
                {
                    splitNumber--;
                } else
                {
                    MyHtmlDocument htmlDoc = new MyHtmlDocument();
                    htmlDoc.TidyHtml(newHtml);

                    string file = ZipFileNames.Where(x => x == Variables.OPFpath + filename).Select(x => x).FirstOrDefault().Replace(filename, splitFilename);
                    htmlDoc.fileOutName = file;
                    htmlDoc.UpdateZip();
                }
            }
        }

        #endregion
    }
}
