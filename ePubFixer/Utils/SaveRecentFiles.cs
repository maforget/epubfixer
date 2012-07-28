using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ePubFixer
{
    public static class RecentFiles
    {
        #region Save Recent Files
        public static void SaveRecentFiles()
        {
            //if (Properties.Settings.Default.RecentFiles == null)
            //    Properties.Settings.Default.RecentFiles = new System.Collections.ArrayList();

            List<string> LastFiles = new List<string>();

            foreach (string item in Properties.Settings.Default.RecentFiles2.LoadSettings())
            {
                LastFiles.Add(item);
            }

            LastFiles.AddRange(Variables.Filenames);
            SaveRecentFilesSettings(LastFiles);
        }

        private static void SaveRecentFilesSettings(List<string> LastFiles)
        {
            int Qty = LastFiles.Count > 10 ? 10 : LastFiles.Count;
            LastFiles = LastFiles.GetRange(LastFiles.Count - Qty, Qty);
            List<string> RecentFiles = new List<string>();

            foreach (string item in LastFiles)
            {
                if (!RecentFiles.Contains(item))
                    //aDD iT
                    RecentFiles.Add(item);
                else
                {
                    //mOVE IT ON TOP
                    int index = RecentFiles.IndexOf(item);
                    RecentFiles.RemoveAt(index);
                    RecentFiles.Add(item);
                }
            }

            Properties.Settings.Default.RecentFiles2 = RecentFiles.SaveSettings();
            Properties.Settings.Default.Save();
        }
        #endregion
    }
}
