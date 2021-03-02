using System;
using System.Collections.Generic;
using System.Text;

namespace TGLibraryTestCore {
	public static class LocalExtensions {
		public static string ToString<T>(this IEnumerable<T> collection, string separator) {
			return ToString(collection, t => t.ToString(), separator);
		}
		public static string ToString<T>(this IEnumerable<T> collection, Func<T, string> stringFunction, string separator, bool skipNullStrings = true) {
			if (collection == null) throw new ArgumentNullException($"'collection' not allowed to be null.");
			if (stringFunction == null) throw new ArgumentNullException($"'stringElement' not allowed to be null.");
			if (separator == null) throw new ArgumentNullException($"'separator' not allowed to be null.");
			StringBuilder sb = new StringBuilder();
			string value;
			foreach (var item in collection) {
				value = stringFunction(item);
				if (skipNullStrings && value == null) continue;
				sb.Append(value);
				sb.Append(separator);
			}
			return sb.ToString(0, Math.Max(0, sb.Length - separator.Length));
		}
	}
}
