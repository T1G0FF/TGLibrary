using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace TGLibrary {
    public class ImageDisplayPopup : Form {
        private readonly ZoomPictureBox _pbox;

        public ImageDisplayPopup(string fileName, bool autoOpen = true)
            : this(Image.FromFile(fileName), autoOpen) {
        }

        public ImageDisplayPopup(Image image, bool autoOpen = true)
            : this(image, autoOpen, image.Width, image.Height) {
        }

        public ImageDisplayPopup(Image image, bool autoOpen, int width, int height) {
            //width = width + 0; // SizableToolWindow has no borders
            height = height + 40 + 9; // Height of Toolbar, plus the 9 pixels it's always short? eg Set size 50, size is 41
            width = width > Screen.PrimaryScreen.Bounds.Width ? Screen.PrimaryScreen.Bounds.Width : width;
            height = height > Screen.PrimaryScreen.Bounds.Height ? Screen.PrimaryScreen.Bounds.Height : height;

            this.BackColor = Color.Black;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(width, height);
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.SetStyle(ControlStyles.DoubleBuffer |
                          ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint,
                true);
            this.UpdateStyles();

            _pbox = new ZoomPictureBox() {
                Image = image,
                BackColor = Color.Transparent,
                Location = new Point(0, 0),
                Size = ClientSize, //new Size(width, height)
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };
            this.Controls.Add(_pbox);

            if (autoOpen)
                Application.Run(this); //this.ShowDialog());
        }

        protected override void OnResize(EventArgs e) {
            if (_pbox != null) {
                _pbox.Size = ClientSize;
                //int h = this.ClientSize.Width - pbox.Width;
                //int w = this.ClientSize.Height - pbox.Height;
                //Padding pad = new Padding {
                //    Top = w / 2,
                //    Left = h / 2,
                //};
                //_pbox.Location = new Point(pad.Left, pad.Top);
            }
            base.OnResize(e);
        }
    }
}
