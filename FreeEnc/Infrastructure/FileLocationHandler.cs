using System.IO;
using System;

namespace FreeEnc.Infrastructure
{
	/// <summary>
	/// Provides a method of setting necessary file information for
	/// encrypt and decrypt functionality
	/// </summary>
	public interface IFileLocationHandler
	{
		/// <summary>
		/// Takes File Location, will set necessary properties to fulfil
		/// needs of Encryption Classes in object returned
		/// </summary>
		/// <param name="encFileLocation"></param>
		FileMetadata SetEncryptFileLocationInfo(string encFileLocation); 
		
		/// <summary>
		/// Takes File Location, will set necessary properties to fulfil
		/// needs of Decryption Classes in object returned
		/// </summary>
		/// <param name="decFileLocation"></param>
		FileMetadata SetDecryptFileLocationInfo(string decFileLocation);
	}
	
	public class FileLocationHandler : IFileLocationHandler
	{
		public FileMetadata SetEncryptFileLocationInfo(string encFileLocation)
		{
			var fileInfo = new FileMetadata(encFileLocation);

			fileInfo.Extention = Path.GetExtension(fileInfo.StartFileLocation);
			fileInfo.LengthOfExtiontionAsByte = Convert.ToByte(fileInfo.Extention.Length);
			fileInfo.NewFileLocation = fileInfo.StartFileLocation.Replace(fileInfo.Extention, ".fec");

			return fileInfo;
		}

		public FileMetadata SetDecryptFileLocationInfo(string decFileLocation)
		{
			
			var fileInfo = new FileMetadata(decFileLocation);

			fileInfo.StartFileLocation = decFileLocation;
			fileInfo.NewFileLocation = fileInfo.StartFileLocation.Substring(0, fileInfo.StartFileLocation.Length - LengthOfDotFecFileType);

			return fileInfo;
		}

		private const int LengthOfDotFecFileType = 4;
	}
}