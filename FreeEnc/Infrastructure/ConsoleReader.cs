using System;
using System.Diagnostics.CodeAnalysis;

namespace FreeEnc.Infrastructure
{
	public interface IConsoleReader
	{
		string GetConsoleInputLine();

		char GetConsoleInputKey();
	}
	[ExcludeFromCodeCoverage]
	internal sealed class ConsoleReader : IConsoleReader
	{
		public string GetConsoleInputLine()
		{
			return Console.ReadLine();
		}

		public char GetConsoleInputKey()
		{
			return Console.ReadKey().KeyChar;
		}
	}
}