using System.Text;
using INF36207.TOTP.Core.Services;
using INF36207.TOTP.Core.Services.Interfaces;
using INF36207.TOTP.Core.Services.OTP;
using INF36207.TOTP.Core.Services.OTP.Interfaces;
using Microsoft.Extensions.Configuration;

namespace INF3602.TOTP.WinForm
{

    public partial class INF3602TOTP : Form
    {
        ICounterService counterService;
        IHashService hashService;
        IOtpService otpService;
        int otpLifetime;
        int otpLength;
        string secretKey;
        int tentative = 5;

        public delegate void TickEventHandler(object sender, EventArgs e);

        public event TickEventHandler Tick;

        public INF3602TOTP()
        {
            InitializeComponent();
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            otpLifetime = config.GetValue<int>("TOTP:OTP_LIFETIME");
            otpLength = config.GetValue<int>("TOTP:OPT_LENGTH");
            secretKey = config.GetValue<string>("TOTP:SECRET_KEY") ?? "";
            
            counterService = new CounterService(otpLifetime);
            hashService = new HashService(new ASCIIEncoding());
            otpService = new TotpService(counterService, hashService, secretKey, otpLength);
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            // Subscribe to the event.
            timer.Tick += Timer_Tick;
            timer.Start();

           
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {

            if (txtOTP.Text.Length != otpLength)
            {
                AccesError();
                return;
            }
           
            int otp;
            bool success = int.TryParse(txtOTP.Text, out otp);

            if (!success)
            {
                AccesError();
                return;
            }

            if (otpService.IsValid(otp))
            {
                MessageBox.Show("Accès Confirmé !");
                return;
            }  

            else
            {
                AccesError();
                return;
            }
            
        }

        private void AccesError()
        {
            MessageBox.Show("Accès refusé !");
            tentative--;
            if (tentative <= 0)
                System.Windows.Forms.Application.Exit();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            if (otpService.CheckIfOtpChanged())
            {
                lbLastOTP.Text = "Jeton précédant: " + otpService.PreviousOtp;
            }

            lblCountdown.Text = GetSecondsBeforeNextOtp();
        }

        private string GetSecondsBeforeNextOtp()
        {
            long seconds = counterService.SecondsBeforeNextOtp(otpLifetime);
            return counterService.Format(seconds);
        }
    }
}