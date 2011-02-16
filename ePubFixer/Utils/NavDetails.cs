using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Reflection;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;
using System.Threading;
using Ionic.Zip;
using ePubFixer;
using System.Web;

namespace ePubFixer
{
    public class NavDetails
    {
        public string navPointid { get; set; }

        private string _ContentSrc;
        public string ContentSrc
        {
            get
            {
                if (Utils.GetFileList().ContainsValue(_ContentSrc))
                {
                    return _ContentSrc;
                } else
                {
                    string str = HttpUtility.UrlDecode(_ContentSrc);
                    return str;
                }
            }
            set
            {
                _ContentSrc = value;
            }
        }

        public string Text { get; set; }
        public List<string> DetectedTexts { get; set; }
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
