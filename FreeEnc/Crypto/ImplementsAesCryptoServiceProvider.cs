using System.Security.Cryptography;
using System.Diagnostics.CodeAnalysis;

namespace FreeEnc.Crypto
{
	/// <summary>
	/// Provides an interface for AesCryptoServiceProvider
	/// consider looking into making Generic
	/// </summary>
	public interface IImplementsAesCryptoServiceProvider
	{
		Rfc2898DeriveBytes Key {get; set;}
		AesCryptoServiceProvider GetAesCryptoServiceProvider();
	}

	//consider not bothering with this..
	[ExcludeFromCodeCoverage]
	public class ImplementsAesCryptoServiceProvider : IImplementsAesCryptoServiceProvider
	{
		public Rfc2898DeriveBytes Key {get; set;}
		public AesCryptoServiceProvider GetAesCryptoServiceProvider()
		{
			return new AesCryptoServiceProvider();
		}
	}
}