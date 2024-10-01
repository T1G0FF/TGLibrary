using System;
using System.Collections.Generic;
using System.Text;
using TGLibrary;

namespace TGConsole {
	public class Alert {
		#region Enums
		public enum FitTo {
			Default = Console,
			Text = 1,
			Console = 2
		}
		#endregion

		#region Subclasses
		public class AlertBuilder : IDisposable {
			#region Subclasses
			public class AlertDescriptor {
				public enum CharIndex {
					Top = 0,
					Middle = 1,
					Bottom = 2,
					Left = 0,
					Center = 1,
					Right = 2,
				}
				public string DefaultTitle;
				public char[][] BoxChars;
			}
			#endregion

			#region Private Properties
			private AlertType _type;
			private AlertDescriptor _alert;
			private List<string> _contents;
			private string _title;
			private int _width;
			private static readonly Dictionary<AlertType, AlertDescriptor> _alertLookup = new Dictionary<AlertType, AlertDescriptor>() {
				{   AlertType.Error, new AlertDescriptor() {
						DefaultTitle = "! Error !",
						BoxChars = new[]{
							new[]{ '!', '-', '!' },
							new[]{ '|', ' ', '|' },
							new[]{ '!', '-', '!' }
						}
					}
				},
				{   AlertType.Info, new AlertDescriptor() {
						DefaultTitle = "Information",
						BoxChars = new[]{
							new[]{ '┌', '─', '┐' },
							new[]{ '│', ' ', '│' },
							new[]{ '└', '─', '┘' }
						}
					}
				},
				{   AlertType.Message, new AlertDescriptor() {
						DefaultTitle = "",
						BoxChars = new[]{
							new[]{ '┌', '─', '┐' },
							new[]{ '│', ' ', '│' },
							new[]{ '└', '─', '┘' }
						}
					}
				},
				{   AlertType.Title, new AlertDescriptor() {
						DefaultTitle = "",
						BoxChars = new[]{
							new[]{ '╔', '═', '╗' },
							new[]{ '║', ' ', '║' },
							new[]{ '╚', '═', '╝' }
						}
					}
				},
				{   AlertType.Usage, new AlertDescriptor() {
						DefaultTitle = "? Usage ?",
						BoxChars = new[]{
							new[]{ '?', '─', '?' },
							new[]{ '│', ' ', '│' },
							new[]{ '?', '─', '?' }
						}
					}
				}
			};
			#endregion

			#region Constructors
			public AlertBuilder(AlertType type, string title = null, FitTo fitTo = FitTo.Default) {
				Init(type, title, FitToAsWidth(fitTo));
			}
			public AlertBuilder(AlertType type, string title, int width) {
				Init(type, title, width);
			}

			private void Init(AlertType type, string title, int width) {
				_type = type;
				_alert = _alertLookup[_type];
				_title = title ?? _alert.DefaultTitle;
				_width = width;
				_contents = new List<string>(3);

				_contents.Add(_title);
			}
			#endregion

			#region Public Methods
			public void AppendRow(string text) {
				_contents.AddRange(text.Split(new[] { Environment.NewLine, "\r\n", "\n" }, StringSplitOptions.None));
			}

			public override string ToString() {
				return this.ToString(string.Empty, _width);
			}
			public string ToString(FitTo fitTo) {
				return this.ToString(string.Empty, FitToAsWidth(fitTo));
			}
			public string ToString(int width) {
				return this.ToString(string.Empty, width);
			}

			public string ToString(string text, FitTo fitTo) {
				return this.ToString(text, FitToAsWidth(fitTo));
			}
			public string ToString(string text, int width) {
				_contents.Add(text);
				int localWidth = GetWidth(width);

				StringBuilder _sb = new StringBuilder();
				if (ConsoleFunctions.ConsolePresent && localWidth == _width) {
					_sb.Append(GetTopRow(_contents[0], localWidth));
					for (int i = 1; i < _contents.Count - 1; i++) { // 2nd to 2nd Last
						var row = _contents[i];
						_sb.Append(GetMiddleRow(row, localWidth));
					}
				}
				else {
					_sb.AppendLine(GetTopRow(_contents[0], localWidth));
					for (int i = 1; i < _contents.Count - 1; i++) { // 2nd to 2nd Last
						var row = _contents[i];
						_sb.AppendLine(GetMiddleRow(row, localWidth));
					}
				}
				_sb.Append(GetBottomRow(_contents[_contents.Count - 1], localWidth));
				return _sb.ToString();
			}

			public int GetWidth(FitTo fitTo) {
				return this.GetWidth(FitToAsWidth(fitTo));
			}
			public int GetWidth(int? width) {
				var result = width ?? _width;
				if (result < 0) {
					result = _contents.MaxBy(row => row.Length).Length + 4; // 4 in total = 2 Ends and 2 Spaces
				}
				return result;
			}
			#endregion

			#region Private Methods
			private string GetTopRow(string text, int width) {
				char l = _alert.BoxChars[(int) AlertDescriptor.CharIndex.Top][(int) AlertDescriptor.CharIndex.Left];
				char c = _alert.BoxChars[(int) AlertDescriptor.CharIndex.Top][(int) AlertDescriptor.CharIndex.Center];
				char r = _alert.BoxChars[(int) AlertDescriptor.CharIndex.Top][(int) AlertDescriptor.CharIndex.Right];

				switch (_type) {
					case AlertType.Error:
					case AlertType.Usage:
						return Text.FitCenter(width, text, l, c, r);
					case AlertType.Info:
						return Text.FitLeft(width, text, l, c, r);
					case AlertType.Message:
						return (string.IsNullOrEmpty(text)) ? Draw.Line(width, l, c, r) : Text.FitLeft(width, text, l, c, r);
					case AlertType.Title:
						return (string.IsNullOrEmpty(text)) ? Draw.Line(width, l, c, r) : Text.FitRight(width, text, l, c, r);
					default:
						return null;
				}
			}

			private string GetMiddleRow(string text, int width) {
				char l = _alert.BoxChars[(int) AlertDescriptor.CharIndex.Middle][(int) AlertDescriptor.CharIndex.Left];
				char c = _alert.BoxChars[(int) AlertDescriptor.CharIndex.Middle][(int) AlertDescriptor.CharIndex.Center];
				char r = _alert.BoxChars[(int) AlertDescriptor.CharIndex.Middle][(int) AlertDescriptor.CharIndex.Right];

				switch (_type) {
					case AlertType.Error:
					case AlertType.Message:
					case AlertType.Title:
						return Text.FitCenter(width, text, l, c, r);
					case AlertType.Info:
					case AlertType.Usage:
						return Text.FitLeft(width, text, l, c, r);
					default:
						return null;
				}
			}

			private string GetBottomRow(string text, int width) {
				char l = _alert.BoxChars[(int) AlertDescriptor.CharIndex.Bottom][(int) AlertDescriptor.CharIndex.Left];
				char c = _alert.BoxChars[(int) AlertDescriptor.CharIndex.Bottom][(int) AlertDescriptor.CharIndex.Center];
				char r = _alert.BoxChars[(int) AlertDescriptor.CharIndex.Bottom][(int) AlertDescriptor.CharIndex.Right];

				switch (_type) {
					case AlertType.Error:
					case AlertType.Info:
					case AlertType.Message:
					case AlertType.Title:
					case AlertType.Usage:
						return (string.IsNullOrEmpty(text)) ? Draw.Line(width, l, c, r) : Text.FitRight(width, text, l, c, r);
					default:
						return null;
				}
			}

			#region IDisposable
			private bool _disposed = false;
			~AlertBuilder() => Dispose(false);

			public void Dispose() {
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			protected virtual void Dispose(bool disposing) {
				if (!_disposed) {
					return;
				}

				if (disposing) {
					_contents.Clear();
					_alertLookup.Clear();
				}

				_alert = null;
				_contents = null;
				_title = null;

				_disposed = true;
			}
			#endregion
			#endregion
		}
		#endregion

		#region Private Properties
		#endregion

		#region Public Properties
		public enum AlertType {
			Error,
			Info,
			Message,
			Title,
			Usage
		}
		#endregion

		#region Public Methods
		public static string Title(string title, FitTo fitTo = FitTo.Default) {
			return Title(title, FitToAsWidth(fitTo));
		}
		public static string Title(string header, string title, FitTo fitTo = FitTo.Default) {
			return Title(header, title, FitToAsWidth(fitTo));
		}
		public static string Title(string title, int width) {
			using (var ab = new AlertBuilder(AlertType.Title, null, width)) {
				ab.AppendRow(title);
				return ab.ToString();
			}
		}
		public static string Title(string header, string title, int width) {
			using (var ab = new AlertBuilder(AlertType.Title, header, width)) {
				ab.AppendRow(title);
				return ab.ToString();
			}
		}

		public static string Message(string title, FitTo fitTo = FitTo.Default) {
			return Message(title, FitToAsWidth(fitTo));
		}
		public static string Message(string header, string title, FitTo fitTo = FitTo.Default) {
			return Message(header, title, FitToAsWidth(fitTo));
		}
		public static string Message(string text, int width) {
			using (var ab = new AlertBuilder(AlertType.Message, null, width)) {
				ab.AppendRow(text);
				return ab.ToString();
			}
		}
		public static string Message(string title, string text, int width) {
			using (var ab = new AlertBuilder(AlertType.Message, title, width)) {
				ab.AppendRow(text);
				return ab.ToString();
			}
		}

		public static string Info(string title, FitTo fitTo = FitTo.Default) {
			return Info(title, FitToAsWidth(fitTo));
		}
		public static string Info(string header, string title, FitTo fitTo = FitTo.Default) {
			return Info(header, title, FitToAsWidth(fitTo));
		}
		public static string Info(string text, int width) {
			using (var ab = new AlertBuilder(AlertType.Info, null, width)) {
				ab.AppendRow(text);
				return ab.ToString();
			}
		}
		public static string Info(string title, string text, int width) {
			using (var ab = new AlertBuilder(AlertType.Info, title, width)) {
				ab.AppendRow(text);
				return ab.ToString();
			}
		}

		public static string Usage(string title, FitTo fitTo = FitTo.Default) {
			return Usage(title, FitToAsWidth(fitTo));
		}
		public static string Usage(string header, string title, FitTo fitTo = FitTo.Default) {
			return Usage(header, title, FitToAsWidth(fitTo));
		}
		public static string Usage(string text, int width) {
			using (var ab = new AlertBuilder(AlertType.Usage, null, width)) {
				ab.AppendRow(text);
				return ab.ToString();
			}
		}
		public static string Usage(string title, string text, int width) {
			using (var ab = new AlertBuilder(AlertType.Usage, title, width)) {
				ab.AppendRow(text);
				return ab.ToString();
			}
		}

		public static string Error(string title, FitTo fitTo = FitTo.Default) {
			return Error(title, FitToAsWidth(fitTo));
		}
		public static string Error(string header, string title, FitTo fitTo = FitTo.Default) {
			return Error(header, title, FitToAsWidth(fitTo));
		}
		public static string Error(string text, int width) {
			using (var ab = new AlertBuilder(AlertType.Error, null, width)) {
				ab.AppendRow(text);
				return ab.ToString();
			}
		}
		public static string Error(string title, string text, int width) {
			using (var ab = new AlertBuilder(AlertType.Error, title, width)) {
				ab.AppendRow(text);
				return ab.ToString();
			}
		}
		#endregion

		#region Private Methods
		private static int FitToAsWidth(FitTo fitTo) {
			int width;
			switch (fitTo) {
				default:
				case FitTo.Text:
					width = -1;
					break;
				case FitTo.Console:
					width = Config.ConsoleWidth;
					break;
			}
			return width;
		}
		#endregion
	}
}

