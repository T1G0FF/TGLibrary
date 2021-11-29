using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TGTaskHelper {
	public static class TaskHelper {
		private static int? _threadCount = null;
		private static int ThreadCount {
			get {
				if (_threadCount == null)
					_threadCount = System.Diagnostics.Process.GetCurrentProcess().Threads.Count;
				return _threadCount.Value;
			}
		}
		#region No Extra Parameters
		public static T[] ProcessDirectory<T>(string dirPath, Func<string, T> function) {
			return ProcessDirectory(dirPath, ThreadCount, function);
		}
		public static T[] ProcessDirectory<T>(string dirPath, int threadCount, Func<string, T> function) {
			T[] result = null;

			if (Directory.Exists(dirPath)) {
				string[] fileList = Directory.GetFiles(dirPath);
				result = ProcessCollection<string, T>(fileList, threadCount, (currentFile) => {
					var currentResult = default(T);
					if (File.Exists(currentFile)) {
						currentResult = function(currentFile);
					}
					return currentResult;
				});
			}

			return result;
		}

		public static T[] ProcessFileList<T>(string[] fileList, Func<string, T> function) {
			return ProcessFileList<T>(fileList, ThreadCount, function);
		}
		public static T[] ProcessFileList<T>(string[] fileList, int threadCount, Func<string, T> function) {
			return ProcessCollection<string, T>(fileList, threadCount, (currentFile) => {
				var result = default(T);
				if (File.Exists(currentFile)) {
					result = function(currentFile);
				}
				return result;
			});
		}

		public static T2[] ProcessCollection<T1, T2>(ICollection<T1> collection, Func<T1, T2> function) {
			return ProcessCollection<T1, T2>(collection, ThreadCount, function);
		}
		public static T2[] ProcessCollection<T1, T2>(ICollection<T1> collection, int threadCount, Func<T1, T2> function) {
			T2[] resultsArray = null;

			if (collection != null && collection.Count > 0) {
				var _threadCount = Math.Min(threadCount, ThreadCount);

				int itemCountTotal = collection.Count;
				T1[] itemArray = new T1[collection.Count];
				collection.CopyTo(itemArray, 0);
				resultsArray = new T2[itemCountTotal];

				int totalThreadGroups = (int) Math.Ceiling(itemCountTotal / (double) _threadCount);

				for (int currentThreadGroup = 0; currentThreadGroup < totalThreadGroups; currentThreadGroup++) {
					int left = itemCountTotal - (currentThreadGroup * _threadCount);
					int size = left > _threadCount ? _threadCount : left;
					Task[] currentTasks = new Task[size];

					for (int itemNumber = 0; itemNumber < size; itemNumber++) {
						int itemIndex = currentThreadGroup * _threadCount + itemNumber;
						T1 currentItem = itemArray[itemIndex];
						currentTasks[itemNumber] = Task.Factory.StartNew(() => {
							resultsArray[itemIndex] = function(currentItem);
						});
					}
					Task.WaitAll(currentTasks);
				}
			}

			return resultsArray;
		}
		#endregion

		#region Extra Parameters Array
		public static T[] ProcessDirectory<T>(string dirPath, Func<string, object[], T> function, params object[] parameters) {
			return ProcessDirectory<T>(dirPath, threadCount: ThreadCount, function: function, parameters: parameters);
		}
		public static T[] ProcessDirectory<T>(string dirPath, int threadCount, Func<string, object[], T> function, params object[] parameters) {
			T[] result = null;

			if (Directory.Exists(dirPath)) {
				string[] fileList = Directory.GetFiles(dirPath);
				result = ProcessCollection<string, T>(fileList, threadCount, (currentFile, currentParameters) => {
					var currentResult = default(T);
					if (File.Exists(currentFile)) {
						currentResult = function(currentFile, currentParameters);
					}
					return currentResult;
				}, parameters);
			}

			return result;
		}

		public static T[] ProcessFileList<T>(string[] fileList, Func<string, object[], T> function, params object[] parameters) {
			return ProcessFileList<T>(fileList, ThreadCount, function);
		}
		public static T[] ProcessFileList<T>(string[] fileList, int threadCount, Func<string, object[], T> function, params object[] parameters) {
			return ProcessCollection<string, T>(fileList, threadCount, (currentFile, currentParameters) => {
				var result = default(T);
				if (File.Exists(currentFile)) {
					result = function(currentFile, currentParameters);
				}
				return result;
			}, parameters);
		}

		public static T2[] ProcessCollection<T1, T2>(ICollection<T1> collection, Func<T1, object[], T2> function, params object[] parameters) {
			return ProcessCollection(collection, ThreadCount, function, parameters);
		}
		public static T2[] ProcessCollection<T1, T2>(ICollection<T1> collection, int threadCount, Func<T1, object[], T2> function, params object[] parameters) {
			T2[] resultsArray = null;

			if (collection != null && collection.Count > 0) {
				var _threadCount = Math.Min(threadCount, ThreadCount);

				int itemCountTotal = collection.Count;
				T1[] itemArray = new T1[collection.Count];
				collection.CopyTo(itemArray, 0);
				resultsArray = new T2[itemCountTotal];

				int totalThreadGroups = (int) Math.Ceiling(itemCountTotal / (double) _threadCount);

				for (int currentThreadGroup = 0; currentThreadGroup < totalThreadGroups; currentThreadGroup++) {
					int left = itemCountTotal - (currentThreadGroup * _threadCount);
					int size = left > _threadCount ? _threadCount : left;
					Task[] currentTasks = new Task[size];

					for (int itemNumber = 0; itemNumber < size; itemNumber++) {
						int itemIndex = currentThreadGroup * _threadCount + itemNumber;
						T1 currentItem = itemArray[itemIndex];
						currentTasks[itemNumber] = Task.Factory.StartNew(() => {
							resultsArray[itemIndex] = function(currentItem, parameters);
						});
					}
					Task.WaitAll(currentTasks);
				}
			}

			return resultsArray;
		}
		#endregion
	}
}
