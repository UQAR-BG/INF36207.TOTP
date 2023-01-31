namespace INF36207.TOTP.Core.Services.OTP.Interfaces;

public interface IOtpService
{
    string CurrentOtp { get; set; }
    string PreviousOtp { get; set; }

    bool CheckIfOtpChanged();
    string ComputeNextOtp();
    bool IsValid(int otp);
}