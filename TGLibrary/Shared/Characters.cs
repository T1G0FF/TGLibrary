namespace TGLibrary {
	public class Characters {
		public static class Letters {
			public static string LowerCase => "abcdefghijklmnopqrstuvwxyz";
			public static string UpperCase => LowerCase.ToUpper();
			public static string Both => (Letters.UpperCase + Letters.LowerCase);
		}
		public static string Numbers => "0123456789";
		public static string Alphanumeric => (Letters.Both + Numbers);
		public static string Symbols => "./<>?;:\"'`!@#$%^&*()[]{}_+=|\\-";
		public static string All => Alphanumeric + Symbols;
	}
}
