using System;
using System.Drawing;
using System.Windows.Forms;

#pragma warning disable 1591

namespace PdfiumViewer
{
  public class SetCursorEventArgs : EventArgs
  {
    public SetCursorEventArgs(Point location, HitTest hitTest)
    {
      Location = location;
      HitTest = hitTest;
    }

    public Point Location { get; }

    public HitTest HitTest { get; }

    public Cursor Cursor { get; set; }
  }

  public delegate void SetCursorEventHandler(object sender, SetCursorEventArgs e);
}
