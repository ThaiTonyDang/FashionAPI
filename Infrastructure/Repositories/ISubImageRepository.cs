using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface ISubImageRepository
    {
        public Task<bool> UpdateSubImage(SubImage subImage);
    }
}
