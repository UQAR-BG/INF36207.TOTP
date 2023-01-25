namespace INF36207.TOTP.Core.Exceptions;

public class OtpLifetimeInvalidException : Exception
{
    public OtpLifetimeInvalidException(string message)
        : base(message)
    {
    }

    public OtpLifetimeInvalidException(string message, int otpLifetime)
        : this($"{otpLifetime} Durée fournie: {otpLifetime} secondes.")
    {
    }
}