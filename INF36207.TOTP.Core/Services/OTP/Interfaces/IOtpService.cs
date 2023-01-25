namespace INF36207.TOTP.Core.Services.OTP.Interfaces;

public interface IOtpService
{
    int CurrentOtp { get; set; }
    int PreviousOtp { get; set; }

    void CheckIfOtpChanged();
    int ComputeNextOtp();
    bool IsValid(int otp);
}