using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.IO;

namespace ePubFixer
{
    public static class Preview
    {
        public static void CloseOpenedForms()
        {
            Variables.OpenedForm.ForEach(item => item.Close());
            Variables.OpenedForm = new List<Form>();
        }

        public static void ShowPreview(this Form frm, string filename, string Chapter)
        {

            try
            {
                ConvertToHTML(Zip.GetTempFilePath(filename));
                frmPreview frmPreview = new frmPreview(filename, Chapter);
                //frm.AddOwnedForm(frmPreview);
                Variables.OpenedForm.Add(frmPreview);
                frmPreview.Show();
            }
            catch (Exception)
            {

            }
        }

        public static void ShowPreview(this Form frm, string filename)
        {
            ShowPreview(frm, filename, "Preview");
        }

        public static string ConvertToHTML(string file)
        {
            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
            html.Load(file, Encoding.UTF8);

            //Find the image needed
            HtmlNodeCollection imgTag = html.DocumentNode.SelectNodes("//image ");
            if (imgTag != null)
            {
                foreach (HtmlNode item in imgTag)
                {
                    //Get the path for the Images
                    string tag = "xlink:href";
                    string src = item.GetAttributeValue(tag, "");

                    //Replace for XHTML
                    string inString = tag + "=\"" + src + "\"";
                    string outString = "src=\"" + src + "\"";
                    string newHtml = item.OuterHtml.Replace(inString, outString);
                    HtmlNode newNode = HtmlNode.CreateNode(newHtml);
                    item.ParentNode.ReplaceChild(newNode, item);
                }
            }

            #region Change Extension
            string ext = Path.GetExtension(file);
            if (ext == ".xhtml")
            {
                string newFile = Path.ChangeExtension(file, ".html");
                file = newFile;
            }

            if (ext != ".html")
            {
                string newFile = file + ".html";
                file = newFile;
            }
            #endregion

            File.WriteAllText(file, html.DocumentNode.OuterHtml, Encoding.UTF8);

            return file;
        }
    }
}
