using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Habanero.Faces.Win.Util
{
    public class ImageUtil
    {
        public static Image ResizeImage(Image image, int width, int height = -1)
        {
            if (width == -1)
                return image;   // disable scaling
            return ResizeBitmap(new Bitmap(image), width, height);
        }

        public static Bitmap ResizeBitmap(Bitmap image, int width, int height = -1)
        {
            //a holder for the result
            if (width == -1)
                return image;   // disable scaling
            return ScaleBitmap(image, width, height);
        }

        private static Bitmap ScaleBitmap(Bitmap image, int width, int height)
        {
            if (height == -1)
            {
                var scale = (double)image.Height / (double)image.Width;
                height = (int)Math.Ceiling(image.Height * ((double)width / (double)image.Width) * scale);
            }
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
