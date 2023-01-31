using INF36207.TOTP.Core.Services.Interfaces;
using INF36207.TOTP.Core.Services.OTP.Interfaces;
using static System.Net.WebRequestMethods;

namespace INF36207.TOTP.Core.Services.OTP;

public class TotpService : IOtpService
{
    private readonly ICounterService _counterService; 
    private readonly IHashService _hashService;

    private string _currentOtp;
    private string _previousOtp;
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
    
    public string ComputeNextOtp()
    {
        long counter = _counterService.GetCounter();
        string strCounter = counter.ToString();

        byte[] hmacHash = _hashService.ComputeHmacSha1(_secretKey, strCounter);

        return ComputeOtp(hmacHash).ToString();
    }

    public bool IsValid(int otp)
    {
        return _currentOtp.Equals(otp.ToString());
    }

    private int ComputeOtp(byte[] hmacHash)
    {
        int offset = hmacHash[^1] & 0x0F;
        int otp = (hmacHash[offset++] & 0x7f) << 24
                  | (hmacHash[offset++] & 0xff) << 16
                  | (hmacHash[offset++] & 0xff) << 8
                  | (hmacHash[offset] & 0xff);
    
        return otp % (int)Math.Pow(10, _length);
    }
}