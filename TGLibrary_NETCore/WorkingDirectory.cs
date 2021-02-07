using System;

namespace TGLibrary {
	public class WorkingDirectory : IDisposable {
		#region Properties
		public string Path { get; private set; }
		#endregion

		#region Public Methods
		public WorkingDirectory() {
			this.Path = System.IO.Path.GetTempPath() + System.IO.Path.DirectorySeparatorChar + FileHelper.GetUniqueString() + System.IO.Path.DirectorySeparatorChar;
			Path = FileHelper.SafeCreateDirectory(Path);
		}

		public WorkingDirectory(string newFolderName) {
			this.Path = System.IO.Path.GetTempPath() + System.IO.Path.DirectorySeparatorChar + newFolderName + System.IO.Path.DirectorySeparatorChar;
			Path = FileHelper.SafeCreateDirectory(Path);
		}

		public override string ToString() {
			return Path;
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
					Path = null;
					FileHelper.RemoveDirectory(Path);
				}
				// Unmanaged objects.

				// Set disposed
				disposed = true;
			}
		}

		// C# Destructor syntax for finalization code.
		~WorkingDirectory() {
			// Using 'false' lets Dispose(bool) know it is being called via the finalisation code.
			Dispose(false);
		}
		#endregion
	}
}