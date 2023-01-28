namespace INF36207.TOTP.Core.Services.Interfaces;

public interface ICounterService
{
    long GetCounter();
    long GetCounter(int otpLifetimeInSeconds);

    long SecondsBeforeNextOtp(int otpLifetimeInSeconds);
}