using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace TGLibrary {
	public static class FileHelper {
		public static void Zip(string dirZip, string dirOut) {
			string fileName = dirOut + ".zip";
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

		/// <summary>
		/// Deletes the specified directory and any subdirectories and files in the directory
		/// </summary>
		/// <param name="targetPath"></param>
		public static void RemoveDirectory(string targetPath) {
			File.SetAttributes(targetPath, FileAttributes.Normal);

			string[] files = Directory.GetFiles(targetPath);
			string[] dirs = Directory.GetDirectories(targetPath);

			foreach (string file in files) {
				File.SetAttributes(file, FileAttributes.Normal);
				File.Delete(file);
			}

			foreach (string dir in dirs) {
				if (Directory.Exists(dir))
					RemoveDirectory(dir);
			}

			Directory.Delete(targetPath, false);
		}

		public static string SafeCreateUniqueDirectory(string folderPath) {
			if (Directory.Exists(folderPath)) {
				folderPath = folderPath + "_" + GetUniqueString() + Path.DirectorySeparatorChar;
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
			return $"{fileName}_{GetUniqueString()}.{fileExtension}";
		}

		internal static string GetUniqueString() {
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

		public static void CreateEmptyFile(string dirPath, string fileName) {
			File.Create(dirPath + fileName).Close();
		}

		public static void SafeExtract(string zipPath, string extractPath) {
			// Normalizes the path.
			extractPath = Path.GetFullPath(extractPath);

			// Ensures that the last character on the extraction path
			// is the directory separator char. 
			// Without this, a malicious zip file could try to traverse outside of the expected
			// extraction path.
			if (!extractPath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
				extractPath += Path.DirectorySeparatorChar;

			using (ZipArchive archive = ZipFile.OpenRead(zipPath)) {
				foreach (ZipArchiveEntry entry in archive.Entries) {
					// Gets the full path to ensure that relative segments are removed.
					string destinationPath = Path.GetFullPath(Path.Combine(extractPath, entry.FullName));

					// Ordinal match is safest, case-sensitive volumes can be mounted within volumes that
					// are case-insensitive.
					if (destinationPath.StartsWith(extractPath, StringComparison.Ordinal)) {
						if (!Directory.Exists(extractPath)) Directory.CreateDirectory(extractPath);
						entry.ExtractToFile(destinationPath);
					}
				}
			}
		}
	}
}
