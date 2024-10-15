using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TGConsole;

namespace TGLibraryTestCore {
	[TestClass]
	public class TGConsoleTests {
		[TestMethod]
		public void TGConsoleTests_1CharNewLine() {
			var titleStr = "Title Here";
			var contentArray = new string[] { "Line 1", "Line 2", "Line 3", "Line 4", "Line 5" };
			var contentStr = contentArray.ToString("\n");
			var alertStr = Alert.Info(titleStr, contentStr);
			Assert.AreEqual(contentArray.Length + 2, alertStr.Split("\n").Length); // Line count plus top and bottom row.
		}

		[TestMethod]
		public void TGConsoleTests_2CharNewline() {
			var titleStr = "Title Here";
			var contentArray = new string[] { "Line 1", "Line 2", "Line 3", "Line 4", "Line 5" };
			var contentStr = contentArray.ToString("\r\n");
			var alertStr = Alert.Info(titleStr, contentStr);
			Assert.AreEqual(contentArray.Length + 2, alertStr.Split("\n").Length); // Line count plus top and bottom row.
		}

		[TestMethod]
		public void TGConsoleTests_FitToConsole() {
			var titleStr = "Title Here";
			var contentStr = "Content Goes Here";
			var alertStr = Alert.Info(titleStr, contentStr, Alert.FitTo.Console);
			var topLine = alertStr.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)[0];
			Assert.AreEqual(Config.ConsoleWidth, topLine.Length);
		}


		[TestMethod]
		public void TGConsoleTests_FitToText() {
			var titleStr = "Title Here";
			var contentStr = "Content Goes Here";
			var alertStr = Alert.Info(titleStr, contentStr, Alert.FitTo.Text);
			var topLine = alertStr.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None)[0];
			Assert.AreEqual(contentStr.Length + 4, topLine.Length); // Line length plus 2 Ends and 2 Spaces
		}
	}
}