using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;

namespace ShoeStoreManagement.CRUD.Implementations
{
    public class ImageCRUD : IImageCRUD
    {
        private readonly ApplicationDbContext _applicationDBContext;

        public ImageCRUD(ApplicationDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

        public async Task CreateAsync(Image image)
        {
            await _applicationDBContext.Images.AddAsync(image);
            _applicationDBContext.SaveChanges();
        }

        public async Task<List<Image>> GetAllByProductIdAsync(string productId)
        {
            return await _applicationDBContext.Images.Where(x => x.ProductId == productId).ToListAsync();
        }

        public async Task<Image?> GetByIdAsync(string id)
        {
            return await _applicationDBContext.Images.Where(x => x.ImageId == id).FirstOrDefaultAsync();
        }

        public void Remove(Image deteleImage)
        {
            if (deteleImage != null)
                _applicationDBContext.Images.Remove(deteleImage);
            _applicationDBContext.SaveChanges();
        }

        public void Update(Image updateImage)
        {
            if (updateImage != null)
                _applicationDBContext.Images.Update(updateImage);
            _applicationDBContext.SaveChanges();
        }
    }
}
