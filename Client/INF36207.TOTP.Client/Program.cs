using Microsoft.Extensions.Configuration;

namespace INF36207.TOTP.Client
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            int otpLifetime = config.GetValue<int>("TOTP:OTP_LIFETIME");
            int otpLength = config.GetValue<int>("TOTP:OPT_LENGTH");

            string secretKey = config.GetValue<string>("TOTP:SECRET_KEY") ?? "";

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new ClientForm(secretKey, otpLength, otpLifetime));
        }
    }
}