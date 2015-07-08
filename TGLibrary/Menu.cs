using System;
using System.Collections.Generic;
using System.Text;
using TGLibrary.TGConsole;

namespace TGLibrary
{
	public partial class Menu
	{
		public string Name { get; private set; }
		public int Length { get; private set; }
		public int Count { get; private set; }

		private int Last { get { return Length - 1; } }
		private List<Option> Options = new List<Option>();

		Option More = new Option("More");
		Option Back = new Option("Back");
		Option Quit = new Option("QUIT");

		public Menu(string name, int length = 10)
		{	Name = name;
			Length = length;
			Count = 0;
			//Options.Add(Quit);
		}

		public void AddOption(string newOp)
		{
			AddOption(new Option(newOp));
		}

		public int Display()
		{	return Display(this);
		}

		public int Display(Menu menu)
		{
			Console.Write( menu.ToString() );
			return 0;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(Alert.Title(Name));

			int i = 1;
			foreach (Option o in Options)
			{
				int selector = ( i++ ) % Length;
				sb.AppendFormat("{0}.\t{1}", selector, o.ToString());
				sb.AppendLine();
			}
			return sb.ToString();
		}

		private void AddOption(Option newOp)
		{
			Options.Insert(Count, newOp);
			Count++;
			
			if ( Count == 1 || ((( Count + 3 ) % Length ) == 0 ) )
			{
				Options.Insert(Count, Quit);
				Options.Insert(Count, Back);
				Options.Insert(Count, More);
				if ( Count != 1 ) Count += 3;
			}
		}
	}
}
