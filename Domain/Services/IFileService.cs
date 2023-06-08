using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IFileService
    {
        public string GetFileLink(string domain, string sublink, string fullFileName);
        public Task SaveFileAsync(string fileName, byte[] data, string fileFolder);
        public string GetFullFileName(string fileName);
        public Task<byte[]> GetFileBytesAsync(string fileName, string fileFolder);
        public string GetFullImageName(string imageName, int width, int height);
        public void ResizeImage(Stream stream, string fullFileName, string fileFolder);       
    }
}
