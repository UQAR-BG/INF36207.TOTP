using INF36207.TOTP.Core.Exceptions;
using INF36207.TOTP.Core.Services.Interfaces;
using INF36207.TOTP.Core.Utils;

namespace INF36207.TOTP.Core.Services;

public class CounterService : ICounterService
{
    private int _otpLifetime;
    
    public int OtpLifetime
    {
        get => _otpLifetime;
        set
        {
            if (value < 1)
                throw new OtpLifetimeInvalidException("Un nombre de secondes positif doit être fourni.", value);
            _otpLifetime = value;
        }
    }

    public CounterService(int otpLifetime = 1)
    {
        OtpLifetime = otpLifetime;
    }
    
    public long GetCounter()
    {
        return GetCounter(OtpLifetime);
    }
    
    public long GetCounter(int otpLifetimeInSeconds)
    {
        return DateTimeUtils.GetCurrentUnixTime() / otpLifetimeInSeconds;
    }

    public long SecondsBeforeNextOtp(int otpLifetimeInSeconds)
    {
        return otpLifetimeInSeconds - DateTimeUtils.GetCurrentUnixTime() % otpLifetimeInSeconds;
    }
}