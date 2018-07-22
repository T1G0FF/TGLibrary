using System;

namespace TGLibrary {
    /// <remarks>
    /// Contains settings default configuration.
    /// </remarks>
    public static class Config {
        public static int DefaultWidth { get; } = Console.WindowWidth;
        public static int DefaultRotation { get; } = 13;
    }
}
