using System;
using System.Text;

namespace TGConsole {
	public class Draw {
		public static string Line(char line) {
			return Line(Config.ConsoleWidth, line);
		}

		public static string Line(char ends, char line) {
			return Line(Config.ConsoleWidth, ends, line, ends);
		}

		public static string Line(char leftEnd, char line, char rightEnd) {
			return Line(Config.ConsoleWidth, leftEnd, line, rightEnd);
		}

		public static string Line(int width, char line) {
			return new string(line, width);
		}

		public static string Line(int width, char ends, char line) {
			return Line(width, ends, line, ends);
		}

		public static string Line(int width, char leftEnd, char line, char rightEnd) {
			StringBuilder sb = new StringBuilder();
			sb.Append(leftEnd);
			sb.Append(Line(width - 2, line));
			sb.Append(rightEnd);
			return sb.ToString();
		}
	}
}

