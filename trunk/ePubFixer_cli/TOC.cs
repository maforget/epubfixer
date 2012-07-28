using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ePubFixer;

namespace ePubFixer_cli
{
    class TOC : TocDocument
    {
        private int playOrder;

        public TOC()
            : base()
        {
            Utils.NewFilename();
        }

        internal override void UpdateFile()
        {
            playOrder = 0;
            fileExtractStream = base.GetStreamOPF(Variables.NCXFile);
            //Get the Existing Table of COntent
            XElement xml = GetXmlElement("navMap");

            AutoCreateNewTOC();
        }

        private void AutoCreateNewTOC()
        {
            var toc = LoadFiles();
            Save(toc);
        }

        private XElement LoadFiles()
        {
            //Get Files LIst
            OpfDocument OpfDoc = new OpfDocument();
            List<string> htmlFileLIst = OpfDoc.GetFilesList("html");
            List<string> t = htmlFileLIst;

            //Load text from Chapter
            MyHtmlDocument htmlDoc = new MyHtmlDocument();
            Dictionary<string, DetectedHeaders> DetectText = htmlDoc.FindHeaderTextInFile(t);

            //Create NavDetails ENtry for Each FIles
            List<NavDetails> FilesToAdd = new List<NavDetails>();
            foreach (var item in DetectText)
            {
                string Text = String.Empty;
                Text = Variables.TextInChapters == true ? item.Value.Result.Count == 0 ? item.Key : item.Value.Result[0] : item.Key;
                FilesToAdd.Add(new NavDetails(Utils.GetId(item.Key), item.Key, Text));
            }

            List<XElement> list = new List<XElement>();

            //Converts the navdetails to xml
            foreach (var item in FilesToAdd)
            {
                XElement navPoint = ConvertToXElement(item);
                if (navPoint != null)
                {
                    list.Add(navPoint);
                }
            }

            XElement navMap = new XElement(ns + "navMap", list);

            //return the new xml file
            return navMap;
        }

        private XElement ConvertToXElement(NavDetails item)
        {
            playOrder++;

            XElement el =
            new XElement(ns + "navPoint",
            new XAttribute("id", item.navPointid),
            new XAttribute("playOrder", playOrder),
            new XElement(ns + "navLabel",
            new XElement(ns + "text", item.Text)),
            new XElement(ns + "content",
            new XAttribute("src", (item.ContentSrc))));

            return el;
        }

        private void Save(XElement xml)
        {
            NewNavMap = xml;

            if (NewNavMap == null)
                return;

            ReplaceNavMap(NewNavMap);
            WriteXML();

            base.UpdateZip(fileOutStream);

            if (Variables.CreateHtmlTOC == true)
            {
                HtmlTOCDocument doc = new HtmlTOCDocument(NewNavMap);
                doc.UpdateFile();
            }
        }
    }
}
