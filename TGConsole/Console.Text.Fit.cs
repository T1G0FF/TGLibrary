using System;
using System.Text;

namespace TGConsole {
	/// <remarks>
	/// Contains methods for manipulating text.
	/// </remarks>
	public partial class Text {
		public static string Fit(string text) { return Fit(Config.DefaultWidth, text, ' ', ' ', ' ', Center); }
		public static string FitLeft(string text) { return Fit(Config.DefaultWidth, text, ' ', ' ', ' ', Left); }
		public static string FitCenter(string text) { return Fit(Config.DefaultWidth, text, ' ', ' ', ' ', Center); }
		public static string FitRight(string text) { return Fit(Config.DefaultWidth, text, ' ', ' ', ' ', Right); }

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
			while (text.Length > rowWidth) {
				string row = GetRow(rowWidth, ref text);

				sb.Append(AlignText(width, row, leftEnd, line, rightEnd));
				if (width < Console.WindowWidth) {
					sb.Append(Environment.NewLine);
				}
			}
			sb.Append(AlignText(width, text, leftEnd, line, rightEnd));
			return sb.ToString();
		}

		private static readonly char[] splitChars = { ' ', ',', '.' };
		private static string GetRow(int width, ref string text) {
			string row;
			string slice = text.Substring(0, width);

			// Get split closest to end for chars.
			int splitBest = -1;
			foreach (char c in splitChars) {
				int curr = slice.LastIndexOf(c);
				splitBest = curr > splitBest ? curr : splitBest;
			}

			int splitNewLine = slice.IndexOf('\n');
			int splitEnvNewLine = slice.IndexOf(Environment.NewLine);
			// Get split closest to start for new lines.
			// If a new row character is found and it appears before where we want to split, keep the newline
			if ((splitEnvNewLine > -1) && (splitEnvNewLine < splitNewLine)) {
				splitNewLine = splitEnvNewLine;
			}
			if ((splitNewLine > -1) && (splitNewLine < splitBest)) {
				splitBest = splitNewLine;
			}

			row = text.Substring(0, splitBest + 1);
			text = text.Substring(splitBest + 1);

			return row.Trim();
		}
	}
}

