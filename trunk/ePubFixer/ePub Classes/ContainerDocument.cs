using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Windows.Forms;

namespace ePubFixer
{
    public class ContainerDocument : Document
    {
        #region Properties & Fields
        protected internal override string fileOutName { get; set; }
        protected internal override Stream fileOutStream { get; set; }
        protected internal override Stream fileExtractStream { get; set; }
        protected override string nsIfEmpty { get; set; }
        protected override XElement NewTOC { get; set; }
        protected override XElement NewNavMap { get; set; }
        protected override XElement OldTOC { get; set; }
        protected override XNamespace ns { get; set; }
        #endregion

        public ContainerDocument()
            : base()
        {
            nsIfEmpty="urn:oasis:names:tc:opendocument:xmlns:container";
            fileExtractStream = base.GetStream("container.xml");
        }

        internal override void UpdateFile()
        {
            throw new NotImplementedException();
        }

        public string GetOPFfilename()
        {
            string ret = "";
            XElement x = base.GetXmlElement("rootfiles");
            foreach (var item in x.Elements())
            {
                if (item.Attribute("media-type").Value=="application/oebps-package+xml")
                {
                    ret = item.Attribute("full-path").Value;
                    break;
                }
            }
            return ret;

        }


    }
}
