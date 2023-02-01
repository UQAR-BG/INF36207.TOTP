namespace INF36207.TOTP.Core.Utils;

/*
 * Tout le cr�dit de l'id�e d'obtention du Unix Timestamp (le nombre de secondes �coul�es
 * depuis le premier janvier 1970) doit �tre port� au compte de l'utilisateur Bob, 04/12/2020.
 * Rep�r� � https://stackoverflow.com/questions/17632584/how-to-get-the-unix-timestamp-in-c-sharp
 */

public static class DateTimeUtils
{
    public static long GetCurrentUnixTime()
    {
        // On retourne le nombre de secondes �coul�es depuis le premier janvier 1970
        // en se basant sur la date et l'heure actuelle en Coordinated Universal Time (UTC).
        // En d'autres termes, on retourne le Unix Timestamp.
        return DateTimeOffset.Now.ToUnixTimeSeconds();
    }
}