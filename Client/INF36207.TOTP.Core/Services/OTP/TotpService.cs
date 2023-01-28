using INF36207.TOTP.Core.Services.Interfaces;
using INF36207.TOTP.Core.Services.OTP.Interfaces;

namespace INF36207.TOTP.Core.Services.OTP;

public class TotpService : IOtpService
{
    private readonly ICounterService _counterService; 
    private readonly IHashService _hashService;

    private int _currentOtp;
    private int _previousOtp;
    private readonly int _length;
    private readonly string _secretKey;

    public int CurrentOtp
    {
        get
        {
            if (_currentOtp == 0)
                _currentOtp = ComputeNextOtp();
            return _currentOtp;
        }
        set 
        {
            PreviousOtp = _currentOtp;
            _currentOtp = value;
        }
    }
    
    public int PreviousOtp { get; set; }

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
        int nextOtp = ComputeNextOtp();

        if (nextOtp != CurrentOtp)
        {
            PreviousOtp = CurrentOtp;
            CurrentOtp = nextOtp;

            otpChanged = true;
        }

        return otpChanged;
    }
    
    public int ComputeNextOtp()
    {
        long counter = _counterService.GetCounter();
        string strCounter = counter.ToString();

        byte[] hmacHash = _hashService.ComputeHmacSha1(_secretKey, strCounter);

        return ComputeOtp(hmacHash);
    }

    public bool IsValid(int otp)
    {
        if(_currentOtp == otp)
           return true;
        else
            return false;
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