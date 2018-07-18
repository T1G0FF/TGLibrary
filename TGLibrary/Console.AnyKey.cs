using System;

namespace TGLibrary {
    namespace TGConsole {
        /// <remarks>
        /// Contains methods for displaying "Press any key" prompts.
        /// </remarks>
        public class AnyKey {
            /// <summary>
            /// Prompts for a key press, then quits the application.
            /// </summary>
            public static void Quit() {
                Console.WriteLine("Press any key to quit...");
                Console.ReadKey(true);
                Environment.Exit(0);
            }

            /// <summary>
            /// Prompts for a key press, then clears the console and continues.
            /// </summary>
            public static void Clear() {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                Console.Clear();
            }

            /// <summary>
            /// Prompts for a key press, then continues.
            /// </summary>
            public static void Continue() {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
            }
        }
    }
}
