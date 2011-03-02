using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

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
                if (_Pourcentage>=100)
                {
                    _Pourcentage = 100;
                } else if (_Pourcentage<0)
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
        public ExportTocEventArgs(XElement xml)
        {
            this.XML = new XElement(xml);
        }

        public XElement XML { get; set; }
    }
}
