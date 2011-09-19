using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Ionic.Zip;
using Aga.Controls.Tree;
using System.IO;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using ePubFixer.Properties;
using System.Xml.Linq;

namespace ePubFixer
{
    public static class Utils
    {
        private static Dictionary<string, string> FileList = null;

        #region NewFileName
        public static void NewFilename(bool BackupDone)
        {
            Decryption.FileDecrypted = false;
            Variables.HeaderTextInFile = new Dictionary<string, DetectedHeaders>();
            Variables.AnchorTextInFile = new Dictionary<string, DetectedHeaders>();
            Variables.AnchorsInFile = new Dictionary<string, List<string>>();
            FileList = null;
            Variables.BackupDone = BackupDone;
            Variables.OPFfile = string.Empty;
            Variables.NCXFile = string.Empty;
            Variables.TOCDownloadedFromNet = false;
            Variables.ZipFileList = new List<string>();
            Variables.FilesPathFromOPF = new List<string>();
        }

        public static void NewFilename()
        {
            NewFilename(false);
        } 
        #endregion


        #region Get Icon & Title
        public static string GetTitle()
        {
            Assembly ass = Assembly.GetExecutingAssembly();
            var name = ass.GetName();
            String Version = "v" + ass.GetName().Version.Major + ".";
            Version += ass.GetName().Version.Minor;
            if (ass.GetName().Version.Build > 0)
            {
                Version += "." + ass.GetName().Version.Build;
            }

            // assembly title attribute
            String Title = ((AssemblyTitleAttribute)ass.GetCustomAttributes(
                        typeof(AssemblyTitleAttribute), false)[0]).Title;

            //#if DRM
            //            Title += " (DRM Removal)";
            //#endif

            return Title + " " + Version;

        }

        public static Icon GetIcon()
        {
            string str = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name;
            return new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream(str + ".epub.ico"));
        }
        #endregion

        #region Get Content.opf Info
        public static string GetSrc(string idRef)
        {
            OpfDocument doc = new OpfDocument();
            Dictionary<string, string> srcTag = doc.GetFilesList();

            string src;
            if (srcTag.TryGetValue(idRef, out src))
            {
                return src;
            }
            return null;
        }

        public static string GetId(string Src)
        {
            OpfDocument doc = new OpfDocument();
            Dictionary<string, string> srcTag = doc.GetFilesList();

            return GetId(Src, srcTag);
        }

        public static string GetId(string Src, Dictionary<string, string> srcTag)
        {
            foreach (var item in srcTag)
            {
                if (item.Value == Src)
                {
                    return item.Key;
                }
            }

            return Guid.NewGuid().ToString();
        }

        public static Dictionary<string, string> GetFileList()
        {
            if (FileList == null)
            {
                OpfDocument doc = new OpfDocument();
                FileList = doc.GetFilesList();
            }

            return FileList;
        }
        #endregion

        #region Verify Files
        public static bool VerifyFileExists(string filename)
        {
            string file = Zip.GetFilePathInsideZip(filename);
            file = string.IsNullOrEmpty(file) ? Zip.GetFilePathInsideZip(System.Web.HttpUtility.UrlDecode(filename)) : file;
            file = string.IsNullOrEmpty(file) ? Zip.GetFilePathInsideZip(System.Web.HttpUtility.UrlPathEncode(filename)) : file;
            return string.IsNullOrEmpty(file) ? false : true;
        }

        private static bool CheckPath(string Path)
        {
            var path = (from f in Variables.ZipFileList
                        where f.EndsWith(Path)
                        select f).FirstOrDefault();

            return string.IsNullOrEmpty(path) ? false : true;

        }
        public static string VerifyFilenameEncoding(string file)
        {
            string EncodedPath = System.Web.HttpUtility.UrlPathEncode(file);
            string DecodedPath = System.Web.HttpUtility.UrlDecode(file);
            
            if (CheckPath(DecodedPath))
            {
                return DecodedPath;
            } else if (CheckPath(EncodedPath))
            {
                return EncodedPath;
            } else
            {
                return file;
            }
        }


        public static System.Collections.ObjectModel.Collection<Node> RemoveNonExistantNode(System.Collections.ObjectModel.Collection<Node> NodeCollection)
        {
            OpfDocument opfDoc = new OpfDocument();
            List<string> htmlFiles = opfDoc.GetFilesList("html");

            for (int i = NodeCollection.Count - 1; i >= 0; i--)
            {
                Node item = NodeCollection[i];
                NavDetails nav = item.Tag as NavDetails;

                if (nav.ContentSrc == null || !Utils.VerifyFileExists(nav.File) | !htmlFiles.Contains(nav.File))
                {
                    NodeCollection.Remove(item);
                }

                if (item.Nodes.Count > 0)
                {
                    RemoveNonExistantNode(item.Nodes);
                }
            }

            return NodeCollection;
        }
        #endregion

        #region Save Recent Files
        public static void SaveRecentFiles()
        {
            if (Properties.Settings.Default.RecentFiles == null)
                Properties.Settings.Default.RecentFiles = new System.Collections.ArrayList();

            List<string> LastFiles = new List<string>();

            foreach (string item in Properties.Settings.Default.RecentFiles)
            {
                LastFiles.Add(item);
            }

            LastFiles.AddRange(Variables.Filenames);
            Utils.SaveRecentFilesSettings(LastFiles);
        }

        public static void SaveRecentFilesSettings(List<string> LastFiles)
        {
            int Qty = LastFiles.Count > 10 ? 10 : LastFiles.Count;
            LastFiles = LastFiles.GetRange(LastFiles.Count - Qty, Qty);
            Properties.Settings.Default.RecentFiles.Clear();

            foreach (string item in LastFiles)
            {

                if (!Properties.Settings.Default.RecentFiles.Contains(item))
                    Properties.Settings.Default.RecentFiles.Add(item);
                else
                {
                    int index = LastFiles.IndexOf(item);
                    Properties.Settings.Default.RecentFiles.RemoveAt(index);
                    Properties.Settings.Default.RecentFiles.Add(item);
                }
            }

            Properties.Settings.Default.Save();
        }
        #endregion

        #region Delete Files
        internal static void DeleteFiles(IEnumerable<string> FilesToDelete)
        {
            //Delete Files
            foreach (string source in FilesToDelete)
            {
                try
                {
                    using (ZipFile zip = ZipFile.Read(Variables.Filename))
                    {
                        string path = Variables.OPFpath + source;
                        zip.RemoveEntry(path);
                    }
                }
                catch (Exception e)
                {
                    //File not found in Zip or Other Error
                    MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }
            }


            //Delete from manifest and spine
            DeleteFromSpine(FilesToDelete);
            DeleteFromManifest(FilesToDelete);
            DeleteFromGuide(FilesToDelete);

        }

        public static void DeleteFromManifest(IEnumerable<string> FilesTodelete)
        {
            OpfDocument doc = new OpfDocument();
            XElement oldManifest = doc.GetXmlElement("manifest");
            XNamespace ns = oldManifest.Name.Namespace;
            List<XElement> newItems = new List<XElement>();

            foreach (XElement item in oldManifest.Elements())
            {
                string href = item.Attribute("href").Value;
                string id = item.Attribute("id").Value;
                string mediaType = item.Attribute("media-type").Value;
                if (!FilesTodelete.Contains(href))
                {
                    newItems.Add(item);
                }
            }

            XElement newManifest = new XElement(ns + "manifest", newItems);
            doc.ReplaceManifest(newManifest);

            //if (doc.SaveMessage.StartsWith("Error"))
            //    MessageBox.Show(doc.SaveMessage, "Manifest Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void DeleteFromSpine(IEnumerable<string> FilesTodelete)
        {
            OpfDocument doc = new OpfDocument();
            XElement oldSpine = doc.GetXmlElement("spine");
            XNamespace ns = oldSpine.Name.Namespace;
            List<XElement> newItems = new List<XElement>();

            foreach (XElement item in oldSpine.Elements())
            {
                string id = item.Attribute("idref").Value;
                if (!FilesTodelete.Contains(GetSrc(id)))
                {
                    newItems.Add(item);
                }
            }

            IEnumerable<XAttribute> spine = oldSpine.Attributes();
            XElement newSpine = null;

            if (spine.Count() > 0)
            {
                newSpine = new XElement(ns + "spine", spine, newItems);
            } else
            {
                //There is no spine elements
                XAttribute TocRef = new XAttribute("toc", doc.GetNCXid());

                newSpine = new XElement(ns + "spine", TocRef, newItems);
            }

            doc.ReplaceSpine(newSpine);

            if (doc.SaveMessage.StartsWith("Error"))
                MessageBox.Show(doc.SaveMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void DeleteFromGuide(IEnumerable<string> FilesToDelete)
        {
            OpfDocument doc = new OpfDocument();
            XElement oldManifest = doc.GetXmlElement("guide");
            XNamespace ns = oldManifest.Name.Namespace;
            List<XElement> newItems = new List<XElement>();

            foreach (XElement item in oldManifest.Elements())
            {
                string href = item.Attribute("href").Value;
                if (!FilesToDelete.Contains(href))
                {
                    newItems.Add(item);
                }
            }

            XElement newManifest = new XElement(ns + "guide", newItems);
            doc.ReplaceSection(newManifest,"guide");
        }
        #endregion
    }
}

