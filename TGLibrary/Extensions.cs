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
        }
    }
}
