using Infrastructure.DataContext;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SubImageRepository : ISubImageRepository
    {
        private readonly AppDbContext _appDbContext;
        public SubImageRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<bool> UpdateSubImage(SubImage subImage)
        {
            var subEntity = await _appDbContext.SubImages.FindAsync(subImage.Id);
            if (subEntity == null)
            {
                await _appDbContext.SubImages.AddAsync(subImage);
                var result = await _appDbContext.SaveChangesAsync();
                return (result > 0);
            }

            subEntity.Id = subImage.Id;
            subEntity.ImageName = subImage.ImageName;
            subEntity.ProductId = subImage.ProductId;
            _appDbContext.SubImages.Update(subEntity);
            var resultUpdate = await _appDbContext.SaveChangesAsync();
            return (resultUpdate > 0);
        }
    }
}
