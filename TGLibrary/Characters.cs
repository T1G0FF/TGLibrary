using System;

namespace TGLibrary
{
	public class Characters
	{	
		public class Letters
		{	public static string LowerCase
			{	get { return "abcdefghijklmnopqrstuvwxyz"; }
			}

			public static string UpperCase
			{	get { return LowerCase.ToUpper(); }
			}

			public static string Both
			{	get { return Letters.LowerCase + Letters.UpperCase; }
			}
		}

		public static string Numbers
		{	get { return "0123456789"; }
		}

		public static string Alphanumeric
		{	get { return Letters.Both + Numbers; }
		}
		
		public static string Punctuation
		{	get { return "./<>?;:\"'`!@#$%^&*()[]{}_+=|\\-"; }
		}

		public static string All
		{	get { return Alphanumeric + Punctuation; }
		}
	}
}
