using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace IniFileNS {
	public static class LocalExtensions {
#if NET6_0_OR_GREATER
		internal static readonly FileStreamOptions DEFAULT_FSO = new FileStreamOptions() {
			Mode = FileMode.Open,
			Access = FileAccess.Read,
			Share = FileShare.ReadWrite,
			Options = FileOptions.SequentialScan
		};
#else
		public static FileStream OpenReadOnly(string filePath) {
			return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4096, FileOptions.SequentialScan);
		}
#endif

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

	public class IniReaderOptions {
		public const char SectionStart = '[';
		public const char SubSectionDelimiter = '.';
		public const char SectionEnd = ']';

		public bool CaseSensitive;
		public char[] KeyDelimiters;
		public char[] CommentDelimiters;

		public StringSplitOptions StringSplitOptions = StringSplitOptions.None;
#if !NET5_0_OR_GREATER
		public bool TrimEntries = false;
#endif

		public IniReaderOptions(bool caseSensitive, char keyDelimiter, char commentDelimiters, StringSplitOptions splitOptions = StringSplitOptions.None)
				: this(caseSensitive, new[] { keyDelimiter }, new[] { commentDelimiters }, splitOptions) {
		}
		public IniReaderOptions(bool caseSensitive = false, char[] keyDelimiter = null, char[] commentDelimiters = null, StringSplitOptions splitOptions = StringSplitOptions.None) {
			CaseSensitive = caseSensitive;
			KeyDelimiters = keyDelimiter ?? new[] { '=' };
			CommentDelimiters = commentDelimiters ?? new[] { ';', '#' };
			StringSplitOptions = splitOptions;
		}
	}

	public class IniFile : List<IniSection> {
		private readonly Dictionary<string, int> _indexLookup;

		public IniFile(bool caseSensitive = false) {
			_indexLookup = new Dictionary<string, int>(0, caseSensitive ? StringComparer.InvariantCulture : StringComparer.InvariantCultureIgnoreCase);
		}

		public new void Add(IniSection section) {
			base.Add(section);
			if (!string.IsNullOrEmpty(section.Name)) _indexLookup.Add(section.Name, this.Count() - 1);
		}

		public void Update(IniSection section) {
			int index;
			if ((index = this.Contains(section)) >= 0) {
				this[index] = section;
			}
			else {
				this.Add(section);
			}
		}

		public new int Contains(IniSection section) {
			return this.Contains(section.Name);
		}
		public int Contains(string name) {
			int index = -1;
			_indexLookup.TryGetValue(name, out index);
			return index;
		}

		public IniSection this[string key] {
			get {
				return this[_indexLookup[key]];
			}
			set {
				this.Update(value);
			}
		}

		public static IniFile Read<T>(string filePath, IniReaderOptions options = null) where T : IniRow, new() {
#if NET6_0_OR_GREATER
			using (TextReader reader = new StreamReader(filePath, LocalExtensions.DEFAULT_FSO)) {
#else
			using (Stream fileStream = LocalExtensions.OpenReadOnly(filePath))
			using (TextReader reader = new StreamReader(fileStream)) {
#endif
				return IniFile.Read<T>(reader, options);
			}
		}

		public static IniFile Read<T>(TextReader reader, IniReaderOptions options = null) where T : IniRow, new() {
			var _options = options ?? new IniReaderOptions();

			var result = new IniFile(_options.CaseSensitive);
			foreach (var element in IniFile.ReadSections<T>(reader, _options)) {
				result.Add(element);
			}
			return result;
		}

		public static IEnumerable<IniSection> ReadSections<T>(string filePath, IniReaderOptions options = null) where T : IniRow, new() {
#if NET6_0_OR_GREATER
			using (TextReader reader = new StreamReader(filePath, LocalExtensions.DEFAULT_FSO)) {
#else
			using (Stream fileStream = LocalExtensions.OpenReadOnly(filePath))
			using (TextReader reader = new StreamReader(fileStream)) {
#endif
				foreach (var section in IniFile.ReadSections<T>(reader, options)) {
					yield return section;
				}
			}
		}

		public static IEnumerable<IniSection> ReadSections<T>(TextReader reader, IniReaderOptions options = null) where T : IniRow, new() {
			var _options = options ?? new IniReaderOptions();

			var keyDelims = Regex.Escape(_options.KeyDelimiters.Aggregate("", (str, chr) => str + chr));
			var commentDelims = Regex.Escape(_options.CommentDelimiters.Aggregate("",(str, chr) => str + chr));
			var regexPattern = $@"(?:(?:\[(?<Section>[^\]]+?)\])|(?:(?<Key>^[^{keyDelims}{commentDelims}\n]+?)[{keyDelims}](?<Value>.*?)))?(?:$|(?:^|(?<=(?:\[\1\]|\2[{keyDelims}]\3).*))(?<Comment>[{commentDelims}].*)?$)";
			Regex IniRegex = new Regex(regexPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

			string line;
			var currentSection = new IniSection(options.CaseSensitive);
			while ((line = reader.ReadLine()) != null) {
				if (string.IsNullOrEmpty(line?.Trim())) continue;
				var currentRow = new T();
				var commentedRow = currentRow as IniRowCommented;
				var ignoreComments = currentRow != null;
				var matches = IniRegex.Match(line);

				if (!matches.Success || matches.Groups[0].Length < 1) throw new InvalidDataException("Malformed line.");

				var section = matches.Groups["Section"].Value;
				var key = matches.Groups["Key"].Value;
				var value = matches.Groups["Value"].Value;
				var comment = matches.Groups["Comment"].Value;

				var isSectionHeader = !string.IsNullOrEmpty(section);
				if (isSectionHeader) {
					if (!string.IsNullOrEmpty(currentSection.Name)) yield return currentSection;
					currentSection = new IniSection(options.CaseSensitive);
					currentSection.Name = section.Trim();
				}
				var hasComment = !string.IsNullOrEmpty(comment);
				if (hasComment && !ignoreComments) {
#if NET5_0_OR_GREATER
					if (_options.StringSplitOptions.HasFlag(StringSplitOptions.TrimEntries)) {
#else
					if (options.TrimEntries) {
#endif
						commentedRow.Comment = comment.Trim();
					}
					else {
						commentedRow.Comment = comment;
					}
				}

				if (isSectionHeader) continue;

				if (!string.IsNullOrEmpty(key)) {
					currentRow.Key = key.Trim();
#if NET5_0_OR_GREATER
					if (!string.IsNullOrEmpty(value) && _options.StringSplitOptions.HasFlag(StringSplitOptions.TrimEntries)) {
#else
					if (options.TrimEntries) {
#endif
						currentRow.Value = value.Trim();
					}
					else {
						currentRow.Value = value;
					}
				}
				if (hasComment && ignoreComments) continue;

				currentSection.Add(currentRow);
			}
			yield return currentSection;
		}
	}

	public class IniSection {
		public string Name;
		public List<IniRow> Values = new List<IniRow>(0);
		private readonly Dictionary<string, int> _indexLookup;

		public IniSection(bool caseSensitive = false) {
			_indexLookup = new Dictionary<string, int>(0, caseSensitive ? StringComparer.InvariantCulture : StringComparer.InvariantCultureIgnoreCase);
		}
		public IniSection(string name, bool caseSensitive)
		: this(caseSensitive) {
			Name = name;
		}

		public void Add(IniRow row) {
			Values.Add(row);
			if (!string.IsNullOrEmpty(row.Key)) _indexLookup.Add(row.Key, Values.Count() - 1);
		}

		public void Update(IniRow row) {
			int index;
			if ((index = this.Contains(row)) >= 0) {
				Values[index] = row;
			}
			else {
				this.Add(row);
			}
		}

		public int Contains(IniRow row) {
			int index = -1;
			_indexLookup.TryGetValue(row.Key, out index);
			return index;
		}
		public int Contains(string key) {
			int index = -1;
			_indexLookup.TryGetValue(key, out index);
			return index;
		}

		public IniRow this[string key] {
			get {
				return Values[_indexLookup[key]];
			}
			set {
				this.Update(value);
			}
		}
	}

	public class IniRow {
		public string Key { get; set; }
		public string Value { get; set; }

		#region bool
		public bool? GetBool() {
			bool? result = null;

			if ((result = Value.ToBoolean()) == null) {
				if (int.TryParse(Value, out int parseResultInt)) {
					result = parseResultInt > 0;
				}
			}

			return result;
		}
		#endregion

		#region int		
		public int? GetInt() {
			int? result = null;

			if (int.TryParse(Value, out int parseResult)) {
				result = parseResult;
			}

			return result;
		}
		#endregion

		#region double
		public double? GetDouble() {
			double? result = null;

			if (double.TryParse(Value, out double parseResult)) {
				result = parseResult;
			}

			return result;
		}
		#endregion

		#region decimal
		public decimal? GetDecimal() {
			decimal? result = null;

			if (decimal.TryParse(Value, out decimal parseResult)) {
				result = parseResult;
			}

			return result;
		}
		#endregion

		#region string
		public string GetString() {
			return Value;
		}
		#endregion

		#region char
		public char? GetChar() {
			return !string.IsNullOrEmpty(Value) ? Regex.Unescape(Value)[0] : (char?)null;
		}
		#endregion

		#region string[]
#if NET5_0_OR_GREATER
		public string[] GetDelimitedArray(bool trimEntries, char delim) {
			return GetDelimitedArray(StringSplitOptions.TrimEntries, delim);
		}
		public string[] GetDelimitedArray(StringSplitOptions splitOptions = StringSplitOptions.None, char delim = ',') {
			if (string.IsNullOrEmpty(Value)) return new string[0];
			var split = Value.Split(delim, splitOptions);
			return split;
		}
#else
		public string[] GetDelimitedArray(bool trimEntries, char delim) {
			return GetDelimitedArray(StringSplitOptions.None, trimEntries, delim); ;
		}
		public string[] GetDelimitedArray(StringSplitOptions splitOptions = StringSplitOptions.None, bool trimEntries = false, char delim = ',') {
			if (string.IsNullOrEmpty(Value)) return new string[0];
			var split = Value.Split(new[] { delim }, splitOptions);
			if (trimEntries) split = split.Select(x => x.Trim()).ToArray();
			return split;
		}
#endif
		#endregion
	}
	public class IniRowCommented : IniRow {
		public string Comment { get; set; }
	}
}
