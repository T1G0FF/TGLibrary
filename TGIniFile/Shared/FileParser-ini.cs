using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace IniFileNS {
	public static class LocalExtensions {
		public static bool ToBoolean(this string value, bool nullValueIs) {
			return value.ToBoolean() ?? nullValueIs;
		}
		public static bool? ToBoolean(this string value) {
			switch (value?.ToUpper().Trim()) {
				case "TRUE":
				case "T":
				case "YES":
				case "Y":
				case "1":
					return true;
				case "FALSE":
				case "F":
				case "NO":
				case "N":
				case "0":
					return false;
				default:
					return null;
			}
		}
	}

	public class IniFile {
		private readonly string _filePath;
		public IniFile(string filePath) {
			_filePath = filePath;
		}

		#region bool
		public bool? GetBool(string section, string key) {
			return GetBool(_filePath, section, key);
		}
		public static bool? GetBool(string filePath, string section, string key) {
			bool? result = null;

			string value = GetString(filePath, section, key);
			if ((result = value.ToBoolean()) == null) {
				if (int.TryParse(value, out int parseResultInt)) {
					result = parseResultInt > 0;
				}
			}

			return result;
		}
		#endregion

		#region int		
		public int? GetInt(string section, string key) {
			return GetInt(_filePath, section, key);
		}
		public static int? GetInt(string filePath, string section, string key) {
			int? result = null;

			string value = GetString(filePath, section, key);
			if (int.TryParse(value, out int parseResult)) {
				result = parseResult;
			}

			return result;
		}
		#endregion

		#region double
		public double? GetDouble(string section, string key) {
			return GetDouble(_filePath, section, key);
		}
		public static double? GetDouble(string filePath, string section, string key) {
			double? result = null;

			string value = GetString(filePath, section, key);
			if (double.TryParse(value, out double parseResult)) {
				result = parseResult;
			}

			return result;
		}
		#endregion

		#region decimal
		public decimal? GetDecimal(string section, string key) {
			return GetDecimal(_filePath, section, key);
		}
		public static decimal? GetDecimal(string filePath, string section, string key) {
			decimal? result = null;

			string value = GetString(filePath, section, key);
			if (decimal.TryParse(value, out decimal parseResult)) {
				result = parseResult;
			}

			return result;
		}
		#endregion

		#region string
		public string GetString(string section, string key) {
			return GetString(_filePath, section, key);
		}
		public static string GetString(string filePath, string section, string key) {
			string result = null;
			string line = "";

			string sectionName = $"[{section}]";

			using (StreamReader sr = new StreamReader(filePath)) {
				while ((line = sr.ReadLine()) != null) {
					// Skip anything that is not a section header
					if (line == "" || line[0] != '[')
						continue;

					if (line.ToLower() == sectionName.ToLower()) {
						result = getKeyValue(sr, key);
						break;
					}
				}
			}

			return result;
		}
		#endregion

		#region char
		public char? GetChar(string section, string key) {
			return GetChar(_filePath, section, key); ;
		}
		public static char? GetChar(string filePath, string section, string key) {
			var str = GetString(filePath, section, key);
			return !string.IsNullOrEmpty(str) ? Regex.Unescape(str)[0] : (char?)null;
		}
		#endregion

		#region string[]
		public string[] GetDelimitedArray(string section, string key, char delim = ',', bool trimEntries = false) {
			return GetDelimitedArray(_filePath, section, key, delim, trimEntries);
		}
		public static string[] GetDelimitedArray(string filePath, string section, string key, char delim = ',', bool trimEntries = false) {
			var str = GetString(filePath, section, key);
			if (string.IsNullOrEmpty(str)) return new string[0];
			var split = str.Split(delim);
			if (trimEntries) split = split.Select(i => i.Trim()).ToArray();
			return split;
		}
		#endregion

		#region GetAllInSection
		public Dictionary<string, string> GetAllInSection(string section) {
			return GetAllInSection(_filePath, section);
		}
		public static Dictionary<string, string> GetAllInSection(string filePath, string section) {
			Dictionary<string, string> result = new Dictionary<string, string>();
			string sectionName = $"[{section}]";

			bool found = false;
			foreach (string line in _readFrom(filePath)) {
				if (line == "")
					continue;
				if (found) {
					if (line[0] == '[')
						break;

					Match m = KeyValuePair.Match(line);
					if (m.Success) {
						result.Add(m.Groups[1].Value.ToLower(), m.Groups[2].Value);
					}
				}
				else {
					if (line[0] != '[')
						continue;

					if (line.ToLower() == sectionName.ToLower()) {
						found = true;
					}
				}
			}

			return result;
		}
		#endregion

		private static readonly Regex KeyValuePair = new Regex(@"([A-Za-z0-9_]+?)\s*[=]\s*(.+)");
		private static string getKeyValue(StreamReader sr, string key) {
			string result = null;
			string line = "";

			// Keep reading until end of file or section.
			while ((line = sr.ReadLine()) != null) {
				// Skip blank lines and comments
				if (line == "" || line[0] == ';')
					continue;
				// We've entered another section
				if (line[0] == '[')
					break;

				Match m = KeyValuePair.Match(line);
				if (m.Success) {
					if (m.Groups[1].Value.ToLower() == key.ToLower()) {
						result = m.Groups[2].Value;
						break;
					}
				}
			}

			return result;
		}
		private static IEnumerable<string> _readFrom(string file) {
			string line;
			using (var reader = File.OpenText(file)) {
				while ((line = reader.ReadLine()) != null) {
					yield return line;
				}
			}
		}
	}
}
