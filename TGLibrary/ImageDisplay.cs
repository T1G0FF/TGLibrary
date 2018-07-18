using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ColourSorting {
    internal static class ImageDisplay {
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        internal static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        internal static void ShowImage(string fileName) {
            var form = new Form {
                BackgroundImage = Image.FromFile(fileName),
                BackgroundImageLayout = ImageLayout.Zoom
            };

            var parent = GetConsoleWindow();
            var child = form.Handle;

            SetParent(child, parent);
            MoveWindow(child, 50, 50, 500, 500, true);

            Application.Run(form);
        }
    }
}
