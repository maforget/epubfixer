using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using System.Diagnostics;
using System.Linq.Expressions;
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
        public static Dictionary<string, List<string>> HeaderTextInFile;
        public static Dictionary<string, List<string>> AnchorTextInFile;
        public static Dictionary<string, List<string>> AnchorsInFile;
        public static bool FileDecrypted = false;
        public static List<Form> OpenedForm = new List<Form>();

        private static string _NCXFile;
        public static string NCXFile
        {
            get
            {
                OpfDocument doc = new OpfDocument();
                _NCXFile = doc.GetNCXfilename();
                return _NCXFile;
            }
        }

        private static string _OPFfile;
        public static string OPFfile
        {
            get
            {
                ContainerDocument doc = new ContainerDocument();
                _OPFfile = doc.GetOPFfilename();
                return _OPFfile;
            }
        }

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
