using FreeEnc.Crypto;
using FreeEnc.Infrastructure;
using Moq;
using Xunit;

namespace FreeEnc.Tests.InfrastructureTests
{
	public class UserInterfaceTests
	{
		public UserInterfaceTests()
		{
			_consoleWriter = new Mock<IConsoleWriter>();
			_consoleReader = new Mock<IConsoleReader>();
			_aes128CbcPbkdf2 = new Mock<IAes128CbcPbkdf2>();
			_fileLocationHandler = new Mock<IFileLocationHandler>();
			_fileMetadata = new FileMetadata("test/file/location.cs");
		}

		[Fact]
		public void ShouldSetCurrentFunctionToExecute()
		{
			GivenValidCommandInput(_encryptCommand);
			WhenInvokingUserInterface();
			ThenFunctonToExecuteIsSet();
		}

		[Fact]
		public void ShouldReturnFalseIfPassedQuitChar()
		{
			GivenValidCommandInput(_encryptCommand);
			GivenUserPassedQuitChar();
			WhenInitUserInterface();
			ThenUserInterfaceReturnsFalse();
		}

		[Fact]
		public void ShouldReturnTrueIfPassedOther()
		{
			GivenValidCommandInput(_encryptCommand);
			GivenUserPassedOtherChar();
			WhenInitUserInterface();
			ThenUserInterfaceReturnsTrue();
		}
		
		[Fact]
		public void ShouldGiveHelpIfAsked()
		{
			GivenUserAskedForHelp();
			WhenInvokingUserInterface();
			ThenHelpMessageWasDisplayed();
		}

		[Fact]
		public void ShouldEncryptIfAsked()
		{
			GivenUserAskedToEncrypt();
			GivenFileMetadataProperties();
			GivenFileLocationReturnMetadata();
			WhenInvokingUserInterface();
			ThenEncryptFunctionsWereCalled();
		}

		[Fact]
		public void ShouldDecryptIfAsked()
		{
			GivenUserAskedToDecrypt();
			GivenFileMetadataProperties();
			GivenFileLocationReturnMetadata();
			WhenInvokingUserInterface();
			ThenDecryptFunctionsWereCalled();
		}

		[Fact]
		public void ShouldGiveHelpTipIfInvalidCommand()
		{
			GivenUserEnteredInvalidCommand();
			WhenInvokingUserInterface();
			ThenInvalidCommandHelpTipWasDisplayed();
		}

		[Theory]
		[InlineData("encrypt")]
		[InlineData("decrypt")]
		[InlineData("help")]
		public void SholdValidateCommand(string command)
		{
			GivenValidCommandInput(command);
			GivenFileMetadataProperties();
			GivenFileLocationReturnMetadata();
			WhenInvokingUserInterface();
			ThenAppropriateFunctionsWereCalled(command);
		}

		#region Givens

		private void GivenValidCommandInput(string command)
		{
			_consoleReader.Setup( x => x.GetConsoleInputLine()).Returns(command);
		}

		private void GivenUserPassedQuitChar()
		{
			_consoleReader.Setup( x => x.GetConsoleInputKey()).Returns('q');
		}

		private void GivenUserPassedOtherChar()
		{
			_consoleReader.Setup( x => x.GetConsoleInputKey()).Returns('x');
		}

		private void GivenUserAskedForHelp()
		{
			_consoleReader.Setup( x => x.GetConsoleInputLine()).Returns("help");
		}

		private void GivenUserAskedToEncrypt()
		{
			_consoleReader.Setup( x => x.GetConsoleInputLine()).Returns("encrypt");
		}

		private void GivenUserAskedToDecrypt()
		{
			_consoleReader.Setup( x => x.GetConsoleInputLine()).Returns("decrypt");
		}

		private void GivenFileLocationReturnMetadata()
		{
			_fileLocationHandler.Setup(x => x.SetEncryptFileLocationInfo(It.IsAny<string>()))
				.Returns(_fileMetadata);
			_fileLocationHandler.Setup(x => x.SetDecryptFileLocationInfo(It.IsAny<string>()))
				.Returns(_fileMetadata);
		}

		private void GivenFileMetadataProperties()
		{
			_fileMetadata.NewFileLocation = "test/newfile/location.cs";
			_fileMetadata.Extention = ".cs";
			_fileMetadata.LengthOfExtiontionAsByte = 3;
		}

		private void GivenUserEnteredInvalidCommand()
		{
			_consoleReader.SetupSequence(x => x.GetConsoleInputLine())
			.Returns("Foo")
			.Returns(_encryptCommand);
		}

		#endregion 

		#region Whens
		private void WhenInvokingUserInterface()
		{
			_userInterface = new UserInterface(_consoleReader.Object, _consoleWriter.Object, _aes128CbcPbkdf2.Object, _fileLocationHandler.Object);
			_userInterface.RunInterface();
		}

		private void WhenInitUserInterface()
		{
			_userInterface = new UserInterface(_consoleReader.Object, _consoleWriter.Object, _aes128CbcPbkdf2.Object, _fileLocationHandler.Object);
		}

		#endregion 

		#region Thens

		private void ThenFunctonToExecuteIsSet()
		{
			Assert.Equal(_encryptCommand ,_userInterface.CurrentFuncToExecute);
		}

		private void ThenUserInterfaceReturnsFalse()
		{
			Assert.False(_userInterface.RunInterface());
		}

		private void ThenUserInterfaceReturnsTrue()
		{
			Assert.True(_userInterface.RunInterface());
		}

		private void ThenHelpMessageWasDisplayed()
		{
			_consoleWriter.Verify(x => x.WriteHelpCommands());
		}

		private void ThenEncryptFunctionsWereCalled()
		{
			_aes128CbcPbkdf2.Verify(x => x.EncryptDoc(_fileMetadata, It.IsAny<string>()));
		}

		private void ThenDecryptFunctionsWereCalled()
		{
			_aes128CbcPbkdf2.Verify(x => x.DecryptDoc(_fileMetadata, It.IsAny<string>()));
		}

		private void ThenInvalidCommandHelpTipWasDisplayed()
		{
			_consoleWriter.Verify(x => x.WriteIncorrectHelpTips());
		}

		private void ThenAppropriateFunctionsWereCalled(string command)
		{
			switch (command)
			{
				case "encrypt":
					ThenEncryptFunctionsWereCalled();
					break;
				case "decrypt":
					ThenDecryptFunctionsWereCalled();
					break;
				case "help":
					ThenHelpMessageWasDisplayed();
					break;
			}
		}

		#endregion

		private  Mock<IConsoleReader> _consoleReader;
		private  Mock<IConsoleWriter> _consoleWriter;
		private Mock<IAes128CbcPbkdf2> _aes128CbcPbkdf2;
		private Mock<IFileLocationHandler> _fileLocationHandler;
		
		private FileMetadata _fileMetadata;
		private UserInterface _userInterface;
		private string _encryptCommand = "encrypt";
	}
}