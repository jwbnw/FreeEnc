using FreeEnc.Infrastructure;
using Xunit;

namespace FreeEnc.Tests.InfrastructureTests
{
	public class FileLocationHandlerTests
	{
		public FileLocationHandlerTests()
		{
			_fileLocationHandler = new FileLocationHandler();
		}

		[Fact]

		public void ShouldSetEncryptFileLocation()
		{
			GivenFileLocationForEncryption();
			WhenSetEncryptFileLocationCalled();
			ThenFileMetaDataIsCorrectSetForEncryption();
		}

		[Fact]
		public void ShouldSetDecryptFileLocation()
		{
			GivenFileLocationForDecrypt();
			WhenSetDecryptFileLocationCalled();
			ThenFileMetaDataIsCorrectForDecryption();
		}
		#region Givens

		private void GivenFileLocationForEncryption()
		{
			_locationToEncrypt = "SomeLocation/To/Test.txt";
		}

		private void GivenFileLocationForDecrypt()
		{
			_locationToEncrypt = "SomeLocation/To/Test.fec";
		}

		#endregion

		#region Whens

		private void WhenSetEncryptFileLocationCalled()
		{
			_fileMetaData = _fileLocationHandler.SetEncryptFileLocationInfo(_locationToEncrypt);
		}

		private void WhenSetDecryptFileLocationCalled()
		{
			_fileMetaData = _fileLocationHandler.SetDecryptFileLocationInfo(_locationToEncrypt);
		}

		#endregion

		#region Thens

		private void ThenFileMetaDataIsCorrectSetForEncryption()
		{
			//Possibly break into own Then funcs if add more test cases such as 4 char extentions (e.g .docx)
			Assert.Equal(".txt", _fileMetaData.Extention);
			Assert.Equal(4, _fileMetaData.LengthOfExtiontionAsByte);
			Assert.Equal(_locationToEncrypt, _fileMetaData.StartFileLocation);
			Assert.Equal("SomeLocation/To/Test.fec", _fileMetaData.NewFileLocation);
		}
		
		private void ThenFileMetaDataIsCorrectForDecryption()
		{
			//Possibly break into own Then funcs if add more test cases
			Assert.Equal(_locationToEncrypt, _fileMetaData.StartFileLocation);
			Assert.Equal("SomeLocation/To/Test", _fileMetaData.NewFileLocation);
		}
		
		#endregion

		private FileLocationHandler _fileLocationHandler;
		private string _locationToEncrypt;

		private FileMetadata _fileMetaData;
		
	}
}