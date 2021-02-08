﻿using System;
using System.Text;

namespace TGConsole {
	public class Alert {
		#region Public Methods
		public static string Title(string title) {
			return Title(Config.ConsoleWidth, title);
		}
		public static string Title(int width, string title) {
			string topLine = Draw.Line(width, '╔', '═', '╗') + Environment.NewLine;
			string midLine = Text.FitCenter(width, title, '║', ' ');
			string btmLine = Draw.Line(width, '╚', '═', '╝');
			return AlertBuilder(width, topLine, midLine, btmLine);
		}

		public static string Message(string msg) {
			return Message(Config.ConsoleWidth, msg);
		}
		public static string Message(int width, string msg) {
			string topLine = Draw.Line(width, '┌', '─', '┐') + Environment.NewLine;
			string midLine = Text.FitCenter(width, msg, '│');
			string btmLine = Draw.Line(width, '└', '─', '┘');
			return AlertBuilder(width, topLine, midLine, btmLine);
		}

		public static string Info(string msg) {
			return Info(Config.ConsoleWidth, msg);
		}
		public static string Info(int width, string msg) {
			string topLine = Text.FitLeft(width, "Information", '┌', '─', '┐');
			string midLine = Text.FitLeft(width, msg, '│');
			string btmLine = Draw.Line(width, '└', '─', '┘');
			return AlertBuilder(width, topLine, midLine, btmLine);
		}

		public static string Usage(string usage) {
			return Usage(Config.ConsoleWidth, usage);
		}
		public static string Usage(int width, string usage) {
			string topLine = Text.FitCenter(width, "? Usage ?", '?', '─');
			string midLine = Text.FitLeft(width, usage, '│');
			string btmLine = Draw.Line(width, '?', '─');
			return AlertBuilder(width, topLine, midLine, btmLine);
		}

		public static string Error(string error) {
			return Error(Config.ConsoleWidth, error);
		}
		public static string Error(int width, string error) {
			string topLine = Text.FitCenter(width, "! Error !", '!', '-');
			string midLine = Text.FitCenter(width, error, '|');
			string btmLine = Draw.Line(width, '!', '-');
			return AlertBuilder(width, topLine, midLine, btmLine);
		}
		#endregion

		#region Private Methods
		private static string AlertBuilder(int width, string topLine, string midLine, string btmLine) {
			StringBuilder sb = new StringBuilder();

			if (false && (!ConsoleFunctions.ConsolePresent || (ConsoleFunctions.ConsolePresent && width < Console.WindowWidth))) {
				sb.AppendLine(topLine);
				sb.AppendLine(midLine);
				sb.Append(btmLine);
			}
			else {
				sb.Append(topLine);
				sb.Append(midLine);
				sb.Append(btmLine);
			}

			return sb.ToString();
		}
		#endregion
	}
}
