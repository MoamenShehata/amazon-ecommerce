using Amazon.SharedKernel.Common.Services;

namespace Amazon.Customers.Domain.ValueObjects;

public record PaymentCardNumber
{
    public string Masked { get; private set; }
    public string Value { get; private set; }

    private PaymentCardNumber(string value, string masked)
    {
        //ArgumentException.ThrowIfNullOrEmpty(value, nameof(PaymentCardNumber));
        //ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, ValidLength, nameof(PaymentCardNumber));
        //ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, ValidLength, nameof(PaymentCardNumber));

        Value = value;
        Masked = masked;
    }

    public class PaymentCardNumberFactory(ITextServices _textServices)
    {
        private const int MaskLength = 12;

        public async Task<PaymentCardNumber> CreateSecuredAsync(string cardNumberAsPlainText)
        {
            var encryptedCardNumber = await _textServices.EncryptAsync(cardNumberAsPlainText);

            var maskedCardNumber = cardNumberAsPlainText.ToString().Replace(cardNumberAsPlainText.Substring(0, MaskLength), string.Join("", Enumerable.Range(1, MaskLength).Select(x => "*")));

            return new PaymentCardNumber(encryptedCardNumber, maskedCardNumber);
        }
    }
}
