using Infrastructure.Repositories;
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

        public Task<string> GetImagePath(string fileName)
        {
            // image path --> full path
            if (string.IsNullOrEmpty(fileName))
                return Task.FromResult(DISPLAY.DEFAULT_IMAGE);

            var path = _fileRepository.GetFilePath(fileName);

            // imagePath, folder, 
            var fullPath = GetFullPath(path, "Product");

            return Task.FromResult(path);
        }

        public string GetFullPath(string imagePath, string folder)
        {
            // GET FULL PATH
            // call repository
            return fullPath;
        }
    }
}
