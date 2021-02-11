using System;
using System.Text;

namespace TGConsole {
	public class Alert {
		#region Public Methods
		public static string Title(string text, int width = -1) {
			width = (width > 0) ? width : Config.ConsoleWidth;
			string topLine = Draw.Line(width, '╔', '═', '╗') + Environment.NewLine;
			string midLine = Text.FitCenter(width, text, '║', ' ');
			string btmLine = Draw.Line(width, '╚', '═', '╝');
			return AlertBuilder(topLine, midLine, btmLine);
		}

		public static string Message(string text, int width = -1) {
			width = (width > 0) ? width : Config.ConsoleWidth;
			string topLine = Draw.Line(width, '┌', '─', '┐') + Environment.NewLine;
			string midLine = Text.FitCenter(width, text, '│');
			string btmLine = Draw.Line(width, '└', '─', '┘');
			return AlertBuilder(topLine, midLine, btmLine);
		}
		public static string Message( string title, string text, int width = -1) {
			width = (width > 0) ? width : Config.ConsoleWidth;
			string topLine = Text.FitCenter(width, title, '┌', '─', '┐');
			string midLine = Text.FitCenter(width, text, '│');
			string btmLine = Draw.Line(width, '└', '─', '┘');
			return AlertBuilder(topLine, midLine, btmLine);
		}

		public static string Info(string text, int width = -1) {
			width = (width > 0) ? width : Config.ConsoleWidth;
			return Info("Information", text, width);
		}
		public static string Info(string title,  string text, int width = -1) {
			width = (width > 0) ? width : Config.ConsoleWidth;
			string topLine = Text.FitLeft(width, title, '┌', '─', '┐');
			string midLine = Text.FitLeft(width, text, '│');
			string btmLine = Draw.Line(width, '└', '─', '┘');
			return AlertBuilder(topLine, midLine, btmLine);
		}

		public static string Usage(string text, int width = -1) {
			width = (width > 0) ? width : Config.ConsoleWidth;
			return Usage("? Usage ?", text, width);
		}
		public static string Usage(string title, string text, int width = -1) {
			width = (width > 0) ? width : Config.ConsoleWidth;
			string topLine = Text.FitCenter(width, title, '?', '─');
			string midLine = Text.FitLeft(width, text, '│');
			string btmLine = Draw.Line(width, '?', '─');
			return AlertBuilder(topLine, midLine, btmLine);
		}

		public static string Error(string text, int width = -1) {
			width = (width > 0) ? width : Config.ConsoleWidth;
			return Error("! Error !", text, width);
		}
		public static string Error(string title, string text, int width = -1) {
			width = (width > 0) ? width : Config.ConsoleWidth;
			string topLine = Text.FitCenter(width, title, '!', '-');
			string midLine = Text.FitCenter(width, text, '|');
			string btmLine = Draw.Line(width, '!', '-');
			return AlertBuilder(topLine, midLine, btmLine);
		}

		public static string AlertBuilder(string topLine, string midLine, string btmLine) {
			StringBuilder sb = new StringBuilder();

			sb.Append(topLine);
			sb.Append(midLine);
			sb.Append(btmLine);

			return sb.ToString();
		}
		#endregion
	}
}

