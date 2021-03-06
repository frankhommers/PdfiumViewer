﻿using System;

namespace PdfiumViewer
{
  internal class PdfLibrary : IDisposable
  {
    private static readonly object _syncRoot = new object();
    private static PdfLibrary _library;

    private bool _disposed;

    private PdfLibrary()
    {
      try
      {
        NativeMethods.FPDF_InitEmbeddedLibraries();
      }
      catch
      {
        // v8 library not loaded
      }

      NativeMethods.FPDF_InitLibrary();
    }

    public void Dispose()
    {
      Dispose(true);

      GC.SuppressFinalize(this);
    }

    public static void EnsureLoaded()
    {
      lock (_syncRoot)
      {
        if (_library == null)
          _library = new PdfLibrary();
      }
    }

    ~PdfLibrary()
    {
      Dispose(false);
    }

    private void Dispose(bool disposing)
    {
      if (!_disposed)
      {
        NativeMethods.FPDF_DestroyLibrary();

        _disposed = true;
      }
    }
  }
}
