using System.Security.Claims;

namespace Amazon.SharedKernel.Common.Services;

public interface IOtpService
{
    Task<string> GenerateAsync(Guid userId);
    Task<string> GenerateAsync(Guid userId, string otpName);
    Task<bool> ValidateAsync(Guid userId, string otp);
    Task<bool> ValidateAsync(Guid userId, string otpName, string otp);
}

public class OtpService(
    ITextGenerator _textGenerator,
    IUserClaimsStore _userClaimsStore
    ) : IOtpService
{
    private const string OtpDefaultClaimType = "otp";
    private const string OtpDefaultCreatedOnClaimType = "otp-createdOn";
    private const int OtpLength = 5;
    private const int OtpExpiryMinutes = 2;

    private string GetOtpClaimType(string otpName) => string.IsNullOrWhiteSpace(otpName) ? OtpDefaultClaimType : $"{OtpDefaultClaimType}-{otpName}";
    private string GetOtpCreatedOnClaimType(string otpName) => string.IsNullOrWhiteSpace(otpName) ? OtpDefaultCreatedOnClaimType : $"{OtpDefaultClaimType}-{otpName}-createdOn";

    public async Task<string> GenerateAsync(Guid userId, string otpName)
    {
        var otp = await _textGenerator.GenerateDigitsAsync(OtpLength);

        await _userClaimsStore.SaveClaimsAsync(userId, new Claim(GetOtpCreatedOnClaimType(otpName), DateTime.UtcNow.ToString()), new Claim(GetOtpClaimType(otpName), otp));

        return otp;
    }

    public async Task<string> GenerateAsync(Guid userId) => await GenerateAsync(userId, string.Empty);

    public async Task<bool> ValidateAsync(Guid userId, string otpName, string otp)
    {
        var createdOnClaimType = GetOtpCreatedOnClaimType(otpName);
        var otpClaimType = GetOtpClaimType(otpName);

        var userClaims = await _userClaimsStore.GetClaimsAsync(userId, createdOnClaimType, otpClaimType);
        var otpCreatedAtClaim = userClaims.FirstOrDefault(c => c.Type == createdOnClaimType);

        if (otpCreatedAtClaim is null)
            return false;

        if (IsOtpExpired(otpCreatedAtClaim))
        {
            await _userClaimsStore.RemoveClaimsAsync(userId, createdOnClaimType, otpClaimType);
            return false;
        }

        var otpClaim = userClaims.FirstOrDefault(c => c.Type == otpClaimType);

        var isValid = otpClaim is not null && otpClaim.Value == otp;
        if (isValid)
            await _userClaimsStore.RemoveClaimsAsync(userId, createdOnClaimType, otpClaimType);

        return isValid;
    }

    public async Task<bool> ValidateAsync(Guid userId, string otp)
    {
        return await ValidateAsync(userId, string.Empty, otp);
    }

    private bool IsOtpExpired(Claim otpCreatedAtClaim)
    {
        var otpCreatedAt = DateTime.Parse(otpCreatedAtClaim.Value);
        return DateTime.UtcNow - otpCreatedAt > TimeSpan.FromMinutes(OtpExpiryMinutes);
    }
}