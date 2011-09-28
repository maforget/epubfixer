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
using System.Text;

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

        private string CoverFile;
        private string ImageURL = string.Empty;
        private Image BookImage = null;
        private static bool ImageIsSVG = false;
        private bool IsGuideEmpty = true;

        //Settings For MassUpdate
        internal static bool MassPreserveRatio = true;
        internal static SourceOfCover MassSource = SourceOfCover.FromBook;
        public static double ImageRatio = 0.75;
        public enum SourceOfCover { FromFolder, FromBook }

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

        #region Image
        public static Image ResizeImage(Image image, double WidthRatioVsHeigth, bool preserveRatio)
        {
            int height = image.Height;
            int Width = image.Width;
            int ResizedWidth = !preserveRatio ? (int)(height * WidthRatioVsHeigth) : image.Width;

            Bitmap b = new Bitmap(ResizedWidth, height);
            Graphics g = Graphics.FromImage(b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, 0, 0, ResizedWidth, height);
            g.Dispose();
            return b;
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
        #endregion

        #region Export NEw COver
        private string OriginalImageURL;
        bool PreserveAspectRatio;
        private bool FixHtml(CoverChangedArgs e)
        {
            bool FixedCoverWidth = false;
            PreserveAspectRatio = e.PreserveAspectRatio;

            string HtmlHeigth = ImageNode.GetAttributeValue("height", "");
            string HtmlWidth = ImageNode.GetAttributeValue("width", "");
            string HeigthValue = e.Heigth.ToString();
            string WidthValue = e.Width.ToString();

            string SVGAspectValue = "xMidYMid meet";
            //string SVGAspectValue = PreserveAspectRatio ? "xMidYMid meet" : "none";
            //string SVGHeigthValue = PreserveAspectRatio ? "100%" : "800";
            //string SVGWidthValue = PreserveAspectRatio ? "100%" : "600";
            string SVGHeigthValue = "100%";
            string SVGWidthValue = "100%";
            HtmlAttribute SVGAspectAttri = ImageNode.ParentNode.Attributes["preserveAspectRatio"];
            HtmlAttribute SVGHeigthAttri = ImageNode.ParentNode.Attributes["height"];
            HtmlAttribute SVGWidthAttri = ImageNode.ParentNode.Attributes["width"];

            HtmlNode body = ImageNode.OwnerDocument.DocumentNode.SelectSingleNode("//body");
            HtmlNodeCollection css = ImageNode.OwnerDocument.DocumentNode.SelectNodes("//head/link");
            HtmlNode BodyStyle = ImageNode.OwnerDocument.DocumentNode.SelectSingleNode("//head/style");
            string styleValue = "margin:0; padding: 0; border-width: 0";
            HtmlAttribute styleAttri = body.Attributes["style"];
            HtmlAttribute classAttri = body.Attributes["class"];

            if (HtmlHeigth != HeigthValue | HtmlWidth != WidthValue | (SVGAspectAttri != null && SVGAspectAttri.Value != SVGAspectValue) | (SVGHeigthAttri != null && SVGHeigthAttri.Value != SVGHeigthValue) | (SVGWidthAttri != null && SVGWidthAttri.Value != SVGWidthValue) | !ImageIsSVG | (styleAttri != null && styleAttri.Value != styleValue) | styleAttri == null | css != null | BodyStyle != null)
            {
                try
                {
                    ImageNode.SetAttributeValue("height", HeigthValue);
                    ImageNode.SetAttributeValue("width", WidthValue);

                    if (ImageIsSVG)
                    {
                        ImageNode.ParentNode.SetAttributeValue("preserveAspectRatio", SVGAspectValue);
                        ImageNode.ParentNode.Attributes["viewBox"].Value = "0 0 " + e.Width.ToString() + " " + e.Heigth.ToString();
                        ImageNode.ParentNode.SetAttributeValue("height", SVGHeigthValue);
                        ImageNode.ParentNode.SetAttributeValue("width", SVGWidthValue);

                        if (css != null)
                            css.ToList().ForEach(x => x.Remove());//Remove any css or xgpt file
                        if (BodyStyle != null)
                            BodyStyle.Remove();//Remove the style element
                        if (classAttri != null)
                            classAttri.Remove();//Delete the class attribute because it is no longer needed
                        body.SetAttributeValue("style", styleValue);//replace the style class with a style Attribute

                    } else
                    {

                        //If file is not SVG base Convert it and rerun the Fix
                        string SVGhtml = @"<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" height=""800"" preserveAspectRatio=""none"" version=""1.1"" viewBox=""0 0 600 800"" width=""100%"">
      <image height=""800"" width=""100%"" xlink:href=""" + this.OriginalImageURL + @"""></image>
    </svg>";
                        string newHtml = ImageNode.OwnerDocument.DocumentNode.OuterHtml.Replace(ImageNode.OwnerDocument.DocumentNode.SelectSingleNode("//body").InnerHtml, SVGhtml);

                        //Reload ImageNode
                        HtmlDocument newDoc = new HtmlDocument();
                        newDoc.OptionDefaultStreamEncoding = Encoding.UTF8;
                        newDoc.LoadHtml(newHtml);
                        ImageNode = newDoc.DocumentNode.SelectSingleNode("//image");
                        ImageIsSVG = true;

                        FixHtml(e);
                    }

                    FixedCoverWidth = true;
                }
                catch (Exception) { }
            }

            return FixedCoverWidth;
        }

        private void ExportNewCover(object sender, CoverChangedArgs e)
        {
            using (new HourGlass())
            {
                if (e.Cover != null && ImageNode != null)
                {
                    bool ChangedCoverFile = false;

                    #region Replace Existing File with the New One if it is different
                    if (!ImageCompare(e.Cover, BookImage))
                    {
                        fileOutName = Zip.GetFilePathInsideZip(ImageURL);
                        fileOutStream = e.Cover.ToStream(BookImage.RawFormat);
                        UpdateZip();
                        e.Message = SaveMessage;
                        ChangedCoverFile = true;
                    } else
                    {
                        e.Message = "File has not changed, Aborting";
                    }
                    #endregion

                    //Make sure it is scaled to fit
                    bool FixedCoverWidth = FixHtml(e);

                    if (FixedCoverWidth)
                    {
                        MyHtmlDoc.fileOutStream = MyHtmlDoc.TidyHtml(ImageNode.OwnerDocument.DocumentNode.OuterHtml).ToStream();
                        MyHtmlDoc.UpdateZip();

                        if (PreserveAspectRatio)
                            e.Message = !ChangedCoverFile ? "File has not changed, But Fixing the dimensions" : e.Message;
                        else
                            e.Message = !ChangedCoverFile ? "File has not changed, But making sure that it is scaled to fit" : e.Message;
                    }

                    #region If guide is empty add it
                    if (IsGuideEmpty)
                    {
                        MyOPFDoc.AddCoverRef(CoverFile);
                        e.Message = !ChangedCoverFile && !FixedCoverWidth ? "File has not changed, But fixing missing Cover Tag in guide" : e.Message;
                    }
                    #endregion

                    //Check to see if the cover file is the first
                    CheckPositionOfCover();

                    //Update the stream and BookImage with the new default
                    GetImage();

                    e.ChangedCoverFile = ChangedCoverFile;
                } else
                {
                    e.Message = "Cover is Empty, Aborting";
                }
            }
        } 
        #endregion

        #region Get Image From Book
        private string GetCoverFile()
        {
            MyOPFDoc = new OpfDocument();
            string coverRef = MyOPFDoc.GetCoverRef();

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

            foreach (var item in TagToCheck)
            {
                ImageNode = HtmlDoc.DocumentNode.SelectSingleNode("//" + item.Key);
                if (ImageNode != null)
                {
                    ImageIsSVG = ImageNode.ParentNode.Name == "svg" ? true : false;
                    ImageURL = ImageNode.Attributes[item.Value].Value;

                    //Clean URL
                    OriginalImageURL = ImageURL;
                    ImageURL = ImageURL.Replace("../", "").Trim();
                    ImageURL = Utils.VerifyFilenameEncoding(ImageURL);
                    break;
                }
            }

            fileExtractStream = GetStream(ImageURL);

            if(fileExtractStream!=null)
                BookImage = string.IsNullOrEmpty(ImageURL) ? null : Image.FromStream(fileExtractStream);
        }
        #endregion



        private void CheckPositionOfCover()
        {
            string FirstRef = MyOPFDoc.GetSpineRefAtIndex(0);
            if (FirstRef != CoverFile)
            {
                //TODO Check if linearize = off
                System.Windows.Forms.MessageBox.Show("The Cover File is not the first File\n" +
                "Please use the Reading Order editor to modify it (id=" + Utils.GetId(CoverFile) + ")", "Cover is Not the First File", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
        }

    }
}
