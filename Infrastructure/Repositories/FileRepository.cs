using Infrastructure.Config;
using Infrastructure.Models;
using Microsoft.Extensions.Options;
using System.IO;
using Utilities.GlobalHelpers;

namespace Infrastructure.Repositories
{
    public class FileException : Exception
    {
        public FileException(string message) : base(message) { }
    }

    public class FileRepository : IFileRepository
    {
        private readonly FileConfig _systemConfig;
        public FileRepository(IOptions<FileConfig> options)
        {
            _systemConfig = options.Value;
        }

        public async Task SaveFile(string path, byte[] data)
        {
            if (data == null || data.Length <= 0)
                throw new FileException("File must be contain data");

            if (!File.Exists(path))
                await File.WriteAllBytesAsync(path, data);
        }

        public string GetFullPath(string filePath)
        {
            var systemPath = GetSystemPath();

            if (!Directory.Exists(systemPath))
                Directory.CreateDirectory(systemPath);

            var fullPath = Path.Combine(systemPath, filePath);
            return fullPath;
        }

        public async Task<byte[]> GetFileBytes(string fileName)
        {
            var hostUrl = GetSystemPath();
            var fileUrl = Path.Combine(hostUrl, fileName);
           
            if (Directory.Exists(hostUrl) && File.Exists(fileUrl))
            {
                var fileBytes = await File.ReadAllBytesAsync(fileUrl);
                return fileBytes;
            }

            return default;
        }

        private string GetSystemPath()
        {
            return _systemConfig.ImagePath;
        }
    }
}