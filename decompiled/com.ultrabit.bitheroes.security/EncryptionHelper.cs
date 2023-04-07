using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace com.ultrabit.bitheroes.security;

public static class EncryptionHelper
{
	private static string key = "jdY9[{`wC*Gz6/'Vga=ZH~N{cC/*]nwT";

	public static string Encrypt(string clearText)
	{
		string password = key;
		byte[] bytes = Encoding.Unicode.GetBytes(clearText);
		using Aes aes = Aes.Create();
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, new byte[13]
		{
			73, 118, 97, 110, 32, 77, 101, 100, 118, 101,
			100, 101, 118
		});
		aes.Key = rfc2898DeriveBytes.GetBytes(32);
		aes.IV = rfc2898DeriveBytes.GetBytes(16);
		using MemoryStream memoryStream = new MemoryStream();
		using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
		{
			cryptoStream.Write(bytes, 0, bytes.Length);
			cryptoStream.Close();
		}
		clearText = Convert.ToBase64String(memoryStream.ToArray());
		return clearText;
	}

	public static string Decrypt(string cipherText)
	{
		try
		{
			string password = key;
			cipherText = cipherText.Replace(" ", "+");
			byte[] array = Convert.FromBase64String(cipherText);
			using (Aes aes = Aes.Create())
			{
				Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, new byte[13]
				{
					73, 118, 97, 110, 32, 77, 101, 100, 118, 101,
					100, 101, 118
				});
				aes.Key = rfc2898DeriveBytes.GetBytes(32);
				aes.IV = rfc2898DeriveBytes.GetBytes(16);
				using MemoryStream memoryStream = new MemoryStream();
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
				{
					cryptoStream.Write(array, 0, array.Length);
					cryptoStream.Close();
				}
				cipherText = Encoding.Unicode.GetString(memoryStream.ToArray());
			}
			return cipherText;
		}
		catch (FormatException)
		{
			return null;
		}
	}
}
