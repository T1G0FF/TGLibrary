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
			T[] result = null;

			if (Directory.Exists(dirPath)) {
				string[] fileList = Directory.GetFiles(dirPath);
				result = ProcessFileList<T>(fileList, function);
			}

			return result;
		}

		public static T[] ProcessFileList<T>(string[] fileList, Func<string, T> function) {
			T[] fileResultsList = null;

			if (fileList != null && fileList.Length > 0) {
				int fileCountTotal = fileList.Length;
				fileResultsList = new T[fileCountTotal];

				int totalThreadGroups = (int) Math.Ceiling(fileCountTotal / (double) ThreadCount);

				for (int currentThreadGroup = 0; currentThreadGroup < totalThreadGroups; currentThreadGroup++) {
					int left = fileCountTotal - (currentThreadGroup * ThreadCount);
					int size = left > ThreadCount ? ThreadCount : left;
					Task[] currentTasks = new Task[size];

					for (int fileNumber = 0; fileNumber < size; fileNumber++) {
						int fileIndex = currentThreadGroup * ThreadCount + fileNumber;
						string currentFile = fileList[fileIndex];
						currentTasks[fileNumber] = Task.Factory.StartNew(() => {
							if (File.Exists(currentFile)) {
								fileResultsList[fileIndex] = function(currentFile);
							}
						});
					}
					Task.WaitAll(currentTasks);
				}
			}

			return fileResultsList;
		}

		public static T2[] ProcessCollection<T1, T2>(ICollection<T1> collection, Func<T1, T2> function) {
			T2[] resultsArray = null;

			if (collection != null && collection.Count > 0) {
				int itemCountTotal = collection.Count;
				T1[] itemArray = new T1[collection.Count];
				collection.CopyTo(itemArray, 0);
				resultsArray = new T2[itemCountTotal];

				int totalThreadGroups = (int) Math.Ceiling(itemCountTotal / (double) ThreadCount);

				for (int currentThreadGroup = 0; currentThreadGroup < totalThreadGroups; currentThreadGroup++) {
					int left = itemCountTotal - (currentThreadGroup * ThreadCount);
					int size = left > ThreadCount ? ThreadCount : left;
					Task[] currentTasks = new Task[size];

					for (int itemNumber = 0; itemNumber < size; itemNumber++) {
						int itemIndex = currentThreadGroup * ThreadCount + itemNumber;
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
			T[] result = null;

			if (Directory.Exists(dirPath)) {
				string[] fileList = Directory.GetFiles(dirPath);
				result = ProcessFileList<T>(fileList, function, parameters);
			}

			return result;
		}

		public static T[] ProcessFileList<T>(string[] fileList, Func<string, object[], T> function, params object[] parameters) {
			T[] fileResultsList = null;

			if (fileList != null && fileList.Length > 0) {
				int fileCountTotal = fileList.Length;
				fileResultsList = new T[fileCountTotal];

				int totalThreadGroups = (int) Math.Ceiling(fileCountTotal / (double) ThreadCount);

				for (int currentThreadGroup = 0; currentThreadGroup < totalThreadGroups; currentThreadGroup++) {
					int left = fileCountTotal - (currentThreadGroup * ThreadCount);
					int size = left > ThreadCount ? ThreadCount : left;
					Task[] currentTasks = new Task[size];

					for (int fileNumber = 0; fileNumber < size; fileNumber++) {
						int fileIndex = currentThreadGroup * ThreadCount + fileNumber;
						string currentFile = fileList[fileIndex];
						currentTasks[fileNumber] = Task.Factory.StartNew(() => {
							if (File.Exists(currentFile)) {
								fileResultsList[fileIndex] = function(currentFile, parameters);
							}
						});
					}
					Task.WaitAll(currentTasks);
				}
			}

			return fileResultsList;
		}
		#endregion
	}
}
