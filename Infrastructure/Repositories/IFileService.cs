using Microsoft.AspNetCore.Http;

namespace Infrastructure.Repositories
{
    public interface IFileService
    {
        public string GetSystemPath();
        public Task SaveFile(string folderExtra, string filePath, byte[] data);
        public string GetFilePath(string fileName);
        Task<string> GetImagePath(string fileName);
    }
}