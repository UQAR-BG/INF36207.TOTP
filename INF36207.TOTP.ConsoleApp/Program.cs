// See https://aka.ms/new-console-template for more information

// https://stackoverflow.com/questions/46442076/google-authenticator-one-time-password-algorithm-in-c-sharp
// https://www.freecodecamp.org/news/how-time-based-one-time-passwords-work-and-why-you-should-use-them-in-your-app-fdd2b9ed43c3/

using System.Text;
using INF36207.TOTP.Core.Services;
using INF36207.TOTP.Core.Services.Interfaces;
using INF36207.TOTP.Core.Services.OTP;
using INF36207.TOTP.Core.Services.OTP.Interfaces;
using Microsoft.Extensions.Configuration;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

int otpLifetime = config.GetValue<int>("TOTP:OTP_LIFETIME");
int otpLength = config.GetValue<int>("TOTP:OPT_LENGTH");

string secretKey = config.GetValue<string>("TOTP:SECRET_KEY") ?? "";

ICounterService counterService = new CounterService(otpLifetime);
IHashService hashService = new HashService(new ASCIIEncoding());
IOtpService otpService = new TotpService(
    counterService, 
    hashService, 
    secretKey, 
    otpLength);

Console.WriteLine(otpService.CurrentOtp);
Console.WriteLine($"Time left until next OTP: {counterService.SecondsBeforeNextOtp(otpLifetime)} seconds.");