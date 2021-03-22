using System;
using System.Diagnostics.CodeAnalysis;
using FreeEnc.Infrastructure;

namespace FreeEnc
{
		/* TODO:
		1) Testing manual/unit tests

		1.5) Error handling..

		2) Reconsider custom ext arch.. do we really need it.. or does it just confuse things..if we do need it can we design it a little better...

		2) Tackle Directory level encryption
		*/
	[ExcludeFromCodeCoverage]
	class Program
	{
		static void Main()
		{
			UserInterface userInterface = new UserInterface();
			bool continueProgram;
			Console.WriteLine("Hi! Welcome to FreeEnc, a free, Command Line Based File and Folder Encryption Tool for Mac, Windows and Linux!");
			
			do
			{
				continueProgram = userInterface.RunInterface();
			} while (continueProgram);

		}


	}
}