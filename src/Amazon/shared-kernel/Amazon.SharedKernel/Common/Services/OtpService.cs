namespace Amazon.SharedKernel.Common.Services;

public interface IOtpService
{
    Task<string> GenerateAsync();
    Task<bool> ValidateAsync(Guid userId, string otp);
}

public class OtpService : IOtpService
{
    public async Task<string> GenerateAsync() => await Task.FromResult("1234");
    public async Task<bool> ValidateAsync(Guid userId, string otp) => await Task.FromResult(otp == "1234");
    public async Task RevokeAsync(Guid userId, string otp) { }
}