using Effortless.Net.Encryption;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace TravelAgency.Shared;

public static class DevCode
{
	private static byte[] _key = Convert.FromBase64String("pY3RDfDlt7xwAnKVu/MhiQxM5u//oe4jt9L+rhyECSY=");
	private static byte[] _iv = Convert.FromBase64String("C76NDuPRRtP20EMiYMyHTQ==");

	public static string ToJson(this object obj)
	{
		return JsonConvert.SerializeObject(obj);
	}

	public static T ToObject<T>(this string str)
	{
		return JsonConvert.DeserializeObject<T>(str)!;
	}

	public static string ToEncrypt(this string plainText)
	{
		return Strings.Encrypt(plainText, _key, _iv);
	}

	public static string ToDecrypt(this string encrypted)
	{
		return Strings.Decrypt(encrypted, _key, _iv);
	}

	public static string HashPassword(string password)
	{
		using SHA256 sha256 = SHA256.Create();
		var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
		string hashedPassword = Convert.ToBase64String(hashedBytes);
		return hashedPassword;
	}
}
