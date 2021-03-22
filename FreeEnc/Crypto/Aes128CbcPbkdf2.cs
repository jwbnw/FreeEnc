using FreeEnc.Infrastructure;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace FreeEnc.Crypto
{
	public interface IAes128CbcPbkdf2
	{	
		/// <summary>
		/// Encrypts a given document using Aes128Cbc,
		/// leverages pbkdf2 for password derivation.
		/// takes various information about the file
		/// which can be set via the FileLocationHandler
		/// Class.
		/// </summary>
		/// <param name="fileInfo">FileMetadata object, returned by FileLocationHandler when setting file location</param>
		/// <param name="password">Password to use to derive key for encryption</param>
		/// <returns></returns>
		bool EncryptDoc(FileMetadata fileInfo, string password);

		/// <summary>
		/// Decrypts a given document using Aes128Cbc,
		/// leverages pbkdf2 for password derivation.
		/// takes information about the file location
		/// which can be set via the FileLocationHandler
		/// Class.
		/// </summary>
		/// <param name="fileInfo">FileMetadata object, returned by FileLocationHandler when setting file location</param>
		/// <param name="password">Password to use to derive key for decryption</param>
		/// <returns></returns>
		bool DecryptDoc(FileMetadata fileInfo, string password);
	}

	/// <summary>
	/// Contains Encrpytion and Decryption Crypto 
	/// Functionality. Very rough implementation
	/// needs to be refactored.
	/// </summary>
	public class Aes128CbcPbkdf2 : IAes128CbcPbkdf2
	{
		public Aes128CbcPbkdf2() : this(new FileSystem(), new SecurePasswordGenerator(), new ImplementsAesCryptoServiceProvider(), 
		new ImplementsCryptoStream())
		{
		}

		public Aes128CbcPbkdf2(FileSystem fileSystem, ISecurePasswordGenerator securePasswordGenerator, IImplementsAesCryptoServiceProvider implementsAesCryptoServiceProvider, 
		IImplementsCryptoStream implementsCryptoStream)
		{
			_fileSystem = fileSystem;
			_implementsAesCryptoServiceProvider = implementsAesCryptoServiceProvider;
			_securePasswordGenerator = securePasswordGenerator;
			_implementsCryptoStream = implementsCryptoStream;
		}

		// I may remove the custom extention, it's not really needed and on top of that it introduces a complcation which is arguably redudnant.
		public bool EncryptDoc(FileMetadata fileMetadata, string password)
		{
			var extHolder = new byte[1];
			extHolder[0] = fileMetadata.LengthOfExtiontionAsByte;

			byte[] extentionBytes = Encoding.ASCII.GetBytes(fileMetadata.Extention);
			var salt = new byte[8];

			_implementsAesCryptoServiceProvider.Key = _securePasswordGenerator.Get128Pdkdf2Key(password); // this was just var key = _secu... need to test still..
			salt = _securePasswordGenerator.Salt;

			try
			{
				using (var sourceStream = _fileSystem.File.OpenRead(fileMetadata.StartFileLocation))
				using (var destinationStream = _fileSystem.File.Create(fileMetadata.NewFileLocation))
				using (var provider = _implementsAesCryptoServiceProvider.GetAesCryptoServiceProvider())
					{
						provider.Key = _implementsAesCryptoServiceProvider.Key.GetBytes(16);

						using (var cryptoTransform = provider.CreateEncryptor())
						using (var cryptoStream = _implementsCryptoStream.GetCryptoStream(destinationStream, cryptoTransform, CryptoStreamMode.Write))
							{
								destinationStream.Write(extHolder, 0, extHolder.Length);
								destinationStream.Write(extentionBytes, 0, extentionBytes.Length);
								destinationStream.Write(salt, 0, salt.Length);
								destinationStream.Write(provider.IV, 0, provider.IV.Length);

								sourceStream.CopyTo(cryptoStream);

								_implementsAesCryptoServiceProvider.Key.Reset();
								_fileSystem.File.Delete(fileMetadata.StartFileLocation);

								return true;
							}
					}

			}
			//better excpetion handling... obviously 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString(), "exception in EncryptDoc()");
				return false;
			}
		}

		public bool DecryptDoc(FileMetadata fileMetadata, string password)
		{
			try
			{
				using (var sourceStream = File.OpenRead(fileMetadata.StartFileLocation))
				{

					var extLength = new byte[1];
					var saltHolder = new byte[8];

					sourceStream.Read(extLength, 0, extLength.Length);

					var lengthOfExtention = Convert.ToInt32(extLength[0]);
					var extentionToWriteInBytes = new byte[lengthOfExtention];

					sourceStream.Read(extentionToWriteInBytes, 0, extentionToWriteInBytes.Length);
					sourceStream.Read(saltHolder, 0, saltHolder.Length);

					var extentionToWriteString = Encoding.ASCII.GetString(extentionToWriteInBytes);
					var key = _securePasswordGenerator.Get128Pdkdf2KeyWithSalt(password, saltHolder);

					fileMetadata.NewFileLocation += extentionToWriteString;
					
					using (var destinationStream = File.Create(fileMetadata.NewFileLocation))
					using (var provider = _implementsAesCryptoServiceProvider.GetAesCryptoServiceProvider())
					{
						var IV = new byte[provider.IV.Length];
						sourceStream.Read(IV, 0, IV.Length);
						provider.Key = key.GetBytes(16);

						using (var cryptoTransform = provider.CreateDecryptor(provider.Key, IV))
						using (var cryptoStream = _implementsCryptoStream.GetCryptoStream(sourceStream, cryptoTransform, CryptoStreamMode.Read))
								{
									cryptoStream.CopyTo(destinationStream);
									key.Reset();
									_fileSystem.File.Delete(fileMetadata.StartFileLocation);
									return true;
								}
					}
					
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString(), "exception in DecryptDoc()");
				return false;
			}
		}

		private int _lengthOfEx { get; set; }

		private readonly IImplementsCryptoStream _implementsCryptoStream;
		private readonly IImplementsAesCryptoServiceProvider _implementsAesCryptoServiceProvider;
		private readonly ISecurePasswordGenerator _securePasswordGenerator;
		private readonly IFileSystem _fileSystem;

	}
}