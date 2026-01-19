namespace Media.Domain.Factories
{
    public class MediaFactory(MediaAccessibilityFactory _accessibilityFactory)
    {
        public Media CreateForPublic(Guid id, Guid ownerId, long sizeInBytes, string name, string path, string mimeType)
        {
            return new Media(id, ownerId, sizeInBytes, name, path, mimeType, _accessibilityFactory.Public());
        }
        
        public Media CreateSecured(Guid id, Guid ownerId, long sizeInBytes, string name, string path, string mimeType)
        {
            return new Media(id, ownerId, sizeInBytes, name, path, mimeType, _accessibilityFactory.Protected());
        }
    }
}