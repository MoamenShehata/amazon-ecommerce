namespace Media.Domain.ValueObjects
{
    public class MediaAuthenticationKey
    {
        internal static MediaAuthenticationKey _null;
        public string? Value { get; private set; }

        internal MediaAuthenticationKey(string? value)
        {
            Value = value;
        }
        internal static MediaAuthenticationKey Null
        {
            get
            {
                _null ??= new MediaAuthenticationKey(null);
                return _null;
            }
        }

        private MediaAuthenticationKey()
        {

        }
    }
}