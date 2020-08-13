using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using MessageBox = System.Windows.MessageBox;

namespace PdfiumViewer.WPFDemo
{
  /// <summary>
  ///   Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private readonly Process currentProcess = Process.GetCurrentProcess();
    private PdfDocument pdfDoc;
    private CancellationTokenSource tokenSource;

    public MainWindow()
    {
      InitializeComponent();
    }

    private async void RenderToMemDCButton_Click(object sender, RoutedEventArgs e)
    {
      if (pdfDoc == null)
      {
        MessageBox.Show("First load the document");
        return;
      }

      int width = (int) (ActualWidth - 30) / 2;
      int height = (int) ActualHeight - 30;

      Stopwatch sw = new Stopwatch();
      sw.Start();

      try
      {
        for (int i = 1; i < pdfDoc.PageCount; i++)
        {
          imageMemDC.Source =
            await
              Task.Run(
                () =>
                {
                  tokenSource.Token.ThrowIfCancellationRequested();

                  return RenderPageToMemDC(i, width, height);
                }, tokenSource.Token);

          labelMemDC.Content = string.Format("Renderd Pages: {0}, Memory: {1} MB, Time: {2:0.0} sec",
            i,
            currentProcess.PrivateMemorySize64 / (1024 * 1024),
            sw.Elapsed.TotalSeconds);

          currentProcess.Refresh();

          GC.Collect();
        }
      }
      catch (Exception ex)
      {
        tokenSource.Cancel();
        MessageBox.Show(ex.Message);
      }

      sw.Stop();
      labelMemDC.Content = string.Format("Rendered {0} Pages within {1:0.0} seconds, Memory: {2} MB",
        pdfDoc.PageCount,
        sw.Elapsed.TotalSeconds,
        currentProcess.PrivateMemorySize64 / (1024 * 1024));
    }

    private BitmapSource RenderPageToMemDC(int page, int width, int height)
    {
      Image image = pdfDoc.Render(page, width, height, 96, 96, false);
      return BitmapHelper.ToBitmapSource(image);
    }

    private void LoadPDFButton_Click(object sender, RoutedEventArgs e)
    {
      using (OpenFileDialog dialog = new OpenFileDialog())
      {
        dialog.Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
        dialog.Title = "Open PDF File";

        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) pdfDoc = PdfDocument.Load(dialog.FileName);
      }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      tokenSource = new CancellationTokenSource();
    }

    private void Window_Closed(object sender, EventArgs e)
    {
      if (tokenSource != null)
        tokenSource.Cancel();

      if (pdfDoc != null)
        pdfDoc.Dispose();
    }

    private void DoSearch_Click(object sender, RoutedEventArgs e)
    {
      string text = searchValueTextBox.Text;
      bool matchCase = matchCaseCheckBox.IsChecked.GetValueOrDefault();
      bool wholeWordOnly = wholeWordOnlyCheckBox.IsChecked.GetValueOrDefault();

      DoSearch(text, matchCase, wholeWordOnly);
    }

    private void DoSearch(string text, bool matchCase, bool wholeWord)
    {
      PdfMatches matches = pdfDoc.Search(text, matchCase, wholeWord);
      StringBuilder sb = new StringBuilder();

      foreach (PdfMatch match in matches.Items)
        sb.AppendLine(
          string.Format(
            "Found \"{0}\" in page: {1}", match.Text, match.Page)
        );

      searchResultLabel.Text = sb.ToString();
    }
  }
}
