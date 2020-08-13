# PdfiumViewer

Apache 2.0 License.

## Introduction

PdfiumViewer is a PDF viewer based on the PDFium project.

PdfiumViewer provides a number of components to work with PDF files:

* PdfDocument is the base class used to render PDF documents;

* PdfRenderer is a WinForms control that can render a PdfDocument;

* PdfiumViewer is a WinForms control that hosts a PdfRenderer control and
  adds a toolbar to save the PDF file or print it.

## Compatibility

The PdfiumViewer library has been tested with Windows 10 and is fully compatible.
However, the native PDFium libraries with V8 support do not support Windows XP.
See below for instructions on how to reference the native libraries.

## Using the library

The PdfiumViewer control requires native PDFium libraries. These libraries are not included.

This version of the PdfiumViewer has been modified to use [different pre-built binaries which available here](https://github.com/bblanchon/pdfium-binaries)

## Note on the `PdfViewer` control

The PdfiumViewer library primarily consists out of three components:

* The `PdfViewer` control. This control provides a host for the `PdfRenderer`
  control and has a default toolbar with limited functionality;
* The `PdfRenderer` control. This control implements the raw PDF renderer.
  This control displays a PDF document, provides zooming and scrolling
  functionality and exposes methods to perform more advanced actions;
* The `PdfDocument` class provides access to the PDF document and wraps
  the Pdfium library.

The `PdfViewer` control should only be used when you have a very simple use
case and where the buttons available on the toolbar provide enough functionality
for you. This toolbar will not be extended with new buttons or with functionality
to hide buttons. The reason for this is that the `PdfViewer` control is just
meant to get you started. If you need more advanced functionality, you should
create your own control with your own toolbar, e.g. by starting out with
the `PdfViewer` control. Also, because people currently are already using the
`PdfViewer` control, adding more functionality to this toolbar would be
a breaking change. See [issue #41](https://github.com/pvginkel/PdfiumViewer/issues/41)
for more information.