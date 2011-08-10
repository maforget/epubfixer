using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ePubFixer
{
    public static class Extensions
    {
        public static Stream ToStream(this Image image, ImageFormat formaw)
        {
            var stream = new System.IO.MemoryStream();
            image.Save(stream, formaw);
            stream.Position = 0;
            return stream;
        }

        public static Stream ToStream(this string text)
        {
            Stream str = new MemoryStream();
            StreamWriter writer = new StreamWriter(str, System.Text.Encoding.UTF8);
            writer.Write(text);
            writer.Flush();
            str.Seek(0, SeekOrigin.Begin);
            return str;
        }

    }
}
