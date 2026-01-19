namespace Media.Domain.ValueObjects
{
    public class MediaAccessibility
    {
        public bool IsPublic { get; private set; }
        public MediaAuthenticationKey AuthKey { get; set; }

        internal MediaAccessibility(bool isPublic, MediaAuthenticationKey authKey)
        {
            IsPublic = isPublic;
            AuthKey = authKey;
        }
        private MediaAccessibility()
        {
            
        }
    }
}