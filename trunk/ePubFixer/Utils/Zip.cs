using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using System.IO;

namespace ePubFixer
{
    class Zip
    {

        public static event EventHandler<ExtractProgressArgs> ExtractProgress;
        static ExtractProgressArgs Progress;

        #region ZipExtract
        internal static void ExtractZip()
        {
            try
            {
                using (ZipFile zip = ZipFile.Read(Variables.Filename))
                {
                    Progress = new ExtractProgressArgs();
                    Progress.TotalToTransfer = Convert.ToInt32(zip.Sum(e => e.UncompressedSize));
                    zip.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(zip_ExtractProgress);
                    Old = 0; New = 0;
                    foreach (ZipEntry item in zip)
                    {

                        item.Extract(Variables.TempFolder, ExtractExistingFileAction.OverwriteSilently);
                        //Progress.Transferred += Convert.ToInt32(item.UncompressedSize);
                        ////System.Threading.Thread.Sleep(100);

                        //if (ExtractProgress!=null)
                        //{
                        //    ExtractProgress(item, Progress);
                        //}
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        static long Old;
        static long New;
        static void zip_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            if (e.EventType == ZipProgressEventType.Extracting_EntryBytesWritten)
            {
                New = e.BytesTransferred;
                Progress.Transferred += New - Old;
                Old = e.BytesTransferred;

                if (ExtractProgress != null)
                {
                    ExtractProgress(e.CurrentEntry, Progress);
                }

            } else if (e.EventType == ZipProgressEventType.Extracting_AfterExtractEntry)
            {
                Old = 0;
            }
        }
        internal static void DeleteTemp()
        {
            try
            {
                if (Directory.Exists(Variables.TempFolder))
                {
                    Directory.Delete(Variables.TempFolder, true);
                }
            }
            catch (Exception)
            {

            }
        }

        internal static string GetTempFilePath(string filename)
        {
            string file = GetFilePathInsideZip(filename);
            file = file.Replace('/', '\\');
            file = Variables.TempFolder + @"\" + file;

            file = Preview.ConvertToHTML(file);

            return file;
        }

        #endregion

        //internal static string GetFilePathInsideZipOPF(string filename)
        //{
        //    string file = "";
        //    if (!String.IsNullOrEmpty(filename))
        //    {
        //        file = (from z in Variables.ZipFileList
        //                where z == Variables.OPFpath + filename
        //                select z).FirstOrDefault();
        //    }

        //    return file;
        //}

        internal static string GetFilePathInsideZip(string filename)
        {
            string file = "";
            if (!String.IsNullOrEmpty(filename))
            {
                file = (from z in Variables.ZipFileList
                        where z.EndsWith(filename)
                        select z).FirstOrDefault();
            }

            return file;
        }

        internal static List<string> GetFilesListInsideZip()
        {
            List<string> fileLIst = new List<string>();

            using (ZipFile zip = ZipFile.Read(Variables.Filename))
            {
                fileLIst = zip.Entries.Select(x => x.FileName).ToList();
            }

            return fileLIst;
        }

    }
}
