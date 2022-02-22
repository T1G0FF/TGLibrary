using System;
using System.Text;

namespace TGConsole {
	/// <remarks>
	/// Contains methods for manipulating text.
	/// </remarks>
	public partial class Text {
		public static string Fit(string text) { return Fit(Config.ConsoleWidth, text, ' ', ' ', ' ', Center); }
		public static string FitLeft(string text) { return Fit(Config.ConsoleWidth, text, ' ', ' ', ' ', Left); }
		public static string FitCenter(string text) { return Fit(Config.ConsoleWidth, text, ' ', ' ', ' ', Center); }
		public static string FitRight(string text) { return Fit(Config.ConsoleWidth, text, ' ', ' ', ' ', Right); }

		public static string Fit(int width, string text) { return Fit(width, text, ' ', ' ', ' ', Center); }
		public static string FitLeft(int width, string text) { return Fit(width, text, ' ', ' ', ' ', Left); }
		public static string FitCenter(int width, string text) { return Fit(width, text, ' ', ' ', ' ', Center); }
		public static string FitRight(int width, string text) { return Fit(width, text, ' ', ' ', ' ', Right); }

		public static string Fit(int width, string text, char ends) { return Fit(width, text, ends, ' ', ends, Center); }
		public static string FitLeft(int width, string text, char ends) { return Fit(width, text, ends, ' ', ends, Left); }
		public static string FitCenter(int width, string text, char ends) { return Fit(width, text, ends, ' ', ends, Center); }
		public static string FitRight(int width, string text, char ends) { return Fit(width, text, ends, ' ', ends, Right); }

		public static string Fit(int width, string text, char ends, char line) { return Fit(width, text, ends, line, ends, Center); }
		public static string FitLeft(int width, string text, char ends, char line) { return Fit(width, text, ends, line, ends, Left); }
		public static string FitCenter(int width, string text, char ends, char line) { return Fit(width, text, ends, line, ends, Center); }
		public static string FitRight(int width, string text, char ends, char line) { return Fit(width, text, ends, line, ends, Right); }

		public static string Fit(int width, string text, char leftEnd, char line, char rightEnd) { return Fit(width, text, leftEnd, line, rightEnd, Center); }
		public static string FitLeft(int width, string text, char leftEnd, char line, char rightEnd) { return Fit(width, text, leftEnd, line, rightEnd, Left); }
		public static string FitCenter(int width, string text, char leftEnd, char line, char rightEnd) { return Fit(width, text, leftEnd, line, rightEnd, Center); }
		public static string FitRight(int width, string text, char leftEnd, char line, char rightEnd) { return Fit(width, text, leftEnd, line, rightEnd, Right); }

		public static string Fit(int width, string text, char leftEnd, char line, char rightEnd, Func<int, string, char, char, char, string> AlignText) {
			// 4 in total = 2 Ends and 2 Spaces
			int rowWidth = width - 4;
			StringBuilder sb = new StringBuilder();

			text = text.Replace("\t", new string(' ', 4));

			while (text.Length > rowWidth || text.Split('\n').Length > 1) {
				string row = GetRow(rowWidth, ref text);

				sb.Append(AlignText(width, row, leftEnd, line, rightEnd));
				if (!ConsoleFunctions.ConsolePresent || (ConsoleFunctions.ConsolePresent && width < Console.WindowWidth)) {
					sb.Append(Environment.NewLine);
				}
			}
			sb.Append(AlignText(width, text, leftEnd, line, rightEnd));

			return sb.ToString().TrimEnd('\r', '\n');
		}

		private static readonly char[] splitChars = { ' ', ',', '.' };
		private static string GetRow(int width, ref string text) {
			string row;
			int max = width < text.Length ? width : text.Length;
			string slice = text.Substring(0, max);

			// Get split closest to end for chars.
			int splitBest = slice.LastIndexOfAny(splitChars, max - 1);

			int splitStart = 1; // Include character in row, unless it's a newline character.
			int splitWidth = 1; // Environment.Newline could be 2 chars wide "\r\n"
			int splitNewLine = slice.IndexOf('\n');
			int splitEnvNewLine = slice.IndexOf(Environment.NewLine);
			// Get split closest to start for new lines.
			// If a new row character is found and it appears before where we want to split, keep the newline
			if ((splitEnvNewLine > -1) && (splitEnvNewLine < splitNewLine)) {
				splitNewLine = splitEnvNewLine;
				splitWidth = Environment.NewLine.Length;
			}
			if ((splitNewLine > -1) && (splitNewLine < splitBest)) {
				splitBest = splitNewLine;
				splitStart = 0;
			}

			row = text.Substring(0, splitBest + splitStart);
			text = text.Substring(splitBest + splitWidth);

			return row.Trim();
		}
	}
}

