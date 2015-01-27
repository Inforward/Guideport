namespace Portal.Infrastructure.Helpers
{
    public static class FileHelper
    {
        public const int DefaultChunkSize = 1000000;
        public const int DefaultBufferSize = 16 * 1024;

        public static int CalculateChunks(long fileSize, int chunkSize = DefaultChunkSize)
        {
            if (fileSize < chunkSize)
                return 1;

            var chunks = fileSize / chunkSize;

            if (fileSize % chunkSize != 0)
                chunks++;

            return (int)chunks;
        }

        public static int CalculateBufferSize(int fileSize, int chunkSize = DefaultChunkSize)
        {
            return fileSize < chunkSize ? fileSize : chunkSize;
        }
    }
}
