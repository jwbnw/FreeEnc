using System.IO;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;


namespace FreeEnc.Crypto
{
	public interface IImplementsCryptoStream
	{
		CryptoStream GetCryptoStream(Stream destinationStream, ICryptoTransform cryptoTransform, CryptoStreamMode CryptoStreamMode); 
	}

	[ExcludeFromCodeCoverage]
	public class ImplementsCryptoStream: IImplementsCryptoStream
	{
		public CryptoStream GetCryptoStream(Stream destinationStream, ICryptoTransform cryptoTransform, CryptoStreamMode CryptoStreamMode)
		{
			return new CryptoStream(destinationStream, cryptoTransform, CryptoStreamMode);
		}
	}
}