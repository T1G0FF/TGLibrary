using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace TGLibrary {
    public static class FileHelper {
        public static void Zip(string dirZip, string dirOut) {
            string fileName = dirOut + ".cbz";
            string fileNameFast = fileName + ".fst";
            string fileNameOptm = fileName + ".opt";
            FileInfo fileFast = null;
            FileInfo fileOptm = null;

            Task[] tasks = new Task[2];

            tasks[0] = Task.Factory.StartNew(() => {
                ZipFile.CreateFromDirectory(dirZip, fileNameFast, CompressionLevel.Fastest, false);
                fileFast = new FileInfo(fileNameFast);
            });

            tasks[1] = Task.Factory.StartNew(() => {
                ZipFile.CreateFromDirectory(dirZip, fileNameOptm, CompressionLevel.Optimal, false);
                fileOptm = new FileInfo(fileNameOptm);
            });

            Task.WaitAll(tasks);

            if (File.Exists(fileName) == false) {
                // Keeps only the smaller of the 2 files
                if (fileFast.Length > fileOptm.Length) {
                    File.Delete(fileNameOptm);
                    File.Move(fileNameFast, fileName);
                }
                else {
                    File.Delete(fileNameFast);
                    File.Move(fileNameOptm, fileName);
                }
            }
            else {
                Console.WriteLine("{0} already exists", fileName);
                if (File.Exists(fileNameFast))
                    File.Delete(fileNameFast);
                if (File.Exists(fileNameOptm))
                    File.Delete(fileNameOptm);
            }
        }

        public static void RemoveDirectory(string targetDir) {
            File.SetAttributes(targetDir, FileAttributes.Normal);

            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files) {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs) {
                RemoveDirectory(dir);
            }

            Directory.Delete(targetDir, false);
        }

        public static string SafeCreateUniqueDirectory(string folderPath) {
            if (Directory.Exists(folderPath)) {
                folderPath = folderPath + "_" + getUniqueString() + Path.DirectorySeparatorChar;
            }
            Directory.CreateDirectory(folderPath);
            return folderPath;
        }

        public static string SafeCreateDirectory(string folderPath) {
            if (!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }
            return folderPath + Path.DirectorySeparatorChar;
        }

        public static string GetUniqueFileName(string fileName, string fileExtension) {
            return $"{fileName}_{getUniqueString()}.{fileExtension}";
        }

        internal static string getUniqueString() {
            return Guid.NewGuid().ToString().ToUpper();
            //DateTime.Now.Ticks.GetHashCode().ToString("x").ToUpper()
            //DateTime.Now.ToString("yyyy-MM-dd-HHmmssfff")
        }

        public static string GetDesktopFolderPath(string newFolderName) {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                                            + Path.DirectorySeparatorChar
                                            + newFolderName
                                            + Path.DirectorySeparatorChar;
            if (!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }
    }
}
