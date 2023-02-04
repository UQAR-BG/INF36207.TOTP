using INF36207.TOTP.Core.Services;
using INF36207.TOTP.Core.Services.Interfaces;
using INF36207.TOTP.Core.Services.OTP;
using INF36207.TOTP.Core.Services.OTP.Interfaces;
using System.Text;

namespace INF36207.TOTP.Client
{
    public partial class ClientForm : Form
    {
        private readonly string _secretKey;
        private readonly int _otpLength;
        private readonly int _otpLifetime;

        private readonly IOtpService _otpService;
        private readonly ICounterService _counterService;
        private readonly IHashService _hashService;

        public ClientForm(string secretKey, int otpLength, int otpLifetime)
        {
            InitializeComponent();

            _secretKey = secretKey;
            _otpLength = otpLength;
            _otpLifetime = otpLifetime;

            _counterService = new CounterService(_otpLifetime);
            _hashService = new HashService(new ASCIIEncoding());
            _otpService = new TotpService(_counterService, _hashService, _secretKey, _otpLength);
        }

        /// <summary>
        /// Set value in the form componenets on load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientForm_Load(object sender, EventArgs e)
        {
            lblCountdown.Text = GetSecondsBeforeNextOtp();
            txtJeton.Text = _otpService.CurrentOtp;

            timerCountdown.Start();
        }

        /// <summary>
        /// Simple function that print the OTP. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerCountdown_Tick(object sender, EventArgs e)
        {
            lblCountdown.Text = GetSecondsBeforeNextOtp();

            if (_otpService.CheckIfOtpChanged())
            {
                txtJeton.Text = _otpService.CurrentOtp;
            }
        }

        /// <summary>
        /// This close the timer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerCountdown.Stop();
        }

        /// <summary>
        /// Print the time avalible before the next OTP. 
        /// </summary>
        /// <returns></returns>
        private string GetSecondsBeforeNextOtp()
        {
            long seconds = _counterService.SecondsBeforeNextOtp(_otpLifetime);
            return _counterService.Format(seconds);
        }

        /// <summary>
        /// This function enable a button to copy the OTP the the clipboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            string otp = _otpService.CurrentOtp;
            Clipboard.SetDataObject(otp);
        }
    }
}