using System;
using System.Collections.Generic;
using System.Text;

namespace TGConsole {
	public class Alert {
		#region Sub Classes
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
			private readonly AlertType _type;
			private AlertDescriptor _alert;
			private StringBuilder _sb;
			private string _title;
			private readonly int _width;
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
			public AlertBuilder(AlertType type, string title = null, int width = -1) {
				_type = type;
				_alert = _alertLookup[_type];
				_title = title ?? _alert.DefaultTitle;
				_width = (width > 0) ? width : Config.ConsoleWidth;

				_sb = new StringBuilder();
				_sb.AppendLine(GetTopRow(_title));
			}
			#endregion

			#region Public Methods
			public void AppendRow(string text) {
				_sb.AppendLine(GetMiddleRow(text));
			}

			public override string ToString() {
				return this.ToString("");
			}

			public string ToString(string text) {
				_sb.Append(GetBottomRow(text));
				return _sb.ToString();
			}
			#endregion

			#region Private Methods
			private string GetTopRow(string text) {
				char l = _alert.BoxChars[(int) AlertDescriptor.CharIndex.Top][(int) AlertDescriptor.CharIndex.Left];
				char c = _alert.BoxChars[(int) AlertDescriptor.CharIndex.Top][(int) AlertDescriptor.CharIndex.Center];
				char r = _alert.BoxChars[(int) AlertDescriptor.CharIndex.Top][(int) AlertDescriptor.CharIndex.Right];

				switch (_type) {
					case AlertType.Error:
					case AlertType.Usage:
						return Text.FitCenter(_width, text, l, c, r);
					case AlertType.Info:
						return Text.FitLeft(_width, text, l, c, r);
					case AlertType.Message:
						return (String.IsNullOrEmpty(text)) ? Draw.Line(_width, l, c, r) : Text.FitLeft(_width, text, l, c, r);
					case AlertType.Title:
						return (String.IsNullOrEmpty(text)) ? Draw.Line(_width, l, c, r) : Text.FitRight(_width, text, l, c, r);
					default:
						return null;
				}
			}

			private string GetMiddleRow(string text) {
				char l = _alert.BoxChars[(int) AlertDescriptor.CharIndex.Middle][(int) AlertDescriptor.CharIndex.Left];
				char c = _alert.BoxChars[(int) AlertDescriptor.CharIndex.Middle][(int) AlertDescriptor.CharIndex.Center];
				char r = _alert.BoxChars[(int) AlertDescriptor.CharIndex.Middle][(int) AlertDescriptor.CharIndex.Right];

				switch (_type) {
					case AlertType.Error:
					case AlertType.Message:
					case AlertType.Title:
						return Text.FitCenter(_width, text, l, c, r);
					case AlertType.Info:
					case AlertType.Usage:
						return Text.FitLeft(_width, text, l, c, r);
					default:
						return null;
				}
			}

			private string GetBottomRow(string text) {
				char l = _alert.BoxChars[(int) AlertDescriptor.CharIndex.Bottom][(int) AlertDescriptor.CharIndex.Left];
				char c = _alert.BoxChars[(int) AlertDescriptor.CharIndex.Bottom][(int) AlertDescriptor.CharIndex.Center];
				char r = _alert.BoxChars[(int) AlertDescriptor.CharIndex.Bottom][(int) AlertDescriptor.CharIndex.Right];

				switch (_type) {
					case AlertType.Error:
					case AlertType.Info:
					case AlertType.Message:
					case AlertType.Title:
					case AlertType.Usage:
						return Draw.Line(_width, l, c, r);
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
					_sb.Clear();
					_alertLookup.Clear();
				}

				_alert = null;
				_sb = null;
				_title = null;

				_disposed = true;
			}
			#endregion
			#endregion
		}
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
		public static string Title(string title, int width = -1) {
			using (var ab = new AlertBuilder(AlertType.Title, null, width)) {
				ab.AppendRow(title);
				return ab.ToString();
			}
		}
		public static string Title(string header, string title, int width = -1) {
			using (var ab = new AlertBuilder(AlertType.Title, header, width)) {
				ab.AppendRow(title);
				return ab.ToString();
			}
		}
		public static string Message(string text, int width = -1) {
			using (var ab = new AlertBuilder(AlertType.Message, null, width)) {
				ab.AppendRow(text);
				return ab.ToString();
			}
		}
		public static string Message(string title, string text, int width = -1) {
			using (var ab = new AlertBuilder(AlertType.Message, title, width)) {
				ab.AppendRow(text);
				return ab.ToString();
			}
		}

		public static string Info(string text, int width = -1) {
			using (var ab = new AlertBuilder(AlertType.Info, null, width)) {
				ab.AppendRow(text);
				return ab.ToString();
			}
		}
		public static string Info(string title, string text, int width = -1) {
			using (var ab = new AlertBuilder(AlertType.Info, title, width)) {
				ab.AppendRow(text);
				return ab.ToString();
			}
		}

		public static string Usage(string text, int width = -1) {
			using (var ab = new AlertBuilder(AlertType.Usage, null, width)) {
				ab.AppendRow(text);
				return ab.ToString();
			}
		}
		public static string Usage(string title, string text, int width = -1) {
			using (var ab = new AlertBuilder(AlertType.Usage, title, width)) {
				ab.AppendRow(text);
				return ab.ToString();
			}
		}

		public static string Error(string text, int width = -1) {
			using (var ab = new AlertBuilder(AlertType.Error, null, width)) {
				ab.AppendRow(text);
				return ab.ToString();
			}
		}
		public static string Error(string title, string text, int width = -1) {
			using (var ab = new AlertBuilder(AlertType.Error, title, width)) {
				ab.AppendRow(text);
				return ab.ToString();
			}
		}
		#endregion
	}
}

