using System;
using System.Windows.Forms;

namespace PreviewHandlerHost.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void ArquivoCarregarMenuItemClick(
            object sender,
            EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        PreviewHandlerHost.Load(dialog.FileName);
                    }

                    catch (Exception exception)
                    {
                        PreviewHandlerHost.Text = exception.Message;
                    }

                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }
    }
}
