using System;
using System.IO;
using System.Threading.Tasks;

namespace TGLibrary {
    public static class TaskHelper {
        public static T[] ProcessDirectory<T>(string dirPath, Func<Object, T> function, int threadCount = 4) {
            T[] result = null;

            if (Directory.Exists(dirPath)) {
                string[] fileList = Directory.GetFiles(dirPath);
                result = ProcessFileList<T>(fileList, function, threadCount);
            }

            return result;
        }

        public static T[] ProcessFileList<T>(string[] fileList, Func<Object, T> function, int threadCount = 4) {
            T[] result = null;

            if (fileList != null && fileList.Length > 0) {
                string workingDir = Path.GetTempPath() + Path.DirectorySeparatorChar + "TEMP";
                int fileCountTotal = fileList.Length;
                T[] fileResultsList = new T[fileCountTotal];

                int totalThreadGroups = (int)Math.Ceiling(fileCountTotal / (double)threadCount);
                if ((fileCountTotal % threadCount) != 0)
                    totalThreadGroups = totalThreadGroups + 1;

                for (int currentThreadGroup = 0; currentThreadGroup < totalThreadGroups; currentThreadGroup++) {
                    int left = fileCountTotal - (currentThreadGroup * threadCount);
                    int size = left > threadCount ? threadCount : left;
                    Task[] currentTasks = new Task[size];

                    for (int fileNumber = 0; fileNumber < size; fileNumber++) {
                        int fileIndex = currentThreadGroup * threadCount + fileNumber;
                        string currentFile = fileList[fileIndex];
                        currentTasks[fileNumber] = Task.Factory.StartNew(() => {
                            if (File.Exists(currentFile))
                                fileResultsList[fileIndex] = function(currentFile);
                        });
                    }
                    Task.WaitAll(currentTasks);
                }
                FileHelper.RemoveDirectory(workingDir);
            }

            return result;
        }
    }
}
