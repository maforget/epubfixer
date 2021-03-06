using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Web;

namespace ePubFixer
{
    public class NavDetails
    {
        public string Text { get; set; }
        public List<string> DetectedTexts { get; set; }

        public string navPointid { get; set; }

        private string _ContentSrc;
        public string ContentSrc
        {
            get
            {
                string str = Utils.VerifyFilenameEncoding(_ContentSrc);
                return str;
            }
            set
            {
                _ContentSrc = value;
            }
        }


        private string _Anchor = "";
        public string Anchor
        {
            get
            {
                if (ContentSrc.Contains('#'))
                {
                    _Anchor = ContentSrc.Split('#')[1];
                }
                return _Anchor;
            }
        }

        public string File
        {
            get
            {
                return ContentSrc.Split('#')[0];
            }
        }

        public NavDetails(string id, string src)
        {
            navPointid = id;
            ContentSrc = src;
        }

        public NavDetails(string id, string src, string Text)
            : this(id, src)
        {
            this.Text = Text;
        }

        public NavDetails(string id, string src, List<string> Text)
            : this(id, src, GetEmptyText(Text))
        {
            this.DetectedTexts = Text;
        }

        private static string GetEmptyText(List<string> Text)
        {
            if (Text == null)
            {
                return "";
            } else
            {
                return Text.FirstOrDefault();
            }
        }

        public void ShiftChapter(List<string> CleanedFileName, int newSourceIndex)
        {
            string newSource = CleanedFileName[newSourceIndex];
            string src = newSource.Split('#')[0];

            if (File == src && newSource.Contains('#'))
            {
                src = newSource;
            }

            ContentSrc = src;
        }

    }
}
