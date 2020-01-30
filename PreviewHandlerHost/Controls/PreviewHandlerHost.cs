using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace PreviewHandlerHost.Controls
{
    public class PreviewHandlerHost
        : Control
    {
        public PreviewHandlerHost()
        {
            Size = new Size(320, 240);
        }

        static readonly Guid PreviewHandlerId = new Guid("8895b1c6-b41f-4c1c-a562-0d564250836f");

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("8895b1c6-b41f-4c1c-a562-0d564250836f")]
        internal interface IPreviewHandler
        {
            void SetWindow(IntPtr hwnd, ref Rectangle rect);
            void SetRect(ref Rectangle rect);
            void DoPreview();
            void Unload();
            void SetFocus();
            void QueryFocus(out IntPtr phwnd);
            [PreserveSig]
            uint TranslateAccelerator(ref Message pmsg);
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("b7d14566-0509-4cce-a71f-0a554233bd9b")]
        internal interface IInitializeWithFile
        {
            void Initialize([MarshalAs(UnmanagedType.LPWStr)] string pszFilePath, uint grfMode);
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("b824b49d-22ac-4161-ac8a-9916e8fa3f7f")]
        internal interface IInitializeWithStream
        {
            void Initialize(IStream pstream, uint grfMode);
        }

        [Flags]
        enum ClassContext : uint
        {
            LocalServer = 0x4
        }

        [DllImport("ole32.dll", CallingConvention = CallingConvention.StdCall)]
        static extern void CoCreateInstance(ref Guid rclsid, IntPtr pUnkOuter, ClassContext dwClsContext, ref Guid riid, out IntPtr ppv);

        public void Load(
            string filename)
        {
            Text = PreviewLoadingText;

            Unload();

            var guid = GetGuid(Path.GetExtension(filename));

            if (guid == Guid.Empty)
            {
                Text = NoPreviewHandlerText;

                return;
            }

            if (guid != _lastGuid)
            {
                if (_previewHandler != null)
                {
                    Unload(true);
                }

                var iid = PreviewHandlerId;

                CoCreateInstance(ref guid, IntPtr.Zero, ClassContext.LocalServer, ref iid, out _previewHandlerPtr);

                if (_previewHandlerPtr == IntPtr.Zero)
                {
                    Text = $@"{CannotCreateClassText} {guid}";

                    return;
                }

                _previewHandler = Marshal.GetUniqueObjectForIUnknown(_previewHandlerPtr);

                _lastGuid = guid;
            }

            if (_previewHandler is IInitializeWithFile iwi)
            {
                iwi.Initialize(filename, 0);
            }

            else if (_previewHandler is IInitializeWithStream iws)
            {
                _stream = new StreamWrapper(File.OpenRead(filename));

                iws.Initialize(_stream, 0);
            }

            if (_previewHandler is IPreviewHandlerVisuals phv)
            {
                phv.SetBackgroundColor(ColorRefFromColor(BackColor));
                phv.SetTextColor(ColorRefFromColor(ForeColor));
            }

            if (_previewHandler is IPreviewHandler ph)
            {
                var r = ClientRectangle;

                ph.SetWindow(Handle, ref r);
                ph.DoPreview();

                OnPreview(new EventArgs());
            }
        }

        [DllImport("Shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern uint AssocQueryString(AssocF flags, AssocStr str, string pszAssoc, string pszExtra, [Out] StringBuilder pszOut, ref uint pcchOut);

        [Flags]
        enum AssocF : uint
        {
            None = 0,
        }

        enum AssocStr
        {
            ShellExtension = 16,
        }

        private Guid GetGuid(
            string extension)
        {
            uint length = 0;

            var result = AssocQueryString(AssocF.None, AssocStr.ShellExtension, extension, $"{{{PreviewHandlerId}}}", null, ref length);

            if (result != 1) return Guid.Empty;

            var stringBuilder = new StringBuilder((int)length);

            result = AssocQueryString(AssocF.None, AssocStr.ShellExtension, extension, $"{{{PreviewHandlerId}}}", stringBuilder, ref length);

            return result == 0
                ? Guid.Parse(stringBuilder.ToString())
                : Guid.Empty;
        }

        public void Unload(
            bool release = false)
        {
            if (_previewHandler == null || !(_previewHandler is IPreviewHandler ph)) return;

            ph.Unload();

            if (_stream != null)
            {
                _stream.Dispose();

                _stream = null;
            }

            if (!release) return;

            Marshal.FinalReleaseComObject(_previewHandler);

            _previewHandler = null;

            if (_previewHandlerPtr != IntPtr.Zero)
            {
                Marshal.Release(_previewHandlerPtr);

                _previewHandlerPtr = IntPtr.Zero;
            }
        }

        protected override void OnResize(
            EventArgs e)
        {
            base.OnResize(e);

            if (_previewHandler is IPreviewHandler ph)
            {
                var r = ClientRectangle;

                ph.SetRect(r);
            }

            Invalidate();
        }

        protected override void OnPaint(
            PaintEventArgs e)
        {
            base.OnPaint(e);

            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                ClientRectangle,
                ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis
            );
        }

        [ComImport]
        [Guid("196bf9a5-b346-4ef0-aa1e-5dcdb76768b1")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IPreviewHandlerVisuals
        {
            [PreserveSig]
            void SetBackgroundColor(uint color);
            [PreserveSig]
            void SetFont(ref IntPtr plf);
            [PreserveSig]
            void SetTextColor(uint color);
        }

        static uint ColorRefFromColor(Color color)
        {
            return ((uint)color.B << 16) | ((uint)color.G << 8) | color.R;
        }

        protected override void Dispose(
            bool disposing)
        {
            Unload(true);

            base.Dispose(disposing);
        }

        [Category("Text")]
        [Description("Text properties")]
        [DefaultValue("Select a file")]
        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;

                Invalidate();
            }
        }

        [Category("Text")]
        [DefaultValue("Preview is loading...")]
        public string PreviewLoadingText { get; set; } = "Preview is loading...";

        [Category("Text")]
        [DefaultValue("Cannot create class")]
        public string CannotCreateClassText { get; set; } = "Cannot create class";

        [Category("Text")]
        [DefaultValue("No preview handler is associated with this file type.")]
        public string NoPreviewHandlerText { get; set; } = "No preview handler is associated with this file type.";

        public event EventHandler Preview;

        protected virtual void OnPreview(
            EventArgs e)
        {
            var handler = Preview;

            handler?.Invoke(this, e);
        }

        private object _previewHandler;
        private IntPtr _previewHandlerPtr;
        private StreamWrapper _stream;
        private Guid _lastGuid;
    }
}
