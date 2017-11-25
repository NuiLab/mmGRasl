using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeapMotionGestureTraining.Helper
{
    class ImageHelper
    {

        public static string stringFromImage(Image image)
        {
            byte[] imageArray = byteArrayFromImage(image);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            return base64ImageRepresentation;
        }

        public static Image imageFromString(string base64ImageStr)
        {
            Image img = null;
            try
            {
                img = Image.FromStream(new MemoryStream(Convert.FromBase64String(base64ImageStr)));
            }
            catch (Exception e)
            {
                FileHelper.saveDebugString("ImageFromString : " + base64ImageStr + " \\n" + e.Data.ToString());
            }
            return img;
        }

        public static byte[] byteArrayFromImage(Image image)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(image, typeof(byte[]));
            return xByte;
        }

        public static Bitmap generateBitmapFromLeapImage(Leap.Image image)
        {
            Bitmap bitmap = null;
            try
            {
                bitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format8bppIndexed);
                //set palette
                ColorPalette grayscale = bitmap.Palette;
                for (int i = 0; i < 256; i++)
                {
                    grayscale.Entries[i] = Color.FromArgb((int)255, i, i, i);
                }
                bitmap.Palette = grayscale;
                Rectangle lockArea = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bitmapData = bitmap.LockBits(lockArea, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                byte[] rawImageData = image.Data;
                System.Runtime.InteropServices.Marshal.Copy(rawImageData, 0, bitmapData.Scan0, image.Width * image.Height);
                bitmap.UnlockBits(bitmapData);
            }
            catch (ArgumentException e)
            {
                FileHelper.saveDebugString("Bitmap creation : " + image.ToString() + " \\n" + e.StackTrace);
            }
            
            return bitmap;
        }

    }
}