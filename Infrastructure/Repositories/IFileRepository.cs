namespace Infrastructure.Repositories
{
    public interface IFileRepository
    {
        public Task SaveFile(string fullPath, byte[] data);
        public string GetFullPath(string fileName, string fileFolder);
        public Task<byte[]> GetFileBytes(string fileName, string fileFolder);
    }
}