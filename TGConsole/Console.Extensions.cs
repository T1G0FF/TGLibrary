using System;
using System.IO;


namespace TGConsole {
	public static class ConsoleFunctions {
		public static string GetValidDirectory() {
			string folderPath;
			do {
				Console.Write("Enter a directory path: ");
				folderPath = Console.ReadLine();
			} while (!Directory.Exists(folderPath));
			return folderPath;
		}

		/// <summary>
		/// Continuously prompts for input until a valid boolean value is given.
		/// </summary>
		/// <param name="prompt"></param>
		/// <param name="YesTrueNoFalse">If true, treats Yes as True and No as False</param>
		/// <returns></returns>
		public static bool GetBool(string prompt = null, bool convertYesTrueNoFalse = false) {
			prompt = prompt ?? "Enter True or False: ";
			if (prompt[prompt.Length - 1] != ' ') prompt += " ";
			bool? result = null;

			while (!result.HasValue) {
				Console.Write(prompt);
				string value = Console.ReadLine();
				if (bool.TryParse(value, out bool parseResult)) {
					result = parseResult;
				}

				if (convertYesTrueNoFalse) {
					switch (value.ToUpper()) {
						case "Y":
						case "YES":
							result = true;
							break;
						case "N":
						case "NO":
							result = false;
							break;
					}
				}
			}

			return result.Value;
		}

		public static int GetInt(string prompt = null) {
			prompt = prompt ?? "Enter an integer: ";
			if (prompt[prompt.Length - 1] != ' ') prompt += " ";
			int? result = null;

			while (!result.HasValue) {
				Console.Write(prompt);
				string value = Console.ReadLine();
				if (int.TryParse(value, out int parseResult)) {
					result = parseResult;
				}
			}

			return result.Value;
		}

		public static double GetDouble(string prompt = null) {
			prompt = prompt ?? "Enter a decimal number: ";
			if (prompt[prompt.Length - 1] != ' ') prompt += " ";
			double? result = null;

			while (!result.HasValue) {
				Console.Write(prompt);
				string value = Console.ReadLine();
				if (double.TryParse(value, out double parseResult)) {
					result = parseResult;
				}
			}

			return result.Value;
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

