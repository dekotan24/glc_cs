using System;
using System.Security.Cryptography;
using System.Text;
using static glc_cs.Core.Property;

namespace glc_cs.Core
{
	internal class Crypt : IDisposable
	{
		private Encoding enc = Encoding.UTF8;
		private Aes aes;

		public Crypt()
		{
			aes = Aes.Create();
			aes.Key = enc.GetBytes(Secret.Crypt.CryptKey);
			aes.IV = enc.GetBytes(Secret.Crypt.CryptVector);
		}

		public void Dispose()
		{
			aes?.Dispose();
			aes = null;
		}

		public string Encode(string str, bool forceCrypt = false)
		{
			byte[] data = enc.GetBytes(str);    // 文字列をバイト配列に変換する
			byte[] encrypted = data;
			if (forceCrypt)
			{
				encrypted = aes.CreateEncryptor().TransformFinalBlock(data, 0, data.Length); // 暗号化
			}
			return Convert.ToBase64String(encrypted);
		}

		public string Decode(string str)
		{
			byte[] data = Convert.FromBase64String(str);    // Base64デコーディングする
			byte[] decrypted = data;
			if (EnablePWCrypt)
			{
				decrypted = aes.CreateDecryptor().TransformFinalBlock(data, 0, data.Length); // 復号化
			}
			return enc.GetString(decrypted);
		}
	}
}
