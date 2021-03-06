﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;

namespace PdfiumViewer.Demo
{
  public class PdfRangeDocument : IPdfDocument
  {
    private readonly IPdfDocument _document;
    private readonly int _endPage;
    private readonly int _startPage;
    private PdfBookmarkCollection _bookmarks;
    private IList<SizeF> _sizes;

    private PdfRangeDocument(IPdfDocument document, int startPage, int endPage)
    {
      _document = document;
      _startPage = startPage;
      _endPage = endPage;
    }

    public int PageCount
    {
      get { return _endPage - _startPage + 1; }
    }

    public PdfBookmarkCollection Bookmarks
    {
      get
      {
        if (_bookmarks == null)
          _bookmarks = TranslateBookmarks(_document.Bookmarks);
        return _bookmarks;
      }
    }

    public IList<SizeF> PageSizes
    {
      get
      {
        if (_sizes == null)
          _sizes = TranslateSizes(_document.PageSizes);
        return _sizes;
      }
    }

    public void Render(int page, Graphics graphics, float dpiX, float dpiY, Rectangle bounds, bool forPrinting)
    {
      _document.Render(TranslatePage(page), graphics, dpiX, dpiY, bounds, forPrinting);
    }

    public void Render(int page, Graphics graphics, float dpiX, float dpiY, Rectangle bounds, PdfRenderFlags flags)
    {
      _document.Render(TranslatePage(page), graphics, dpiX, dpiY, bounds, flags);
    }

    public Image Render(int page, float dpiX, float dpiY, bool forPrinting)
    {
      return _document.Render(TranslatePage(page), dpiX, dpiY, forPrinting);
    }

    public Image Render(int page, float dpiX, float dpiY, PdfRenderFlags flags)
    {
      return _document.Render(TranslatePage(page), dpiX, dpiY, flags);
    }

    public Image Render(int page, int width, int height, float dpiX, float dpiY, bool forPrinting)
    {
      return _document.Render(TranslatePage(page), width, height, dpiX, dpiY, forPrinting);
    }

    public Image Render(int page, int width, int height, float dpiX, float dpiY, PdfRenderFlags flags)
    {
      return _document.Render(TranslatePage(page), width, height, dpiX, dpiY, flags);
    }

    public Image Render(int page, int width, int height, float dpiX, float dpiY, PdfRotation rotate,
      PdfRenderFlags flags)
    {
      return _document.Render(page, width, height, dpiX, dpiY, rotate, flags);
    }

    public void Save(string path)
    {
      _document.Save(path);
    }

    public void Save(Stream stream)
    {
      _document.Save(stream);
    }

    public PdfMatches Search(string text, bool matchCase, bool wholeWord)
    {
      return TranslateMatches(_document.Search(text, matchCase, wholeWord));
    }

    public PdfMatches Search(string text, bool matchCase, bool wholeWord, int page)
    {
      return TranslateMatches(_document.Search(text, matchCase, wholeWord, page));
    }

    public PdfMatches Search(string text, bool matchCase, bool wholeWord, int startPage, int endPage)
    {
      return TranslateMatches(_document.Search(text, matchCase, wholeWord, startPage, endPage));
    }

    public PrintDocument CreatePrintDocument()
    {
      return _document.CreatePrintDocument();
    }

    public PrintDocument CreatePrintDocument(PdfPrintMode printMode)
    {
      return _document.CreatePrintDocument(printMode);
    }

    public PrintDocument CreatePrintDocument(PdfPrintSettings settings)
    {
      return _document.CreatePrintDocument(settings);
    }

    public PdfPageLinks GetPageLinks(int pageNumber, Size pageSize)
    {
      return TranslateLinks(_document.GetPageLinks(pageNumber + _startPage, pageSize));
    }

    public void DeletePage(int pageNumber)
    {
      _document.DeletePage(TranslatePage(pageNumber));
    }

    public void RotatePage(int pageNumber, PdfRotation rotation)
    {
      _document.RotatePage(TranslatePage(pageNumber), rotation);
    }

    public PdfInformation GetInformation()
    {
      return _document.GetInformation();
    }

    public string GetPdfText(int page)
    {
      return _document.GetPdfText(TranslatePage(page));
    }

    public string GetPdfText(PdfTextSpan textSpan)
    {
      return _document.GetPdfText(textSpan);
    }

    public IList<PdfRectangle> GetTextBounds(PdfTextSpan textSpan)
    {
      List<PdfRectangle> result = new List<PdfRectangle>();

      foreach (PdfRectangle rectangle in _document.GetTextBounds(textSpan))
        result.Add(new PdfRectangle(
          rectangle.Page + _startPage,
          rectangle.Bounds
        ));

      return result;
    }

    public PointF PointToPdf(int page, Point point)
    {
      return _document.PointToPdf(TranslatePage(page), point);
    }

    public Point PointFromPdf(int page, PointF point)
    {
      return _document.PointFromPdf(TranslatePage(page), point);
    }

    public RectangleF RectangleToPdf(int page, Rectangle rect)
    {
      return _document.RectangleToPdf(TranslatePage(page), rect);
    }

    public Rectangle RectangleFromPdf(int page, RectangleF rect)
    {
      return _document.RectangleFromPdf(TranslatePage(page), rect);
    }

    public int GetCharacterIndexAtPosition(PdfPoint location, double xTolerance, double yTolerance)
    {
      return _document.GetCharacterIndexAtPosition(location, xTolerance, yTolerance);
    }

    public bool GetWordAtPosition(PdfPoint location, double xTolerance, double yTolerance, out PdfTextSpan span)
    {
      return _document.GetWordAtPosition(location, xTolerance, yTolerance, out span);
    }

    public int CountCharacters(int page)
    {
      return _document.CountCharacters(page);
    }

    public List<PdfRectangle> GetTextRectangles(int page, int startIndex, int count)
    {
      return _document.GetTextRectangles(page, startIndex, count);
    }

    public void Dispose()
    {
      _document.Dispose();
    }

    public static PdfRangeDocument FromDocument(IPdfDocument document, int startPage, int endPage)
    {
      if (document == null)
        throw new ArgumentNullException(nameof(document));

      if (endPage < startPage)
        throw new ArgumentException("End page cannot be less than start page");
      if (startPage < 0)
        throw new ArgumentException("Start page cannot be less than zero");
      if (endPage >= document.PageCount)
        throw new ArgumentException("End page cannot be more than the number of pages in the document");

      return new PdfRangeDocument(
        document,
        startPage,
        endPage
      );
    }

    private PdfBookmarkCollection TranslateBookmarks(PdfBookmarkCollection bookmarks)
    {
      PdfBookmarkCollection result = new PdfBookmarkCollection();

      TranslateBookmarks(result, bookmarks);

      return result;
    }

    private void TranslateBookmarks(PdfBookmarkCollection result, PdfBookmarkCollection bookmarks)
    {
      foreach (PdfBookmark bookmark in bookmarks)
        if (bookmark.PageIndex >= _startPage && bookmark.PageIndex <= _endPage)
        {
          PdfBookmark resultBookmark = new PdfBookmark
          {
            PageIndex = bookmark.PageIndex - _startPage,
            Title = bookmark.Title
          };

          TranslateBookmarks(resultBookmark.Children, bookmark.Children);

          result.Add(resultBookmark);
        }
    }

    private IList<SizeF> TranslateSizes(IList<SizeF> pageSizes)
    {
      List<SizeF> result = new List<SizeF>();

      for (int i = _startPage; i <= _endPage; i++) result.Add(pageSizes[i]);

      return result;
    }

    private PdfMatches TranslateMatches(PdfMatches search)
    {
      if (search == null)
        return null;

      List<PdfMatch> matches = new List<PdfMatch>();

      foreach (PdfMatch match in search.Items)
        matches.Add(new PdfMatch(
          match.Text,
          new PdfTextSpan(match.TextSpan.Page + _startPage, match.TextSpan.Offset, match.TextSpan.Length),
          match.Page + _startPage
        ));

      return new PdfMatches(
        search.StartPage + _startPage,
        search.EndPage + _startPage,
        matches
      );
    }

    private PdfPageLinks TranslateLinks(PdfPageLinks pageLinks)
    {
      if (pageLinks == null)
        return null;

      List<PdfPageLink> links = new List<PdfPageLink>();

      foreach (PdfPageLink link in pageLinks.Links)
        links.Add(new PdfPageLink(
          link.Bounds,
          link.TargetPage + _startPage,
          link.Uri
        ));

      return new PdfPageLinks(links);
    }

    private int TranslatePage(int page)
    {
      if (page < 0 || page >= PageCount)
        throw new ArgumentException("Page number out of range");
      return page + _startPage;
    }
  }
}
