using System.Security.Cryptography;

namespace FreeEnc.Crypto
{
	public interface ISecurePasswordGenerator
	{
		Rfc2898DeriveBytes Get128Pdkdf2Key(string password);

		Rfc2898DeriveBytes Get128Pdkdf2KeyWithSalt(string password, byte[] salt);
	
		byte[] Salt {get; set;}
	}

	public class SecurePasswordGenerator : ISecurePasswordGenerator
	{
		public Rfc2898DeriveBytes Get128Pdkdf2Key(string password)
		{
			Salt = GetSalt();
			return new Rfc2898DeriveBytes(password, Salt, rfcIterations);
		}

		public Rfc2898DeriveBytes Get128Pdkdf2KeyWithSalt(string password, byte[] salt)
		{
			return new Rfc2898DeriveBytes(password, salt);
		}

		internal byte[] GetSalt()
		{
			var salt = new byte[8];
			using (var rngCsp = new RNGCryptoServiceProvider())
			{
				rngCsp.GetBytes(salt);
			}
			return salt;
		}
		
		public int rfcIterations { get; set; } = 1000;

		public byte[] Salt {get; set;}

	}
}