using System;
using System.IO;

namespace TGLibrary {
    namespace TGConsole {
        public static class ConsoleTG {
            public static string GetValidDirectory() {
                string folderPath;
                do {
                    Console.Write("Enter a directory path: ");
                    folderPath = Console.ReadLine();
                } while (!Directory.Exists(folderPath));
                return folderPath;
            }

            public static void ReplaceLine(string text, Text.Align alignment = Text.Align.Left) {
                int width = Console.WindowWidth - 1;

                switch (alignment) {
                    default:
                    case Text.Align.Left:
                        Console.Write($"\r{text.PadRight(width)}");
                        break;
                    case Text.Align.Right:
                        Console.Write($"\r{text.PadLeft(width)}");
                        break;
                    case Text.Align.Center:
                        Console.Write($"\r{Text.Center(width, text)}");
                        break;
                }       
            }
        }
    }
}
