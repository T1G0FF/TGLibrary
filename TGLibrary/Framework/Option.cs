namespace TGLibrary {
	public partial class Menu {
		private class Option {
			public string Name { get; private set; }
			public Option(string name) {
				Name = name;
			}

			public override string ToString() {
				return Name;
			}
		}
	}
}
