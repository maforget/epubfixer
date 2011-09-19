using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using HtmlAgilityPack;

namespace ePubFixer
{
    public class HtmlTOCDocument : Document
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

        #region Fields
        bool CreateTOCNeeded = false;
        #endregion

        public HtmlTOCDocument(XElement tocXML)
            : base()
        {
            NewNavMap = new XElement(tocXML);
            fileOutName = GetTOCRef();//Get existing Toc name or default one if not found
            ns = tocXML.Name.Namespace;
        }

        private string GetTOCRef()
        {
            OpfDocument doc = new OpfDocument();
            string tocRef = doc.GetHtmlTOCRef();
            tocRef = string.IsNullOrEmpty(tocRef) ? string.Empty : Zip.GetFilePathInsideZip(tocRef);

            if (string.IsNullOrEmpty(tocRef))
            {
                CreateTOCNeeded = true;
                return Path.Combine(Variables.OPFpath, "content.html");
            }

            return tocRef;
        }

        internal override void UpdateFile()
        {
            string text = CreateHtmlTOC();

            if (!string.IsNullOrEmpty(text))
            {
                fileOutStream = text.ToStream();
                UpdateZip();

                if (CreateTOCNeeded)
                {
                    //We need to add the new file inside the spine and manifest and guide
                    OpfDocument doc = new OpfDocument();
                    string file = fileOutName.Replace(Variables.OPFpath, "");
                    doc.AddTOCContentRef(file);
                    doc.AddHtmlFile(file);//Will also add the spine option
                    Utils.NewFilename();
                }
            }

        }

        #region Create TOC
        private string CreateHtmlTOC()
        {
            HtmlNode Title = Tocdocument.CreateElement("h1");
            HtmlNode titleAppendChild = Title.AppendChild(HtmlTextNode.CreateNode("Table of Content"));
            Tocdocument.DocumentNode.AppendChild(Title);

            foreach (XElement item in NewNavMap.Elements())
            {
                indent = 0;

                HtmlNode itemNode = CreateHtmlTOC(item);
                if (itemNode!=null)
                {
                    Tocdocument.DocumentNode.AppendChild(itemNode); 
                }
            }

            MyHtmlDocument doc = new MyHtmlDocument();
            string text = doc.TidyHtml(Tocdocument.DocumentNode.OuterHtml);

            return text;
        }

        HtmlDocument Tocdocument = new HtmlDocument();
        int indent = 0;
        private HtmlNode CreateHtmlTOC(XElement toc)
        {
            TOCElements ele = new TOCElements(toc);
            string source = fileOutName.Contains(Variables.OPFpath) ? "../" + ele.Source : ele.Source;

            HtmlNode TopNode = Tocdocument.CreateElement("p");
            string style = "margin:0; padding: 0;" + "text-indent:" + indent.ToString() + "pt;";
            TopNode.Attributes.Add("style", style);

            HtmlNode node = Tocdocument.CreateElement("a");
            node.Attributes.Add("href", source);

            HtmlNode text = HtmlTextNode.CreateNode(ele.Text);
            if (text == null)
                return null;

            node.AppendChild(text);
            TopNode.AppendChild(node);

            if (toc.Descendants(ns + "navPoint").Count() > 0)
            {
                indent = indent + 15;

                foreach (XElement item in toc.Descendants(ns + "navPoint"))
                {
                    HtmlNode itemNode = CreateHtmlTOC(item);
                    if (itemNode!=null)
                    {
                        TopNode.AppendChild(itemNode); 
                    }
                }
            }

            return TopNode;
        } 
        #endregion
    }

    class TOCElements
    {
        public string Source { get; private set; }
        public string Text { get; private set; }
        XNamespace ns;

        public TOCElements(XElement navMapElement)
        {
            if (navMapElement != null)
            {
                ns = navMapElement.Name.Namespace;
                if (navMapElement.Name.LocalName == "navPoint")
                {
                    Text = navMapElement.Element(ns + "navLabel").Element(ns + "text").Value.Trim();
                    Source = navMapElement.Element(ns + "content").Attribute("src").Value.Trim();
                }
            }
        }
    }
}
