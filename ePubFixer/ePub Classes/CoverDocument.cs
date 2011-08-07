using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using HtmlAgilityPack;
using System.Drawing;
using System.Drawing.Imaging;

namespace ePubFixer
{
    public class CoverDocument : Document
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

        private MyHtmlDocument MyHtmlDoc;
        private HtmlNode ImageNode;

        private OpfDocument MyOPFDoc;
        private XElement ManifestGuide;

        private string CoverFile;
        private string ImageURL = string.Empty;
        private Image BookImage = null;
        private bool IsGuideEmpty = true;

        #endregion

        #region Constructor
        public CoverDocument()
            : base()
        {
            //GetImage();
        } 
        #endregion

        internal override void UpdateFile()
        {
            GetImage();
            frmCover frm = new frmCover(BookImage);
            frm.CoverChanged += new EventHandler<CoverChangedArgs>(ExportNewCover);
            frm.ShowDialog();
        }

        private void ExportNewCover(object sender, CoverChangedArgs e)
        {
            if (e.Cover != null)
            {
                bool ChangedCoverFile = false;
                bool FixedCoverWidth = false;

                #region Replace Existing File with the New One if it is different
                if (!ImageCompare(e.Cover, BookImage))
                {
                    fileOutName = Zip.GetFilePathInsideZip(ImageURL);
                    fileOutStream = e.Cover.ToStream(ImageFormat.Jpeg);
                    UpdateZip();
                    e.Message = SaveMessage;
                    ChangedCoverFile = true;
                } else
                {
                    e.Message = "File has not changed, Aborting";
                }
                #endregion

                #region Make sure it is scaled to fit
                string[] Name = { "height","width"};
                string Fit = "100%";

                foreach (var item in Name)
                {
                    string Heigth = ImageNode.GetAttributeValue(item, "");
                    string Value = item == "height" ? Fit : Fit;

                    if (Heigth != Value)
                    {
                        if (string.IsNullOrEmpty(Heigth))
                        {
                            ImageNode.SetAttributeValue(item, Value);
                        } else
                        {
                            ImageNode.Attributes[item].Value = Value;
                        }

                        FixedCoverWidth = true;
                    } 
                }

                if (FixedCoverWidth)
                {
                    MyHtmlDoc.TidyHtml(ImageNode.OwnerDocument.DocumentNode.OuterHtml);
                    MyHtmlDoc.UpdateZip();

                    e.Message = !ChangedCoverFile ? "File has not changed, But making sure that it is scaled to fit" : e.Message;
                }
                
                #endregion

                #region If guide is empty add it
                if (IsGuideEmpty)
                {
                    MyOPFDoc.AddCoverRef(CoverFile);
                    e.Message = !ChangedCoverFile && !FixedCoverWidth ? "File has not changed, But fixing missing Cover Tag in guide" : e.Message;
                } 
                #endregion

                //Update the stream and BookImage with the new default
                GetImage();

                e.ChangedCoverFile = ChangedCoverFile;
            } else
            {
                e.Message = "Cover is Empty, Aborting";
            }
        }

        private string GetCoverFile()
        {
            MyOPFDoc = new OpfDocument();
            ManifestGuide = MyOPFDoc.GetXmlElement("guide");
            string coverRef = string.Empty;

            if (ManifestGuide != null)
                coverRef = MyOPFDoc.GetCoverRef();

            //If it does not exist get first page in spine
            if (string.IsNullOrEmpty(coverRef))
                coverRef = MyOPFDoc.GetSpineRefAtIndex(0);
            else
                IsGuideEmpty = false;

            return coverRef;
        }

        private void GetImage()
        {
            CoverFile = GetCoverFile();
            MyHtmlDoc = new MyHtmlDocument();
            HtmlDocument HtmlDoc = MyHtmlDoc.GetHtml(CoverFile);
            ImageNode = null;

            Dictionary<string, string> TagToCheck = new Dictionary<string, string>();
            TagToCheck.Add("img", "src");
            TagToCheck.Add("image", "xlink:href");
            //TagToCheck.Add("svg:image", "xlink:href");

            foreach (var item in TagToCheck)
            {
                ImageNode = HtmlDoc.DocumentNode.SelectSingleNode("//" + item.Key);
                if (ImageNode != null)
                {
                    ImageURL = ImageNode.Attributes[item.Value].Value;

                    //Clean URL
                    ImageURL = ImageURL.Replace("../", "");
                    break;

                }
            }

            fileExtractStream = GetStream(ImageURL);
            BookImage = string.IsNullOrEmpty(ImageURL) ? null : Image.FromStream(fileExtractStream);
        }

        public static bool ImageCompare(Image firstImage, Image secondImage)
        {
            if (firstImage != null && secondImage != null)
            {
                MemoryStream ms = new MemoryStream();
                firstImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                String firstBitmap = Convert.ToBase64String(ms.ToArray());
                ms.Position = 0;

                secondImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                String secondBitmap = Convert.ToBase64String(ms.ToArray());

                if (firstBitmap.Equals(secondBitmap))
                {
                    return true;
                } else
                {
                    return false;
                }
            } else
            {
                return false;
            }
        }

    }
}
