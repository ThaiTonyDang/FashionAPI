using Infrastructure.Repositories;
using Newtonsoft.Json.Serialization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Utilities.GlobalHelpers;

namespace Domain.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;

        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public string GetFileLink(string domain, string sublink, string fullFileName) => $"{domain}{sublink}/{fullFileName}";

        public string GetFullFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return string.Empty;

            var dateTime = DateTime.Now.ToString("yyyyMMddhhmmss");
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var extensions = Path.GetExtension(fileName);

            var filePath = fileNameWithoutExtension.Replace(" ", "").Trim() + dateTime + extensions;
            return filePath;
        }

        public async Task SaveFileAsync(string fileName, byte[] data, string fileFolder)
        {
            var fullPath = _fileRepository.GetFullPath(fileName, fileFolder);
            await this._fileRepository.SaveFile(fullPath, data);
        }

        public async Task<byte[]> GetFileBytesAsync(string fileName, string fileFolder)
        {
            var fileBytes = await _fileRepository.GetFileBytes(fileName, fileFolder);
            return fileBytes;
        }

        public void ResizeImage(Stream stream, string fullFileName, string fileFolder)
        {
            using (var image = Image.Load(stream))
            {
                image.Mutate(x => x.Resize(SIZE.Width, SIZE.Height));
                var fullPath = GetImageFullPath(fullFileName, fileFolder);
                image.Save(fullPath);
            }           
        }

        public string GetFullImageName(string fileName, int width, int height)
        {
            if (string.IsNullOrEmpty(fileName))
                return string.Empty;

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var extensions = Path.GetExtension(fileName);

            fileName = fileNameWithoutExtension.Replace(" ", "").Trim() + $"-{Guid.NewGuid()}-w{width}-h{height}-{DateTime.Now.ToString("yyyyMMddhhmmss")}" + extensions;
            return fileName;
        }

        private string GetImageFullPath(string imageName, string fileFolder)
        {
            var path = _fileRepository.GetFullPath(imageName, fileFolder);
            return path;
        }
    }
}
