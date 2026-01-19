using Media.Domain.ValueObjects;

namespace Media.Domain.Factories
{
    public class MediaAccessibilityFactory(MediaAuthenticationKeyFactory _authenticationKeyFactory)
    {
        private static MediaAccessibility _public;
        internal MediaAccessibility Public()
        {
            _public ??= new MediaAccessibility(true, MediaAuthenticationKey.Null);
            return _public;
        }

        internal MediaAccessibility Protected() => new MediaAccessibility(false, _authenticationKeyFactory.Create());

    }
}
