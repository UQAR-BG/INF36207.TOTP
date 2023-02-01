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

    // Si le TOTP généré à l'instant précis est différent de celui
    // contenu depuis la dernière génération, il devient le nouveau jeton.
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

    // L'algorithme de génération du TOTP se situe dans cette méthode. 
    // Les explications de l'algorithme ainsi que nos sources d'information
    // et d'inspiration seront données au fur et à mesure.
    public string ComputeNextOtp()
    {
        // Génération du compteur utilisé pour la création du hash
        long counter = _counterService.GetCounter();
        string strCounter = counter.ToString();

        // Calcul du code HMAC (Hash-based Message Authentication Code)
        // utilisant l'algorithme de chiffrement SHA-1.
        byte[] hmacHash = _hashService.ComputeHmacSha1(_secretKey, strCounter);

        // Calcul du jeton unique d'une longueur de 8 caractères.
        return ComputeOtp(hmacHash).ToString();
    }

    // On compare le TOTP calculé par le service avec celui fourni par l'usager.
    public bool IsValid(int otp)
    {
        return _currentOtp.Equals(otp.ToString());
    }

    /*
     * Tout le crédit de l'algorithme afin d'isoler certaines valeurs 
     * du hash et les assembler pour former un jeton de 8 caractères
     * (une valeur entière inférieure à 100 000 000) doit être porté 
     * au compte de Prakash Sharma, 18/06/2018.
     * Repéré à https://www.freecodecamp.org/news/how-time-based-one-time-passwords-work-and-why-you-should-use-them-in-your-app-fdd2b9ed43c3/
     */

    // Algorithme pour obtenir un jeton OTP de 8 caractères de long
    // depuis le hash HMAC produit avec la clé secrète, le compteur basé
    // sur le Unix Timestamp et l'algorithme de chiffrement SHA-1.
    // Cet algorithme nécessite une explication étape par étape.
    private int ComputeOtp(byte[] hmacHash)
    {
        // On prend le dernier octet du hash (hmacHash[^1]) et on applique un
        // masque binaire 0000 1111 afin de récupérer la valeur des derniers 4 bits.
        // Cette valeur est garantie d'être inférieure 
        int offset = hmacHash[^1] & 0x0F;

        int firstByte = hmacHash[offset++] & 0x7f;
        firstByte = firstByte << 24;

        int secondByte = hmacHash[offset++] & 0xff;
        secondByte = secondByte << 16;

        int thirdByte = hmacHash[offset++] & 0xff;
        thirdByte = thirdByte << 8;

        int fourthByte = hmacHash[offset] & 0xff;

        int otp = firstByte | secondByte | thirdByte | fourthByte;

        return otp % (int)Math.Pow(10, _length);
    }
}