using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ePubFixer
{
    class Decryption
    {

        public static bool FileDecrypted = false;
        private static void ShowEncryptedMessage()
        {
            MessageBox.Show("This file is protected by DRM\n\"" + Variables.BookName + "\"",
                                    "File Protected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        internal static bool IsEncrypted
        {
            get
            {
                //List<string> AdeptFiles = new List<string>() { "rights.xml"};
                List<string> AdeptFiles = new List<string>() { "META-INF/rights.xml", "META-INF/encryption.xml" };


                foreach (var item in AdeptFiles)
                {
                    if (string.IsNullOrEmpty(Zip.GetFilePathInsideZip(item)))
                        return false;
                }

#if !DRM
                ShowEncryptedMessage();
#else
                if (!Properties.Settings.Default.Decrypt)
                {
                    ShowEncryptedMessage();
                }
#endif

                return true;

            }
        }

        public static string SaveDirectory { get; set; }
        internal static bool DecryptFile()
        {
#if DRM
            try
            {
                if (Properties.Settings.Default.Decrypt && IsEncrypted && !FileDecrypted)
                {
                    FolderBrowserDialog save = new FolderBrowserDialog();
                    save.Description = "Please select the folder to save the Decrypted files into.";

                    if (string.IsNullOrEmpty(SaveDirectory))
                    {
                        if (save.ShowDialog() == DialogResult.OK)
                            SaveDirectory = save.SelectedPath;
                        else
                            return false;
                    }

                    string NewFilePath = SaveDirectory + "\\" + Variables.BookName;
                    if (File.Exists(NewFilePath) &&
                        MessageBox.Show("File Already Exists, Do you want to replace it?\n\"" + Variables.BookName + "\"",
                        "File Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                    {
                        return false;
                    }

                    string ProtectedFilePath = Variables.Filename;
                    Variables.Filename = NewFilePath;
                    Variables.Filenames[Variables.Filenames.IndexOf(ProtectedFilePath)] = NewFilePath;
                    using (new HourGlass())
                    {
                        FileDecrypted = Drm.Adept.Epub.Strip(ProtectedFilePath, NewFilePath);
                        if (FileDecrypted)
                        {
                            Properties.Settings.Default.RecentFiles.Add(NewFilePath);
                            Utils.SaveRecentFilesSettings(Properties.Settings.Default.RecentFiles.Cast<string>().ToList());

                        }
                        return FileDecrypted;
                    }

                }
                return false;
            }
            catch (Exception e)
            {
                FileDecrypted = false;
                MessageBox.Show("File could not be decrypted\n" + e.Message, "Decryption Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

#else
            return false;
#endif
        }


    }
}
