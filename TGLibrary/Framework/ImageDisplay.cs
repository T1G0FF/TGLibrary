using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TGLibrary {
    public static class ImageDisplay {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        public static void ShowImage(string fileName, bool ownWindow = true) {
            ShowImage(Image.FromFile(fileName), ownWindow);
        }

        public static void ShowImage(Image image, bool ownWindow = true) {
            ShowImage(image, ownWindow, 0, 0, image.Width, image.Height);
        }

        public static void ShowImage(Image image, bool ownWindow, int x, int y, int width, int height) {
            var form = new Form {
                BackgroundImage = image,
                BackgroundImageLayout = ImageLayout.Zoom,
                FormBorderStyle = ownWindow ? FormBorderStyle.FixedToolWindow : FormBorderStyle.None
            };

            var parent = GetConsoleWindow();
            var child = form.Handle;

            if (!ownWindow)
                SetParent(child, parent); // Draws inside Console window, bit finicky
            MoveWindow(child, x, y, width, height, true);

            Application.Run(form);
        }
    }
}
