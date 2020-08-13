using System.ComponentModel;

#pragma warning disable 1591

namespace PdfiumViewer
{
  public class LinkClickEventArgs : HandledEventArgs
  {
    public LinkClickEventArgs(PdfPageLink link)
    {
      Link = link;
    }

    /// <summary>
    ///   Gets the link that was clicked.
    /// </summary>
    public PdfPageLink Link { get; }
  }

  public delegate void LinkClickEventHandler(object sender, LinkClickEventArgs e);
}
