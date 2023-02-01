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

    // On retourne le compteur avec la durée de vie par défaut.
    public long GetCounter()
    {
        return GetCounter(OtpLifetime);
    }

    // On divise le Unix Timestamp par le nombre de secondes durant lequel
    // notre jeton est valide. Sans cette étape, le jeton serait différent
    // chaque seconde. En effet, la division par 60, par example, permet
    // d'obtenir le même compteur pendant cet intervalle de temps.
    public long GetCounter(int otpLifetimeInSeconds)
    {
        return DateTimeUtils.GetCurrentUnixTime() / otpLifetimeInSeconds;
    }

    // En calculant le reste de la division entre le Unix Timestamp et la durée
    // de vie du jeton en secondes, on obtient le nombre de secondes écoulées
    // depuis la dernière génération. Il suffit donc de calculer la différence
    // entre la durée de vie et cette valeur pour obtenir le temps restant au jeton.
    public long SecondsBeforeNextOtp(int otpLifetimeInSeconds)
    {
        return otpLifetimeInSeconds - DateTimeUtils.GetCurrentUnixTime() % otpLifetimeInSeconds;
    }

    public string Format(long secondsLeft)
    {
        return $"{secondsLeft} sec.";
    }
}