using System;

namespace TGLibrary
{
	/// <remarks>
	/// Contains settings default configuration.
	/// </remarks>
	public static class Config
	{	private static int _DEFAULTWIDTH = Console.WindowWidth;
		private static int _DEFAULTROTATION = 13;
		public static int DefaultWidth { get { return _DEFAULTWIDTH; } }
		public static int DefaultRotation { get { return _DEFAULTROTATION; } }
	}
}
