using System.Security.Cryptography;
using System.Text;
using INF36207.TOTP.Core.Services.Interfaces;

// https://stackoverflow.com/questions/46442076/google-authenticator-one-time-password-algorithm-in-c-sharp
// https://www.freecodecamp.org/news/how-time-based-one-time-passwords-work-and-why-you-should-use-them-in-your-app-fdd2b9ed43c3/

namespace INF36207.TOTP.Core.Services;

public class HashService : IHashService
{
    private readonly Encoding _encoding;

    public HashService(Encoding encoding)
    {
        _encoding = encoding;
    }
    
    public byte[] ComputeHmacSha1(string key, string data)
    {
        byte[] keyByte = _encoding.GetBytes(key);
        byte[] dataBytes = _encoding.GetBytes(data);

        HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);
        return hmacsha1.ComputeHash(dataBytes);
    }
}