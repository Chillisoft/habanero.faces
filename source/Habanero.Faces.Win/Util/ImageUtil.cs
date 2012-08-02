using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Habanero.Faces.Win.Util
{
    class ImageUtil
    {
        public static Image ResizeImage(Image image, int width, int height = -1)
        {
            //a holder for the result
            if (width == -1)
                return image;   // disable scaling
            if (height == -1)
                height = width * (int)Math.Ceiling((double)image.Height / (double)image.Width);
            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            // set the resolutions the same to avoid cropping due to resolution differences
            bmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, bmp.Width, bmp.Height);
                bmp.MakeTransparent(bmp.GetPixel(0, 0));
            }
            //return the resulting bitmap
            return bmp;
        }
    }
}
