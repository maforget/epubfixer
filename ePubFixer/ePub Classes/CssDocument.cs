using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Xml.Linq;
using System.Linq;
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
        private string BodyTag = "";
        private string CSSFile = "css";
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
            //Get the Body Tag and the CSS file from a html file
            MyHtmlDocument doc = new MyHtmlDocument();
            BodyTag = doc.FindBodyStyleClass(ref CSSFile);


            fileExtractStream = base.GetStream(CSSFile);
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
            SaveOpfFixToFile();

            if (Variables.Filenames.IndexOf(Variables.Filename) == (Variables.Filenames.Count - 1)
                && Factory.NumberOfJobs == 1)
            {
                System.Windows.Forms.MessageBox.Show("Your Files are now fixed");
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
                    bool KoboFix = Properties.Settings.Default.KoboFixMargins;

                    while ((str = sr.ReadLine()) != null)
                    {
                        str = str.Trim();
                        BodyTagSeen = Regex.IsMatch(str, "\\b" + BodyTag + "\\b", RegexOptions.IgnoreCase) ? true : BodyTagSeen;

                        Predicate<string> Lines = new Predicate<string>(x => str.StartsWith(x));

                        if (Lines("margin-left:"))
                        {
                            cssOutput.Add(BodyTagSeen ? !KoboFix ? "margin-left: 5pt;" : "" : "margin-left: 0;");

                        } else if (Lines("margin-right:"))
                        {
                            cssOutput.Add(BodyTagSeen ? !KoboFix ? "margin-right: 5pt;" : "" : "margin-right: 0;");

                            BodyTagSeen = false;
                        } else if (Lines("text-indent:"))
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

                cssOutput = (from i in cssOutput
                             where !string.IsNullOrEmpty(i)
                             select i).ToList();
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
