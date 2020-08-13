namespace PdfiumViewer
{
  public class PdfiumResolver
  {
    public static event PdfiumResolveEventHandler Resolve;

    private static void OnResolve(PdfiumResolveEventArgs e)
    {
      Resolve?.Invoke(null, e);
    }

    internal static string GetPdfiumFileName()
    {
      PdfiumResolveEventArgs e = new PdfiumResolveEventArgs();
      OnResolve(e);
      return e.PdfiumFileName;
    }
  }
}
