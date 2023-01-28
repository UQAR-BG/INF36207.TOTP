namespace INF36207.TOTP.Core.Utils;

// https://stackoverflow.com/questions/17632584/how-to-get-the-unix-timestamp-in-c-sharp

public static class DateTimeUtils
{
    public static long GetCurrentUnixTime()
    {
        return DateTimeOffset.Now.ToUnixTimeSeconds();
    }
}