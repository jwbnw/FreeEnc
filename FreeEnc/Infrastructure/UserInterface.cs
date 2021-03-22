using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using FreeEnc.Crypto;

namespace FreeEnc.Infrastructure
{

	/// <summary>
	/// Cointains logic for user interaction
	/// Still WIP
	/// </summary>
	public class UserInterface
	{
		public UserInterface() : this(new ConsoleReader(), new ConsoleWriter(), new Aes128CbcPbkdf2(), new FileLocationHandler())
		{
		}

		public UserInterface(IConsoleReader consoleReader, IConsoleWriter consoleWriter, IAes128CbcPbkdf2 aes128CbcPbkdf2, IFileLocationHandler fileLocationHandler)
		{
			_consoleReader = consoleReader;
			_consoleWriter = consoleWriter;
			_aes128CbcPbkdf2 = aes128CbcPbkdf2;
			_fileLocationHandler = fileLocationHandler;
		}
		public bool RunInterface()
		{
			_consoleWriter.WriteConsoleLine("Please enter a command! (enter help for a list of optins and useful tips) \n");
			
			if (ValidateCommand(_consoleReader.GetConsoleInputLine()))
			{
				ExecuteCommand(CurrentFuncToExecute);
			}

			_consoleWriter.WriteConsoleLine("To exit the program press q, press enter to continue");
			var shouldQuit = _consoleReader.GetConsoleInputKey();
			return (shouldQuit != 'q' ? true : false);
		}

		private void ExecuteCommand(string command)
		{
			switch (CurrentFuncToExecute)
			{
				case HelpCommand:
					_consoleWriter.WriteHelpCommands();
					break;
				case EncryptCommand:
					var encFileLocation = GetEncryptFileLocationFromCli(); 
					var encFileMetadata = _fileLocationHandler.SetEncryptFileLocationInfo(encFileLocation);
					var encPassword = GetPassword();
					
					_aes128CbcPbkdf2.EncryptDoc(encFileMetadata, encPassword); 
					break;
				case DecryptCommand:
					var decFileLocation = GetDecryptFileLocationFromCli();
					var decFileMetadata = _fileLocationHandler.SetDecryptFileLocationInfo(decFileLocation);
					var decPassword = GetPassword();
					
					_aes128CbcPbkdf2.DecryptDoc(decFileMetadata, decPassword);
					break;
				default:
					// should never reach default case to due input handling, maybe throw here but probably redundant
					break;
			}
		}

		private string GetEncryptFileLocationFromCli()
		{
			_consoleWriter.WriteConsoleLine("Please enter the path to the file to be encrypted");
			return _consoleReader.GetConsoleInputLine(); // probably error handle in this class too or abstract this validation and the command validation (see list below) into their own "CLI Validation Class"
		}

		private string GetDecryptFileLocationFromCli()
		{
			_consoleWriter.WriteConsoleLine("Please enter the path to the file to be Decrypted");
			return _consoleReader.GetConsoleInputLine(); // probably error handle in this class too or abstract this validation and the command validation (see list below) into their own "CLI Validation Class"
		}

		private string GetPassword()
		{
			_consoleWriter.WriteConsoleLine("Please enter a password: ");
			return _consoleReader.GetConsoleInputLine();
		}

		private bool ValidateCommand(string userIntput)
		{
			var userCommand = userIntput;
			bool isValid = false;
			
			while(!isValid) {
				isValid = _validUserCommands.Contains(userCommand);
				if(!isValid)
				{
					_consoleWriter.WriteIncorrectHelpTips();
					userCommand = _consoleReader.GetConsoleInputLine();
				}
			}
			CurrentFuncToExecute = userCommand; 
			return true;
		}

		//I feel like this List and the consts below should probably be abstracted
		//to their own class but ¯\_(ツ)_/¯ if someone wants do do that in a clean 
		//way feel free
		private static readonly IList<String> _validUserCommands = new ReadOnlyCollection<string>
		(new List<String>{
			HelpCommand, EncryptCommand, DecryptCommand 
		});

		internal const string HelpCommand = "help";

		internal const string EncryptCommand = "encrypt";
		
		internal const string DecryptCommand = "decrypt";
		
		internal string CurrentFuncToExecute {get; set;}
		
		private readonly IConsoleReader _consoleReader;
		
		private readonly IConsoleWriter _consoleWriter;

		private readonly IAes128CbcPbkdf2 _aes128CbcPbkdf2;

		private readonly IFileLocationHandler _fileLocationHandler;

	}
}