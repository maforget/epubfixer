using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace ePubFixer
{
    public static class Variables
    {
        #region Fields & Path
        public static string SigilDefaultPath = "C:\\Program Files\\Sigil\\Sigil.exe";
        public static bool BackupDone { get; set; }
        public static bool DoBackup { get; set; }
        public static string VersionLocation = @"http://epubfixer.googlecode.com/svn/trunk/ePubFixer/Version.xml";
        public static string DownloadLocation = @"http://code.google.com/p/epubfixer/downloads/list";
        public static Dictionary<string, DetectedHeaders> HeaderTextInFile;
        public static Dictionary<string, DetectedHeaders> AnchorTextInFile;
        public static Dictionary<string, List<string>> AnchorsInFile;

        public static List<Form> OpenedForm = new List<Form>();
        public static bool TOCDownloadedFromNet = false;

        public static string ApplicationName
        {
            get
            {
                return (System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            }
        }

        private static string _NCXFile;
        public static string NCXFile
        {
            get
            {
                if (String.IsNullOrEmpty(_NCXFile))
                {
                    OpfDocument doc = new OpfDocument();
                    _NCXFile = doc.GetNCXfilename();
                }
                return _NCXFile;
            }
            set
            {
                _NCXFile = value;
            }
        }

        private static string _OPFfile;
        public static string OPFfile
        {
            get
            {
                if (String.IsNullOrEmpty(_OPFfile))
                {
                    ContainerDocument doc = new ContainerDocument();
                    _OPFfile = doc.GetOPFfilename();
                }
                return _OPFfile;
            }
            set
            {
                _OPFfile = value;
            }
        }

        public static string OPFpath
        {
            get
            {
                return GetPath(OPFfile);
            }
        }

        public static string GetPath(string file)
        {
            string[] split = file.Split('/');
            System.Text.StringBuilder pathBuilder = new System.Text.StringBuilder();

            for (int i = 0; i < split.Length - 1; i++)
            {
                pathBuilder.Append(split[i] + "/");
            }

            return pathBuilder.ToString();
        }


        #region FileInZip
        private static List<string> _ZipFileList;
        public static List<string> ZipFileList
        {
            get
            {
                if (_ZipFileList == null || _ZipFileList.Count == 0)
                {
                    _ZipFileList = Zip.GetFilesListInsideZip();
                }

                return _ZipFileList;
            }

            set
            {
                _ZipFileList = value;
            }
        }
        #endregion


        #region FileInZip
        private static List<string> _FilesPathFromOPF;
        public static List<string> FilesPathFromOPF
        {
            get
            {
                if (_FilesPathFromOPF == null || _FilesPathFromOPF.Count == 0)
                {
                    OpfDocument doc = new OpfDocument();
                    _FilesPathFromOPF = doc.GetFilesFromOPF();
                }

                return _FilesPathFromOPF;
            }

            set
            {
                _FilesPathFromOPF = value;
            }
        }
        #endregion

        #endregion

        #region Filenames
        public static string Filename { get; set; }

        private static List<string> _Filenames = new List<string>();
        public static List<string> Filenames
        {
            get
            {
                return _Filenames;
            }
            set
            {
                _Filenames = value;
            }
        }

        public static string BackupName
        {
            get
            {
                return Path.GetDirectoryName(Filename) + "\\" + Path.GetFileNameWithoutExtension(Filename) + " - Backup.epub";
            }
        }
        #endregion

        #region Temp Folder
        public static string TempFolder
        {
            get { return Path.GetTempPath() + @"ePubFixerTemp." + BookName; }
        }
        #endregion

        #region Book Name
        public static string BookName
        {
            get { return GetBookName(); }
        }

        private static string GetBookName()
        {
            string[] s = Filename.Split('\\');
            return s[s.Length - 1];
        }
        #endregion




    }

    public enum AddWindowType
    {
        SpineEdit, TOCEdit
    }
}
