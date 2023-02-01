using INF36207.TOTP.Core.Services.Interfaces;
using INF36207.TOTP.Core.Services.OTP.Interfaces;

namespace INF36207.TOTP.Core.Services.OTP;

public class TotpService : IOtpService
{
    private readonly ICounterService _counterService; 
    private readonly IHashService _hashService;

    private string _currentOtp;
    private readonly int _length;
    private readonly string _secretKey;

    public string CurrentOtp
    {
        get
        {
            if (string.IsNullOrEmpty(_currentOtp))
                CurrentOtp = ComputeNextOtp();
            return _currentOtp;
        }
        set 
        {
            PreviousOtp = _currentOtp;

            if (value.Length < _length)
                value = value.PadRight(_length, '0');
            _currentOtp = value;
        }
    }
    
    public string PreviousOtp { get; set; }

    public TotpService(ICounterService counterService, IHashService hashService, string key, int length)
    {
        _counterService = counterService;
        _hashService = hashService;
        _secretKey = key;
        _length = length;
    }

    // Si le TOTP g�n�r� � l'instant pr�cis est diff�rent de celui
    // contenu depuis la derni�re g�n�ration, il devient le nouveau jeton.
    public bool CheckIfOtpChanged()
    {
        bool otpChanged = false;
        string nextOtp = ComputeNextOtp();

        if (!nextOtp.Equals(CurrentOtp))
        {
            PreviousOtp = CurrentOtp;
            CurrentOtp = nextOtp;

            otpChanged = true;
        }

        return otpChanged;
    }

    // L'algorithme de g�n�ration du TOTP se situe dans cette m�thode. 
    // Les explications de l'algorithme ainsi que nos sources d'information
    // et d'inspiration seront donn�es au fur et � mesure.
    public string ComputeNextOtp()
    {
        // G�n�ration du compteur utilis� pour la cr�ation du hash
        long counter = _counterService.GetCounter();
        string strCounter = counter.ToString();

        // Calcul du code HMAC (Hash-based Message Authentication Code)
        // utilisant l'algorithme de chiffrement SHA-1.
        byte[] hmacHash = _hashService.ComputeHmacSha1(_secretKey, strCounter);

        // Calcul du jeton unique d'une longueur de 8 caract�res.
        return ComputeOtp(hmacHash).ToString();
    }

    // On compare le TOTP calcul� par le service avec celui fourni par l'usager.
    public bool IsValid(int otp)
    {
        return _currentOtp.Equals(otp.ToString());
    }

    /*
     * Tout le cr�dit de l'algorithme afin d'isoler certaines valeurs 
     * du hash et les assembler pour former un jeton de 8 caract�res
     * (une valeur enti�re inf�rieure � 100 000 000) doit �tre port� 
     * au compte de Prakash Sharma, 18/06/2018.
     * Rep�r� � https://www.freecodecamp.org/news/how-time-based-one-time-passwords-work-and-why-you-should-use-them-in-your-app-fdd2b9ed43c3/
     */

    // Algorithme pour obtenir un jeton OTP de 8 caract�res de long
    // depuis le hash HMAC produit avec la cl� secr�te, le compteur bas�
    // sur le Unix Timestamp et l'algorithme de chiffrement SHA-1.
    // Cet algorithme n�cessite une explication �tape par �tape.
    private int ComputeOtp(byte[] hmacHash)
    {
        // On prend le dernier octet du hash (hmacHash[^1]) et on applique le
        // masque binaire 0000 1111 afin de r�cup�rer la valeur des derniers 4 bits.
        // Cette valeur est garantie d'�tre inf�rieure � 15 puisque 4 bits � 1 = 15.
        // Ainsi, il est certain, que l'on pourra manipuler 4 octets du tableau sans
        // rencontrer une erreur out of bound.
        int offset = hmacHash[^1] & 0x0F;

        // On prend un des octets du tableau et on lui applique le masque binaire
        // 0111 1111 afin d'�tre certain d'avoir un bit � gauche � z�ro. Sur cette valeur
        // n�cessairement inf�rieure, on effectue un d�calage � gauche de 24 bits (<< 24).
        // L'octet calcul� � la ligne 109 est d�cal� vers le premier octet � gauche.
        // Le masque 0x7F force le dernier bit � gauche � devenir un z�ro. Ainsi, il est
        // impossible d'obtenir une valeur de jeton n�gative puisque le dernier bit � gauche
        // sera toujours z�ro.
        int firstByte = hmacHash[offset++] & 0x7f;
        firstByte = firstByte << 24;

        // On prend l'octet suivant du tableau et on lui applique le masque binaire
        // 1111 1111 afin d'isoler notre octet dans le 32 bits. Sur cette valeur,
        // on effectue un d�calage � gauche de 16 bits (<< 16).
        // L'octet calcul� � la ligne 119 est d�cal� vers le deuxi�me octet � partir de la gauche.
        int secondByte = hmacHash[offset++] & 0xff;
        secondByte = secondByte << 16;

        // On prend l'octet suivant du tableau et on lui applique le masque binaire
        // 1111 1111 afin d'isoler notre octet dans le 32 bits. Sur cette valeur,
        // on effectue un d�calage � gauche de 8 bits (<< 8).
        // L'octet calcul� � la ligne 126 est d�cal� vers le troisi�me octet � partir de la gauche.
        int thirdByte = hmacHash[offset++] & 0xff;
        thirdByte = thirdByte << 8;

        // On prend l'octet suivant du tableau et on lui applique le masque binaire
        // 1111 1111 afin d'isoler notre octet dans le 32 bits. Cette valeur, dont les
        // 24 derniers bits sont n�cessairement � z�ro peut demeurer telle quelle dans
        // le premier octet � partir de la droite.
        int fourthByte = hmacHash[offset] & 0xff;

        // On effctue un OU binaire entre les 4 valeurs afin de combiner les 4 octets
        // sur un seul 32 bits et ainsi obtenir une valeur enti�re qui tient sur 32 bits.
        int otp = firstByte | secondByte | thirdByte | fourthByte;

        // On retourne le reste de l'op�ration modulo entre notre valeur sur 32 bits et
        // 10^8 (ici, 100 000 000). Le reste sera n�cessairement inf�rieur � 100 000 000
        // dans ce cas-ci et on aura donc un OTP sur 8 caract�res.
        return otp % (int)Math.Pow(10, _length);
    }
}