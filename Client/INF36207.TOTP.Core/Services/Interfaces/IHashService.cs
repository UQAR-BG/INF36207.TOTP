namespace INF36207.TOTP.Core.Services.Interfaces;

public interface IHashService
{
    byte[] ComputeHmacSha1(string key, string data);
}