using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Xml.Linq;

using System.Text.RegularExpressions;

namespace ePubFixer
{
    public class CssDocument : Document
    {
        #region Properties & Fields
        List<string> cssOutput = new List<string>();
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
        public CssDocument()
            : base()
        {
            //fileOutStream = new MemoryStream();
        } 
        #endregion

        #region Update File
        internal override void UpdateFile()
        {
            fileExtractStream = base.GetStream("css");
            if (fileExtractStream == null)
            {
                System.Windows.Forms.MessageBox.Show("No Stylesheet present", Variables.BookName);
                return;
            }

            this.ReadCSS();
            if (cssOutput.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Stylesheet is empty", Variables.BookName);
                return;
            }

            this.WriteCSS();
            base.UpdateZip(fileOutStream);


            if (Variables.Filenames.IndexOf(Variables.Filename) == (Variables.Filenames.Count - 1)
                && Factory.NumberOfJobs == 1)
            {
                System.Windows.MessageBox.Show("Your Files are now fixed");
            }
        } 
        #endregion

        #region Read Css
        private void ReadCSS()
        {
            if (fileExtractStream.Length > 0)
            {
                bool BodyTagSeen = false;

                using (StringReader sr = new StringReader(ByteToString(fileExtractStream)))
                {
                    string str;
                    MyHtmlDocument doc = new MyHtmlDocument();
                    string body = doc.FindBodyStyleClass();
                    while ((str = sr.ReadLine()) != null)
                    {
                        str = str.Trim();
                        BodyTagSeen = Regex.IsMatch(str, "\\b" + body + "\\b", RegexOptions.IgnoreCase) ? true : BodyTagSeen;

                        if (str.StartsWith("margin-left:"))
                        {
                            cssOutput.Add(BodyTagSeen ? "margin-left: 5pt;" : "margin-left: 0;");

                        } else if (str.StartsWith("margin-right:"))
                        {
                            cssOutput.Add(BodyTagSeen ? "margin-right: 5pt;" : "margin-right: 0;");
                            BodyTagSeen = false;
                        } else if (str.StartsWith("text-indent:"))
                        {
                            string s = str.Replace("text-indent:", "").Trim();
                            s = s.Replace("pt", "");
                            s = s.Replace("em", "");

                            int indet;
                            int.TryParse(s, out indet);

                            cssOutput.Add(indet < 0 ? "text-indent: 0" : str);

                            BodyTagSeen = false;
                        } else
                        {
                            cssOutput.Add(str);
                        }
                    }

                }
            }
        } 
        #endregion

        #region Write Css
        private void WriteCSS()
        {
            if (cssOutput.Count > 0)
            {
                StreamWriter sw = new StreamWriter(fileOutStream);


                foreach (string lines in cssOutput)
                {
                    sw.WriteLine(lines);
                }


                sw.Flush();
                fileOutStream.Flush();
                //Reset Stream
                fileOutStream.Seek(0, SeekOrigin.Begin);
            }
        } 
        #endregion

    }
}
