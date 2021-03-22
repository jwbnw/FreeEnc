using System;
using System.IO;
using System.IO.Abstractions;
using System.Security.Cryptography;
using System.Text;
using Xunit;
using FreeEnc.Crypto;
using FreeEnc.Infrastructure;
using Moq;


namespace FreeEnc.Tests.Crypto
{
	public class Aes128CbcPbkdf2Tests
	{	
		public Aes128CbcPbkdf2Tests()
		{
			_aes128CbcPbkdf2 = new Aes128CbcPbkdf2();
			_fileSystem = new Mock<IFileSystem>();
			_securePasswordGenerator = new Mock<ISecurePasswordGenerator>();
			_implementsAesCryptoServiceProvider = new Mock<IImplementsAesCryptoServiceProvider>();
			_implementsCryptoStream = new Mock<IImplementsCryptoStream>();
		}

		// TODO Crypto Tests
		[Fact]
		public void ShouldWriteEncryptedStreamWithCorrectParameters()
		{
			GivenFileMetadata();
			GivenValidPasswordAndSalt();
			GivenSourceStreamCreated();
			GivenDestinationStreamCreated();
			GivenAesCryptoServiceProviderCreated();

			//WhenEncryptingDocument()

			//ThenExtentionPlaceHolderIsWrittenToDestinationStream()
			//ThenExtentionBytesIsWrittenToDestinationStream()
			//ThenSaltIsWrittenToDestinationStream()
			//ThenIvIsWrittenToDestinationStream()
		}
		
		[Fact]
		public void ShouldResetKeyAndDeleteUnencryptedFileWhenEncrypting()
		{
			//GivenFileMetadata()
			//GivenValidPassword()
			//GivenSourceStreamCreated()
			//GivenDestinationStreamCreated()
			//GivenAesCryptoServiceProviderCreated()

			//WhenEncryptingDocument()

			//ThenKeyIsReset()
			//ThenUnencryptedFileIsRemoved()

		}

		#region Givens

		private void GivenFileMetadata()
		{
			_fileMetadata = new FileMetadata("test/file/location.cs");
		}

		private void GivenValidPasswordAndSalt()
		{
			var byteArr = new byte[8];
		
			_password = "password";
			_securePasswordGenerator.SetupProperty(x => x.Salt, byteArr);

			var rfcMock = new Rfc2898DeriveBytes(_password, byteArr); // wrap RfcDerivedBytes so I can Mock it?
			
			_securePasswordGenerator.Setup(x => x.Get128Pdkdf2Key(_password)).Returns(rfcMock);
		}

		private void GivenSourceStreamCreated()
		{
			_fileSystem.Setup(x => x.File.OpenRead(_fileMetadata.StartFileLocation));
		}

		private void GivenDestinationStreamCreated()
		{
			_fileSystem.Setup(x => x.File.Create(_fileMetadata.NewFileLocation));
		}

		private void GivenAesCryptoServiceProviderCreated()
		{
			_implementsAesCryptoServiceProvider.Setup(x => x.GetAesCryptoServiceProvider());
		}

		#endregion

		private readonly Aes128CbcPbkdf2 _aes128CbcPbkdf2;
		private Mock<IFileSystem> _fileSystem;
		private Mock<ISecurePasswordGenerator> _securePasswordGenerator;
		private Mock<IImplementsAesCryptoServiceProvider> _implementsAesCryptoServiceProvider;
		private Mock<IImplementsCryptoStream> _implementsCryptoStream;

		private FileMetadata _fileMetadata;
		private string _password;


	}
}