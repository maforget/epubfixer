using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Windows.Forms;

namespace ePubFixer
{
    public abstract class Document
    {
        #region Properties & Fields
        protected string Filename;
        protected internal abstract string fileOutName { get; set; }
        protected internal abstract Stream fileOutStream { get; set; }
        protected internal abstract Stream fileExtractStream { get; set; }
        protected abstract string nsIfEmpty { get; set; }
        protected abstract XElement NewTOC { get; set; }
        protected abstract XElement NewNavMap { get; set; }
        protected abstract XElement OldTOC { get; set; }
        protected abstract XNamespace ns { get; set; }
        #endregion

        #region Constructor

        public Document()
        {
            Filename = Variables.Filename;

            if (!Filename.EndsWith("epub"))
                return;

            fileOutStream = new MemoryStream();
        }
        #endregion

        internal abstract void UpdateFile();

        #region Get From Zip

        #region Get Stream From Zip
        protected Stream OpenZip(Predicate<ZipEntry> NameCheck)
        {
            try
            {
                MemoryStream s = new MemoryStream();
                using (ZipFile zip = ZipFile.Read(Variables.Filename))
                {
                    foreach (ZipEntry e in zip)
                    {
                        //OPen the CSS file
                        if (NameCheck.Invoke(e))
                        {
                            fileOutName = e.FileName;
                            //Extract the Css file to a stream

                            e.Extract(s);
                            s.Seek(0, SeekOrigin.Begin);
                            return s;
                        }
                    }
                }

                return null;
            } catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region Get Stream
        public virtual Stream GetStreamOPF(string FilenameInsideEpub)
        {
            Stream s = this.OpenZip(e => e.FileName == Variables.OPFpath + FilenameInsideEpub);
            return s;
        }

        public virtual Stream GetStream(string FilenameInsideEpub)
        {
            Stream s = this.OpenZip(e => e.FileName.EndsWith(FilenameInsideEpub));
            return s;
        }

        #endregion

        #region Get String From Stream
        protected string ByteToString(Stream stream)
        {
            if (stream == null || stream.Length <= 0)
                return null;

            //Reset Stream Position
            stream.Seek(0, SeekOrigin.Begin);

            byte[] bytearray = new byte[stream.Length];
            stream.Read(bytearray, 0, bytearray.Length);
            string text = System.Text.Encoding.UTF8.GetString(bytearray);

            return text;
        }
        #endregion

        #region Update Zip File
        internal void UpdateZip()
        {
            fileOutStream.Seek(0, SeekOrigin.Begin);
            UpdateZip(this.fileOutStream);
        }

        internal void UpdateZip(Stream fileOutStream)
        {
            try
            {
                SaveBackup();
                fileOutStream.Seek(0, SeekOrigin.Begin);

                using (ZipFile zip = ZipFile.Read(Filename))
                {
                    zip.UpdateEntry(fileOutName, fileOutStream);
                    zip.Save();
                    fileOutStream = null;
                }
            } catch (Exception)
            {
            }
        }
        #endregion

        #endregion

        #region Save Backup File
        protected void SaveBackup()
        {
            if (Variables.DoBackup && !Variables.BackupDone)
            {
                if (File.Exists(Variables.BackupName))
                {
                    DialogResult dia = MessageBox.Show("Backup File Already Exists, Replace it?", "Backup Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (dia == DialogResult.Yes)
                    {
                        File.Copy(Variables.Filename, Variables.BackupName, true);
                    }
                } else
                {
                    File.Copy(Variables.Filename, Variables.BackupName);
                }
                Variables.BackupDone = true;
            }
        }
        #endregion

        #region XML
        #region Write XML to Output Stream
        protected void WriteXML()
        {
            XmlWriterSettings xms = new XmlWriterSettings();
            xms.Indent = true;
            xms.OmitXmlDeclaration = false;
            fileOutStream = new MemoryStream();

            using (XmlWriter xml = XmlWriter.Create(fileOutStream, xms))
            {
                XDocument doc = new XDocument(NewTOC);
                doc.WriteTo(xml);
            }

            //Reset Stream Position
            fileOutStream.Seek(0, SeekOrigin.Begin);
        }
        #endregion

        #region Get XML Element
        internal XElement GetXmlElement(Stream stream, string TextToFind)
        {
            try
            {
                string Text = ByteToString(stream);

                if (String.IsNullOrEmpty(Text))
                {
                    //System.Windows.Forms.MessageBox.Show("TOC is empty",Variables.BookName);
                    ns = nsIfEmpty;
                    return null;
                }

                Text = Text.Replace("?xml version=\"1.1\"", "?xml version=\"1.0\"");

                OldTOC = XElement.Parse(Text.Trim());
                ns = OldTOC.Name.Namespace;

                return OldTOC.Elements(ns + TextToFind).FirstOrDefault();
            } catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Invalid File\n" + ex.Message, Variables.BookName);
                ns = nsIfEmpty;
                return null;
            }
        }

        internal XElement GetXmlElement(string TextToFind)
        {
            Stream stream = fileExtractStream;
            return GetXmlElement(stream, TextToFind);
        }
        #endregion
        #endregion


    }
}