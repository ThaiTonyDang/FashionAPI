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

        public string GetFileLink(string domain, string resource, string fullFileName) => $"{domain}{resource}/{fullFileName}";

        public string GetFullFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return string.Empty;

            var dateTime = DateTime.Now.ToString("yyyyMMdd");
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var extensions = Path.GetExtension(fileName);

            var filePath = fileNameWithoutExtension.Replace(" ", "").Trim() + dateTime + extensions;
            return filePath;
        }

        public async Task SaveFileAsync(string fileName, byte[] data)
        {
            var path = _fileRepository.GetFullPath(fileName);
            await this._fileRepository.SaveFile(path, data);
        }

        public async Task<byte[]> GetFileBytesAsync(string fileName)
        {
            var fileBytes = await _fileRepository.GetFileBytes(fileName);
            return fileBytes;
        }

        public void ResizeImage(Stream stream, string fullFileName)
        {
            using (var image = Image.Load(stream))
            {
                image.Mutate(x => x.Resize(SIZE.Width, SIZE.Height));
                var fullPath = GetImageFullPath(fullFileName);
                image.Save(fullPath);
            }           
        }

        public string GetFullImageName(string imageName, int width, int height)
        {
            if (string.IsNullOrEmpty(imageName))
                return string.Empty;

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imageName);
            var extensions = Path.GetExtension(imageName);

            var filePath = fileNameWithoutExtension.Replace(" ", "").Trim() + $"-w{width}-h{height}-{DateTime.Now.ToString("hhmmss")}" + extensions;
            return filePath;
        }

        private string GetImageFullPath(string imageName)
        {
            var path = _fileRepository.GetFullPath(imageName);
            return path;
        }
    }
}
