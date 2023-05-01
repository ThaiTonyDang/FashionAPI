namespace Infrastructure.Repositories
{
    public interface IFileRepository
    {
        public Task SaveFile(string fullPath, byte[] data);
        public string GetFullPath(string imagePath);
        public Task<byte[]> GetFileBytes(string fileName);
    }
}