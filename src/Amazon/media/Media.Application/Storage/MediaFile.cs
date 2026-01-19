namespace Media.Application.Storage
{
    public class MediaFile
    {
        public string Path { get; private set; }
        public string MimeType { get; private set; }
        public string Name { get; private set; }
        public long SizeInBytes { get; private set; }

        public MediaFile(string path, string mimeType, string name, long sizeInBytes)
        {
            Path = path;
            MimeType = mimeType;
            Name = name;
            SizeInBytes = sizeInBytes;
        }
    }
}