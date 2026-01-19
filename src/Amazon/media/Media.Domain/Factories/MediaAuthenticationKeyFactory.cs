using Media.Domain.ValueObjects;

namespace Media.Domain.Factories
{
    public class MediaAuthenticationKeyFactory
    {
        internal MediaAuthenticationKey Create()
        {
            return new MediaAuthenticationKey(Guid.NewGuid().ToString());
        }
    }
}
