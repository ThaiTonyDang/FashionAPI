using Domain.Dtos;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IFileService
    {
        public string GetFileLink(string domain, string sublink, string fullFileName);
        public Task<string> SaveAsync(FileDto file);
        public Task<byte[]> GetAsync(FileDto file);
    }
}
