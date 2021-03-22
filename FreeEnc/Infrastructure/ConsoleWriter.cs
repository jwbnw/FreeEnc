using System;

namespace FreeEnc.Infrastructure
{
	public interface IConsoleWriter
	{
		void WriteConsoleLine(string input);

		void WriteConsole(string input);

		void WriteHelpCommands();

		void WriteIncorrectHelpTips();
	}
	internal class ConsoleWriter : IConsoleWriter
	{
		public void WriteConsoleLine(string input)
		{
			Console.WriteLine(input);
		}

		public void WriteConsole(string input)
		{
			Console.Write(input);
		}

		public virtual void WriteHelpCommands()
		{
			Console.WriteLine(HelpCommand);
		}

		public virtual void WriteIncorrectHelpTips()
		{
			Console.WriteLine(HelpTips);
		}

		// Add to Help Command when implemented 
		// "alg or -a: lists all algortims Enc me offers");
		// "github or -g: links to open source codebase please file issues there");
		internal readonly string HelpCommand = "Here is a list of commands..\n" +
											 "encrypt: Begins encryption process. You will be prompted for additional information regarding this process \n" +
											 "decrypt or -d: Begins decryption process. You will be prompted for additional information regarding this process \n" +
											 "quit or -q: quits or exits application";

		internal readonly string HelpTips = "Opps That is not a valid command, please try agagin. For a list of valid commands type help";

	}
}