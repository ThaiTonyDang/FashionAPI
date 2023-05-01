using Microsoft.AspNetCore.Http;

namespace Infrastructure.Repositories
{
    public interface IFileRepository
    {
        public string GetSystemPath();
        public Task SaveFile(string folderExtra, string filePath, byte[] data);
        public string GetFilePath(string fileName);
    }
}