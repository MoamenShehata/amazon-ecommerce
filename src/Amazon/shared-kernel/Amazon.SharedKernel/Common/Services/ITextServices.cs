using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.SharedKernel.Common.Services;

public interface ITextServices
{
    Task<string> EncryptAsync(string plainText);
    Task<string> DecryptAsync(string encryptedText);
}

public class TextServices : ITextServices
{
    private Dictionary<char, char> _language = new Dictionary<char, char>
    {
        { '0','a' },
        { '1','d' },
        { '2','j' },
        { '3','c' },
        { '4','p' },
        { '5','z' },
        { '6','_' },
        { '7','@' },
        { '8','u' },
        { '9','g' }
    };

    public Task<string> EncryptAsync(string plainText)
    {
        var result = plainText.ToCharArray().Select(c => _language[c]);

        return Task.FromResult(string.Join("", result));
    }

    public Task<string> DecryptAsync(string encryptedText)
    {
        var result = encryptedText.ToCharArray().Select(c => _language.FirstOrDefault(x => x.Value == c).Key);

        return Task.FromResult(string.Join("", result));
    }
}
