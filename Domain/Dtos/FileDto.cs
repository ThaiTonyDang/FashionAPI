using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class FileDto
    {
        public string FileName { get; set; }
        public Stream Stream { get; set; }
        public byte[] Data { get; set; }
        public long ContentLength { get; set; }
        public string ContentType { get; set; }
    }
}
