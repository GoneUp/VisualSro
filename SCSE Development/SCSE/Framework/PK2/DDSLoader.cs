using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Framework.PK2
{
    public class DDSLoader
    {
        public static Bitmap LoadDDJ(byte[] buffer)
        {
            if (buffer == null)
            {
                return new Bitmap(256, 256, PixelFormat.Format16bppRgb555);
            }
            Bitmap bitmap;
            using (Stream stream = new MemoryStream(buffer, 20, buffer.GetUpperBound(0) - 0x13))
            {
                FreeImageAPI.FREE_IMAGE_FORMAT format = FreeImageAPI.FREE_IMAGE_FORMAT.FIF_DDS;
                FreeImageAPI.FIBITMAP dib = FreeImageAPI.FreeImage.LoadFromStream(stream, FreeImageAPI.FREE_IMAGE_LOAD_FLAGS.DEFAULT, ref format);
                bitmap = FreeImageAPI.FreeImage.GetBitmap(dib);
                FreeImageAPI.FreeImage.UnloadEx(ref dib);
            }
            return bitmap;
        }
    }
}
