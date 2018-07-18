using System;

namespace TGLibrary {
    /// <remarks>
    /// Contains settings default configuration.
    /// </remarks>
    public static class Config {
        private static readonly int _DEFAULTWIDTH = Console.WindowWidth;
        private static readonly int _DEFAULTROTATION = 13;
        public static int DefaultWidth => _DEFAULTWIDTH;
        public static int DefaultRotation => _DEFAULTROTATION;
    }
}
