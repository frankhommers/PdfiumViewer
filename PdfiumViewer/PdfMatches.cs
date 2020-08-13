using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace PdfiumViewer
{
  public class PdfMatches
  {
    public PdfMatches(int startPage, int endPage, IList<PdfMatch> matches)
    {
      if (matches == null)
        throw new ArgumentNullException("matches");

      StartPage = startPage;
      EndPage = endPage;
      Items = new ReadOnlyCollection<PdfMatch>(matches);
    }

    public int StartPage { get; }

    public int EndPage { get; }

    public IList<PdfMatch> Items { get; }
  }
}
