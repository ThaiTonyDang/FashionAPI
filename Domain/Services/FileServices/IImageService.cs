namespace Domain.Services.FileServices
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(Stream stream, string fileName);

        Task<byte[]> GetImageAsync(string fileName);
    }
}
