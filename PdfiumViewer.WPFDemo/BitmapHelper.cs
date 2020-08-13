using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PdfiumViewer.WPFDemo
{
  internal class BitmapHelper
  {
    public static BitmapSource ToBitmapSource(Image image)
    {
      return ToBitmapSource(image as Bitmap);
    }

    /// <summary>
    ///   Convert an IImage to a WPF BitmapSource. The result can be used in the Set Property of Image.Source
    /// </summary>
    /// <param name="bitmap">The Source Bitmap</param>
    /// <returns>The equivalent BitmapSource</returns>
    public static BitmapSource ToBitmapSource(Bitmap bitmap)
    {
      if (bitmap == null) return null;

      using (Bitmap source = (Bitmap) bitmap.Clone())
      {
        IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap

        BitmapSource bs = Imaging.CreateBitmapSourceFromHBitmap(
          ptr,
          IntPtr.Zero,
          Int32Rect.Empty,
          BitmapSizeOptions.FromEmptyOptions());

        NativeMethods.DeleteObject(ptr); //release the HBitmap
        bs.Freeze();
        return bs;
      }
    }

    public static BitmapSource ToBitmapSource(byte[] bytes, int width, int height, int dpiX, int dpiY)
    {
      BitmapSource result = BitmapSource.Create(
        width,
        height,
        dpiX,
        dpiY,
        PixelFormats.Bgra32,
        null /* palette */,
        bytes,
        width * 4 /* stride */);
      result.Freeze();

      return result;
    }
  }
}
