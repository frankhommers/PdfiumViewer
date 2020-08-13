#pragma warning disable 1591

namespace PdfiumViewer
{
  public class PdfMatch
  {
    public PdfMatch(string text, PdfTextSpan textSpan, int page)
    {
      Text = text;
      TextSpan = textSpan;
      Page = page;
    }

    public string Text { get; }
    public PdfTextSpan TextSpan { get; }
    public int Page { get; }
  }
}
