using ShoeStoreManagement.Core.Models;

namespace ShoeStoreManagement.CRUD.Interfaces
{
    public interface IImageCRUD
    {
        public Task<List<Image>> GetAllByProductIdAsync(string productId);
        public Task<Image?> GetByIdAsync(string id);
        public Task CreateAsync(Image image);
        public void Update(Image updateImage);
        public void Remove(Image deteleImage);
    }
}
