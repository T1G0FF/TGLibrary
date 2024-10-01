using System;

namespace TGConsole {
	public class ProgressBar {
		public int Max { get; private set; }
		public int Width { get; private set; }
		public double Percent { get; private set; }
		public int Loaded { get { return (int)Math.Ceiling(Percent * Width); } }
		public int Unloaded { get { return Width - Loaded; } }


		public ProgressBar()
			: this(100, Config.ConsoleWidth - 9) {
		}

		public ProgressBar(int max)
			: this(max, Config.ConsoleWidth - 9) {
		}

		public ProgressBar(int max, int width) {
			Max = max;
			Width = width;
			Percent = 0;
		}

		public string Update(int value) {
			Percent = (double)value / (double)Max;
			string Bar = this.ToString();
			if (value > Max - 1) {
				Bar = Bar + "\n";
			}

			return "\r " + Bar;
		}

		public override string ToString() {
			return new string('█', Loaded) + new string('░', Unloaded) + String.Format(" ({0,3:F0}%)", Percent * 100);
		}
	}
}

