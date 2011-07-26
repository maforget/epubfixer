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
    public static class Factory
    {
        private static List<Document> _Li;
        public static int NumberOfJobs
        {
            get
            {
                return _Li.Count;
            }
        }

        public static List<Document> GetDocumentType(List<string> type)
        {
            _Li = new List<Document>();

            foreach (string item in type)
            {
                switch (item.ToLower())
                {
                    case "css": _Li.Add(new CssDocument());
                        break;
                    case "toc": _Li.Add(new TocDocument());
                        break;
                    case "opf": _Li.Add(new OpfDocument());
                        break;
                    case "sigil": _Li.Add(new SigilDocument());
                        break;
                    case "cover": _Li.Add(new CoverDocument());
                        break;
                    default:
                        break;
                }
            }

            if (_Li.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("You must First Select an Option");
            }

            return _Li;

        }
    }
}
