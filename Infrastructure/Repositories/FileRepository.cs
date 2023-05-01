using Infrastructure.Config;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;
using Utilities.GlobalHelpers;
using static System.Net.Mime.MediaTypeNames;

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

        public async Task SaveFile(string folder, string imagePath, byte[] data)
        {
            if (data == null || data.Length <= 0)
                throw new FileException("File must be contain data");


            if (!File.Exists(fullPath))
                await File.WriteAllBytesAsync(fullPath, data);
        }

        private async Task GetFullPath(string imagePath, string folder)
        {
            var systemPath = GetSystemPath();
            var imageFolder = Path.Combine(systemPath, folder);

            if (!Directory.Exists(systemPath) || !Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);

            var fullPath = Path.Combine(imageFolder, imagePath);
        }

        public string GetFilePath(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return string.Empty;

            var dateTime = DateTime.Now.ToString("yyyyMMddhhmmss");
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var extensions = Path.GetExtension(fileName);

            var filePath = fileNameWithoutExtension.Replace(" ", "").Trim() + dateTime + extensions;
            return filePath;
        }

        public string GetSystemPath()
        {
            return _systemConfig.ImagePath;
        }

        
    }
}