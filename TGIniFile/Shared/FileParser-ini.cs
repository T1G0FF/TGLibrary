using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace IniFileNS {
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
			if (bool.TryParse(value, out bool parseResult)) {
				result = parseResult;
			}
			if (int.TryParse(value, out int parseResultInt)) {
				result = parseResultInt > 0;
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
