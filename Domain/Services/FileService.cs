using Domain.Dtos;
using Domain.Services.FileServices;
using Infrastructure.Repositories;

namespace Domain.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IImageService _imageService;

        public FileService(IFileRepository fileRepository, IImageService imageService)
        {
            _fileRepository = fileRepository;
            this._imageService = imageService;
        }

        public async Task<string> SaveAsync(FileDto file)
        {
            var contentType = file.ContentType;
            if (contentType.Contains("image"))
                return await _imageService.SaveImageAsync(file.Stream, file.FileName);

            return file.FileName;
        }

        public async Task<byte[]> GetAsync(FileDto file)
        {
            var contentType = file.ContentType;
            if (contentType.Contains("image"))
                return await _imageService.GetImageAsync(file.FileName);

            return default;
        }

        public string GetFileLink(string domain, string sublink, string fullFileName) => $"{domain}{sublink}/{fullFileName}";
    }
}
