using System;
using System.IO;

namespace TGLibrary {
    public class CSVLoader : IDisposable {
        #region Properties
        private string[][] _rows;
        private bool _hasHeaders;
        private char[] _separator;
        public string FilePath { get; private set; }
        public string FileName { get; private set; }
        public string[] Headers { get; private set; }
        public int RowCount { get; private set; }
        public bool IsValid { get; private set; }

        public string[] this[int index] {
            get {
                return _rows[index];
            }
        }

        #endregion

        #region Public Methods
        public CSVLoader(string filePath, char separator, bool hasHeaders) {
            RowCount = 0;
            IsValid = false;
            FilePath = filePath;
            FileName = FilePath.Substring(FilePath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            _separator = new char[] { separator };
            _hasHeaders = hasHeaders;
            if (File.Exists(filePath)) {
                LoadCSV(filePath);
            }
            else {
                throw new FileNotFoundException(String.Format("CSV file not found - {0}", filePath));
            }
        }

        public string[] Find(string query, int column = 0) {
            foreach (string[] row in _rows) {
                if (query.Equals(row[column])) {
                    return row;
                }
            }
            return null;
        }

        public override string ToString() {
            return FileName;
        }
        #endregion

        #region Private Methods
        private bool LoadCSV(string filePath) {
            // Default to Invalid 
            IsValid = false;

            // If loading file fails
            try {
                RowCount = File.ReadAllLines(filePath).Length;
            }
            catch {
                throw;
            }

            // Does the first row contain hasHeaders?
            if (_hasHeaders == true) { RowCount = RowCount - 1; }
            if (RowCount > 1) {
                using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read)) {
                    using (StreamReader sr = new StreamReader(fs)) {
                        // Store first row in seperate array
                        if (_hasHeaders == true) {
                            Headers = sr.ReadLine().Split(_separator);
                        }

                        for (int index = 0; index < RowCount; index++) {
                            string[] row = sr.ReadLine().Split(_separator);
                            _rows[index] = row;
                        }

                        sr.Close();
                        IsValid = true;
                    }
                }
            }
            return IsValid;
        }
        #endregion

        #region IDisposable Members
        private bool disposed = false;

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                // Managed objects
                if (disposing) {
                    _rows = null;
                    _separator = null;
                    FileName = null;
                    Headers = null;
                }
                // Unmanaged objects.

                // Set disposed
                disposed = true;
            }
        }

        // C# Destructor syntax for finalization code.
        ~CSVLoader() {
            // Using 'false' lets Dispose(bool) know it is being called via the finalisation code.
            Dispose(false);
        }
        #endregion
    }
}