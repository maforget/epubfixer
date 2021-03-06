﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using System.IO;

namespace ePubFixer
{
    class Zip
    {

        public event EventHandler<ExtractProgressArgs> ExtractProgress;
        ExtractProgressArgs Progress;

        #region ZipExtract
        internal void ExtractZip()
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
                    }
                }
            } catch (Exception)
            {
            }
        }

        long Old;
        long New;
        void zip_ExtractProgress(object sender, ExtractProgressEventArgs e)
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
            } catch (Exception)
            {

            }
        }

        internal static string GetTempFilePath(string filename)
        {
            string file = GetFilePathInsideZip(filename);
            file = file.Replace('/', Path.DirectorySeparatorChar);
            file = Path.Combine(Variables.TempFolder, file);

            //file = Preview.ConvertToHTML(file); Done directly inside the preview class

            return file;
        }

        #endregion

        internal static string GetFilePathInsideZip(string filename)
        {
            string file = "";
            if (!String.IsNullOrEmpty(filename))
            {
                file = (from z in Variables.FilesPathFromOPF
                        where filename.StartsWith("../") ? filename.Substring(3) == z : Path.Combine(Variables.OPFpath, filename) == z || filename == z
                        select z).FirstOrDefault();

                if (string.IsNullOrEmpty(file))
                {
                    file = (from z in Variables.ZipFileList
                            where z.EndsWith(filename)
                            select z).FirstOrDefault();
                }
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
