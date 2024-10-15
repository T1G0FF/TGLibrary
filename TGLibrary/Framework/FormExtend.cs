using System;
using System.Threading;
using System.Windows.Forms;

namespace TGLibrary {
	internal static class FormExtensions {
		private delegate void SafeCallDelegate(object state);

		private static void ApplicationRunProc(object state) {
			if ((state as Form).InvokeRequired) {
				var d = new SafeCallDelegate(ApplicationRunProc);
				(state as Form).BeginInvoke(d, new object[] { state });
			}
			else {
				Application.Run(state as Form);
			}
		}

		public static void RunInNewThread(this Form form, bool isBackground) {
			if (form == null)
				throw new ArgumentNullException("form");
			if (form.IsHandleCreated)
				throw new InvalidOperationException("Form is already running.");
			Thread thread = new Thread(ApplicationRunProc);
			thread.SetApartmentState(ApartmentState.STA);
			thread.IsBackground = isBackground;
			thread.Start(form);
		}
	}
}
