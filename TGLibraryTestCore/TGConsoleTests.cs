using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TGConsole;

namespace TGLibraryTestCore {
    [TestClass]
    public class TGConsoleTests {
        [TestMethod]
        public void TGConsoleTests_1() {
            var titleStr = (new string[] { "Line 1", "Line 2", "Line 3", "Line 4", "Line 5" }).ToString("\n");
			var alertStr = Alert.Info("Converted Images", titleStr);
			Assert.AreEqual(7, alertStr.Split("\n").Length);
        }
    }
}