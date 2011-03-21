using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using HtmlAgilityPack;
using Ionic.Zip;
using System.Text;


namespace ePubFixer
{
    class MyHtmlDocument : Document
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
        private List<string> AnchorsInFile;
        #endregion

        #region Constructor
        public MyHtmlDocument()
            : base()
        {
            //fileOutStream = new MemoryStream();
        }
        #endregion

        internal override void UpdateFile()
        {
            throw new NotImplementedException();
        }

        #region Find Style Class for Body
        public string FindBodyStyleClass(ref string CSSfile)
        {
            OpfDocument doc = new OpfDocument();
            List<string> filelist = doc.GetFilesList("html");

            string file = filelist[filelist.Count / 2];

            HtmlDocument html = GetHtml(file);

            HtmlNode BodyNode = html.DocumentNode.SelectSingleNode("//body//@class");
            HtmlNode CSSNode = html.DocumentNode.SelectSingleNode("//head/link[@type='text/css']");
            
            //Select CSS file
            if (CSSNode!=null)
            {
                CSSfile = CSSNode.GetAttributeValue("href", "css");
            }

            string tag = BodyNode.GetAttributeValue("class", "");
            return tag;

        }
        #endregion

        #region Detect text
        /// <summary>
        /// Returns The First Line of Text(Value) for each File(Key)
        /// </summary>
        /// <param name="filenames">List of Filenames to Check</param>
        /// <returns>Returns The First Line of Text(Value) for each File(Key)</returns>
        public Dictionary<string, List<string>> FindHeaderTextInFile(List<string> filenames)
        {
            Dictionary<string, List<string>> Result = new Dictionary<string, List<string>>();
            foreach (string path in filenames)
            {
                if (Variables.HeaderTextInFile.ContainsKey(path))
                {
                    if (!Result.ContainsKey(path))
                    {
                        var clean = (from f in Variables.HeaderTextInFile[path]
                                     select HttpUtility.HtmlDecode(f)).ToList();

                        Result.Add(path, clean);
                    }
                } else
                {
                    HtmlDocument html = GetHtml(path);
                    if (html != null)
                    {
                        List<string> Found = GetText(html.DocumentNode.SelectSingleNode("//body"));

                        Result.Add(path, Found);
                        Variables.HeaderTextInFile.Add(path, Found);
                    }
                }

            }

            //Variables.HeaderTextInFile = Result;
            return Result;
        }

        /// <summary>
        /// Returns The First Line of Text(Value) after each Anchor(Key)
        /// </summary>
        /// <param name="path">Path of The file</param>
        /// <param name="Anchors">List of Anchors to Check</param>
        /// <returns>Returns The First Line of Text(Value) after each Anchor(Key)</returns>
        public Dictionary<string, List<string>> FindAchorTextInFile(string path, List<string> Anchors)
        {
            Dictionary<string, List<string>> Result = new Dictionary<string, List<string>>();
            HtmlDocument html = null;

            foreach (string id in Anchors)
            {
                string key = path + "#" + id;
                if (Variables.AnchorTextInFile.ContainsKey(key))
                {
                    if (!Result.ContainsKey(key))
                    {
                        var clean = (from f in Variables.AnchorTextInFile[key]
                                     select HttpUtility.HtmlDecode(f)).ToList();

                        Result.Add(key, clean);
                    }
                } else
                {
                    if (html == null)
                    {
                        html = GetHtml(path);
                        break;
                    }
                }
            }

            if (html != null)
            {
                var body = html.DocumentNode.SelectNodes("//body//*");
                foreach (string id in Anchors)
                {
                    string key = path + "#" + id;
                    if (Variables.AnchorTextInFile.ContainsKey(key))
                    {
                        if (!Result.ContainsKey(key))
                        {
                            Result.Add(key, Variables.AnchorTextInFile[key]);
                        }
                    } else
                    {
                        #region New Method (Slower)
                        //var t = body.SkipWhile(x => x.Id != id)
                        //    .Where(x => x.InnerText.Trim() != "" && x.InnerHtml.Trim() == x.InnerText.Trim())
                        //    .Select(x => x.InnerText.Trim())
                        //    .Take(3)
                        //    .ToList();
                        //Result.Add(key, t);
                        //Variables.AnchorTextInFile.Add(key, t);
                        #endregion
                        #region Old Method
                        for (int i = 0; i < body.Count; i++)
                        {
                            HtmlNode element = body[i];
                            HtmlNode NodeFound = null;
                            string id2 = element.Id;
                            if (id2 != "" && id2 == id)
                                NodeFound = element;
                            else
                                continue;

                            if (NodeFound != null)
                            {
                                List<string> t = GetText(NodeFound);
                                int j = i;
                                int Counter = t.Count;

                                while (Counter < 5)
                                {
                                    if (j < body.Count - 1)
                                    {
                                        j++;
                                        NodeFound = body[j];
                                    } else
                                        break;


                                    List<string> getText = GetText(NodeFound);
                                    if (getText != null && getText.Count > 0)
                                    {
                                        t.AddRange(getText);
                                        t = t.Distinct().ToList();
                                        Counter = t.Count;
                                    }

                                    if (t.Count >= 5)
                                        break;
                                }

                                Result.Add(key, t);
                                Variables.AnchorTextInFile.Add(key, t);
                                break;
                            }
                        }
                        #endregion
                    }
                }

            }

            return Result;
        }

        public Dictionary<string, List<string>> FindAchorTextInFile(string path, string id)
        {
            Dictionary<string, List<string>> Result = new Dictionary<string, List<string>>();
            List<string> Anchors = new List<string>();
            Anchors.Add(id);

            Result = FindAchorTextInFile(path, Anchors);

            return Result;
        }

        private static List<string> GetText(HtmlNode NodeFound)
        {
            if (NodeFound != null)
            {
                var idTextNodes = NodeFound.Descendants("#text");

                var Clean = idTextNodes.Select(x => x.InnerText.Trim().Replace("\n", " "));

                var Cleaned = from n in Clean
                              where !String.IsNullOrEmpty(HttpUtility.HtmlDecode(n))
                              select HttpUtility.HtmlDecode(n);

                List<string> t = (from n in Cleaned
                                  where !string.IsNullOrEmpty(n.Trim())
                                  select n).Take(5).ToList();

                return t;
            }

            return null;
        }
        #endregion

        #region FindAnchor
        public List<string> FindAnchorsInFile(string filename)
        {
            if (Variables.AnchorsInFile.ContainsKey(filename))
            {
                return Variables.AnchorsInFile[filename];
            } else
            {
                AnchorsInFile = new List<string>();
                HtmlDocument html = GetHtml(filename);
                if (html != null)
                {
                    var elements = html.DocumentNode.DescendantNodes();
                    foreach (HtmlNode item in elements)
                    {
                        FindAnchorsInFile(item);
                    }
                }

                Variables.AnchorsInFile.Add(filename, AnchorsInFile);
                return AnchorsInFile;
            }
        }

        private void FindAnchorsInFile(HtmlNode elements)
        {
            foreach (HtmlNode item in elements.DescendantNodes())
            {
                string id = item.GetAttributeValue("id", "");
                if (id != "" && !AnchorsInFile.Contains(id))
                {
                    AnchorsInFile.Add(id);
                }
                FindAnchorsInFile(item);
            }
        }
        #endregion

        #region Get Html from File
        internal HtmlDocument GetHtml(string filename)
        {
            try
            {
                Stream htmlStream = base.GetStreamOPF(filename);
                HtmlDocument html = new HtmlDocument();
                html.Load(htmlStream, Encoding.UTF8);

                return html;
            } catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Embedding Image & CSS into HTML
        //public override Stream GetStream(string FilenameInsideEpub)
        //{
        //    Stream FinalStream = null;
        //    try
        //    {
        //        Stream htmlStream = base.GetStream(FilenameInsideEpub);
        //        Stream ConvertStream = EmbedImageIntoHTML(htmlStream);
        //        FinalStream = EmbedCSSIntoHTML(ConvertStream);
        //    } catch (Exception)
        //    {
        //        return null;
        //    }

        //    return FinalStream;
        //}
        //private Stream EmbedImageIntoHTML(Stream htmlStream)
        //{
        //    HtmlDocument html = new HtmlDocument();
        //    html.Load(htmlStream);
        //    Encoding encod = html.StreamEncoding;
        //    htmlStream.Seek(0, SeekOrigin.Begin);

        //    //Find the image needed
        //    HtmlNodeCollection imgTagNormal = html.DocumentNode.SelectNodes("//img");
        //    HtmlNodeCollection imgTagHref = html.DocumentNode.SelectNodes("//image ");
        //    HtmlNodeCollection imgTag = imgTagNormal == null ? imgTagHref : imgTagNormal;
        //    if (imgTag != null)
        //    {
        //        foreach (HtmlNode item in imgTag)
        //        {
        //            //Get the path for the Images
        //            string tag = imgTagNormal == null ? "xlink:href" : "src";
        //            string src = item.GetAttributeValue(tag, "");

        //            //Remove dots and / at the begining
        //            string prefix = Regex.Match(src, "(?<prefix>[./]*).*", RegexOptions.IgnoreCase).Groups["prefix"].Value;
        //            src = string.IsNullOrEmpty(prefix) ? src : src.Replace(prefix, "");

        //            string ext = src.Split('.')[1];

        //            //Get the Images and COnvert them into Base 64
        //            Stream s = base.GetStream(src);
        //            byte[] bytearray = new byte[s.Length];
        //            s.Read(bytearray, 0, bytearray.Length);
        //            string base64string = Convert.ToBase64String(bytearray);

        //            //REPLACE IMG TAG WITH BASE 64 ENCODING
        //            string inString = tag + "=\"" + prefix + src + "\"";
        //            string outString = "src=\"data:image/" + ext + ";base64," + base64string + "\"";
        //            string base64html = item.OuterHtml.Replace(inString, outString);
        //            HtmlNode newNode = HtmlNode.CreateNode(base64html);
        //            item.ParentNode.ReplaceChild(newNode, item);
        //        }

        //        MemoryStream base64HtmlStream = new MemoryStream();
        //        html.Save(base64HtmlStream, encod);
        //        base64HtmlStream.Flush();
        //        base64HtmlStream.Seek(0, SeekOrigin.Begin);
        //        return base64HtmlStream;

        //    }

        //    return htmlStream;
        //}
        //private Stream EmbedCSSIntoHTML(Stream convertStream)
        //{
        //    String css = base.ByteToString(base.GetStream("css"));
        //    HtmlDocument html = new HtmlDocument();
        //    html.Load(convertStream);
        //    Encoding encod = html.StreamEncoding;
        //    convertStream.Seek(0, SeekOrigin.Begin);

        //    //Find the styleNode
        //    IEnumerable<HtmlAttribute> attribute = html.DocumentNode.SelectSingleNode("//head").ChildNodes.SelectMany(x => x.Attributes);
        //    HtmlNode styleTag = (from a in attribute
        //                         where a.Value == "text/css"
        //                         select a.OwnerNode).FirstOrDefault();
        //    if (styleTag != null)
        //    {

        //        //REPLACE style Tag
        //        string outString = "<style type=\"text/css\">" + css + "</style>";
        //        HtmlNode newNode = HtmlNode.CreateNode(outString);
        //        styleTag.ParentNode.ReplaceChild(newNode, styleTag);

        //        MemoryStream cssStream = new MemoryStream();
        //        html.Save(cssStream, encod);
        //        cssStream.Seek(0, SeekOrigin.Begin);
        //        return cssStream;

        //    }

        //    return convertStream;
        //} 
        #endregion


    }
}
