using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            int i = otpService.CurrentOtp;
            txtOTP.Text = i.ToString();

            if (txtOTP.Text.Length != 6)
            {
                MessageBox.Show("AccŠs refus‚ !");
                return;
            }
           
            int otp;
            bool success = int.TryParse(txtOTP.Text, out otp);

            if (!success)
            {
                MessageBox.Show("AccŠs refus‚ !");
                return;
            }

            if (otpService.IsValid(otp))
            {
                MessageBox.Show("AccŠs Confirm‚ !");
                return;
            }  
            else
            {
                MessageBox.Show("AccŠs refus‚ !");
                return;
            }
            
        }
        private void Timer_Tick(object sender, EventArgs e)
        {

            if (counterService.SecondsBeforeNextOtp(otpLifetime) == 60)
            {
                otpService.CurrentOtp = otpService.ComputeNextOtp();
                System.Threading.Thread.Sleep(1000);
                lbLastOTP.Text = otpService.PreviousOtp.ToString();
                
            }
            lbTime.Text = "Timer left: " + counterService.SecondsBeforeNextOtp(otpLifetime).ToString();
        }
    }
}