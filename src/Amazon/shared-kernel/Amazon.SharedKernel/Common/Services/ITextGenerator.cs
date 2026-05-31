namespace Amazon.SharedKernel.Common.Services;

public interface ITextGenerator
{
    Task<string> GenerateDigitsAsync(int length);
}

public class TextGenerator : ITextGenerator
{
    private readonly Random _random = new Random();

    public async Task<string> GenerateDigitsAsync(int length)
    {
        var otp = string.Empty;
        for (int i = 0; i < length; i++)
            otp += _random.Next(0, 10).ToString();
        return await Task.FromResult(otp);
    }
}