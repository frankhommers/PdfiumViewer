using System;
using System.Windows.Forms;

namespace PdfiumViewer
{
  internal partial class PasswordForm : Form
  {
    public PasswordForm()
    {
      InitializeComponent();

      UpdateEnabled();
    }

    public string Password
    {
      get { return _password.Text; }
    }

    private void _password_TextChanged(object sender, EventArgs e)
    {
      UpdateEnabled();
    }

    private void UpdateEnabled()
    {
      _acceptButton.Enabled = _password.Text.Length > 0;
    }

    private void _acceptButton_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
    }
  }
}
