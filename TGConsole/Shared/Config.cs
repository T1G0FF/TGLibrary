using System;

namespace TGConsole {
	/// <remarks>
	/// Contains settings default configuration.
	/// </remarks>
	public static class Config {
		private const int DEFAULT_WIDTH = 80;
		private static int? _ConsoleWidth = null;
		public static int ConsoleWidth {
			get {
				if (_ConsoleWidth == null) {
					if (ConsoleFunctions.ConsolePresent) {
						_ConsoleWidth = Console.WindowWidth;
					}
					else {
						_ConsoleWidth = DEFAULT_WIDTH;
					}
				}
				return _ConsoleWidth.Value;
			}
		}
	}
}

