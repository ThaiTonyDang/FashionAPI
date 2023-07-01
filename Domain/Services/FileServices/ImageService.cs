using Infrastructure.Repositories;
using Utilities.GlobalHelpers;

namespace Domain.Services.FileServices
{
    public class ImageService : IImageService
    {
        private readonly IFileRepository _fileRepository;
        public ImageService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }
        public async Task<string> SaveImageAsync(Stream stream, string fileName)
        {
            var fullFileName = this.GetFullImageName(fileName, SIZE.Width, SIZE.Height);

            var folderPath = GetFolderImageName(fullFileName);
            var fullFilePath = _fileRepository.GetFullPath(fullFileName, folderPath);

            using (var image = await Image.LoadAsync(stream))
            {
                image.Mutate(x => x.Resize(SIZE.Width, SIZE.Height));
                await image.SaveAsync(fullFilePath);
            }

            return fullFileName;
        }

        public async Task<byte[]> GetImageAsync(string fileName)
        {
            var folder = GetFolderImageName(fileName);
            var data = await _fileRepository.GetFileBytes(fileName, folder);
            return data;
        }

        private string GetFullImageName(string fileName, int width, int height)
        {
            if (string.IsNullOrEmpty(fileName))
                return string.Empty;

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var extensions = Path.GetExtension(fileName);
            var dateTime = DateTime.Now.ToString("yyyyMMddhhmmss");
            var size = $"w{width}-h{height}";
            var fileNameRefactor = fileNameWithoutExtension.Replace(" ", "").Trim();
            var id = Guid.NewGuid();

            var result = $"{fileNameRefactor}-{id}-{size}-{dateTime}{extensions}";
            return result;
        }

        private string GetFolderImageName(string fileName)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var resultstring = new string(fileNameWithoutExtension.ToCharArray().Reverse().ToArray());
            var folderName = new string(resultstring.Substring(6, 8).ToCharArray().Reverse().ToArray());
            return folderName;
        }
    }
}
