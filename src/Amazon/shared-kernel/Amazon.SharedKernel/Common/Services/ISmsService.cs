using System.Diagnostics;

namespace Amazon.SharedKernel.Common.Services;

public interface ISmsService
{
    Task SendMessageAsync(string phoneNumber, string message);
}

public class SmsService : ISmsService
{
    public async Task SendMessageAsync(string phoneNumber, string message)
    {
        Debug.WriteLine($"Sending SMS to {phoneNumber}: {message}");
    }
}