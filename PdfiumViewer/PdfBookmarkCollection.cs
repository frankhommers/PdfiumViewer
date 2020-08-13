using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace PdfiumViewer
{
  public class PdfBookmark
  {
    public PdfBookmark()
    {
      Children = new PdfBookmarkCollection();
    }

    public string Title { get; set; }
    public int PageIndex { get; set; }

    public PdfBookmarkCollection Children { get; }

    public override string ToString()
    {
      return Title;
    }
  }

  public class PdfBookmarkCollection : Collection<PdfBookmark>
  {
  }
}
