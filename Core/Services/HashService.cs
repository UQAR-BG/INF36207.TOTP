using System.Security.Cryptography;
using System.Text;
using INF36207.TOTP.Core.Services.Interfaces;

namespace INF36207.TOTP.Core.Services;

/*
 * Tout le cr�dit de l'id�e du calcul du hash HMAC avec SHA-1 doit �tre port� au compte 
 * de l'utilisateur Ogglas, 27/11/2017.
 * Rep�r� � https://stackoverflow.com/questions/46442076/google-authenticator-one-time-password-algorithm-in-c-sharp
 */

public class HashService : IHashService
{
    private readonly Encoding _encoding;

    public HashService(Encoding encoding)
    {
        _encoding = encoding;
    }

    // On convertit la cl� secr�te et une donn�e (ici le compteur) en
    // tableaux d'octets afin de g�n�rer le hash HMAC gr�ce � l'algorithme
    // SHA-1 qui produit un hash sur 20 octets. Une instance de la classe 
    // HMACSHA1 du package System.Security.Cryptography est utilis� pour ces
    // calculs.
    public byte[] ComputeHmacSha1(string key, string data)
    {
        byte[] keyByte = _encoding.GetBytes(key);
        byte[] dataBytes = _encoding.GetBytes(data);

        HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);
        return hmacsha1.ComputeHash(dataBytes);
    }
}