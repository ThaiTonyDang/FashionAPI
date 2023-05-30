using Infrastructure.Repositories;

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

            var dateTime = DateTime.Now.ToString("yyyyMMddhhmmss");
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
    }
}
