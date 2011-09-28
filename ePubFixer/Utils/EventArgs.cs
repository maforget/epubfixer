using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;

namespace ePubFixer
{
    public class ExtractProgressArgs : EventArgs
    {
        private int _Pourcentage;
        private long _Transferred;
        private long _TotalToTransfer;

        public long Transferred
        {
            get { return _Transferred; }
            set
            {
                _Transferred = value;
                _Pourcentage = Convert.ToInt32((100 * _Transferred / _TotalToTransfer));
                if (_Pourcentage >= 100)
                {
                    _Pourcentage = 100;
                } else if (_Pourcentage < 0)
                {
                    _Pourcentage = 0;
                }
            }
        }

        public long TotalToTransfer
        {
            get { return _TotalToTransfer; }
            set { _TotalToTransfer = value; }
        }


        public int Pourcentage
        {
            get { return _Pourcentage; }
        }


        public ExtractProgressArgs()
        {
            _Pourcentage = 0;
            _Transferred = 0;
            _TotalToTransfer = 0;
        }
    }

    public class FilesAddedArgs : EventArgs
    {
        public FilesAddedArgs(List<NavDetails> files)
        {
            Files = files;
        }

        public List<NavDetails> Files { get; set; }
    }

    public class ExportTocEventArgs : EventArgs
    {
        public ExportTocEventArgs(XElement xml,bool createHtmlTOC=false)
        {
            this.XML = new XElement(xml);
            this.Message = string.Empty;
            this.CreateHtmlTOC = createHtmlTOC;
        }

        public bool CreateHtmlTOC { get; set; }
        public XElement XML { get; set; }
        public string Message { get; set; }
    }
    public class CoverChangedArgs : EventArgs
    {
        public Image Cover { get; set; }
        public string Message { get; set; }
        public bool ChangedCoverFile { get; set; }
        public int Heigth { get; set; }
        public int Width { get; set; }
        public bool PreserveAspectRatio { get; set; }

        public CoverChangedArgs(Image cover, bool PreserveRatio)
        {
            this.Cover = cover;
            this.Message = string.Empty;
            this.ChangedCoverFile = false;
            if (cover!=null)
            {
                this.Heigth = cover.Height;
                this.Width = cover.Width; 
            }
            this.PreserveAspectRatio = PreserveRatio;
        }

        public CoverChangedArgs(Image cover)
            : this(cover, true)
        {
        }

    }
}
