﻿using ShoeStoreManagement.Core.Models;

namespace ShoeStoreManagement.CRUD.Interfaces
{
    public interface ICartDetailCRUD
    {
        public Task<List<CartDetail>> GetAllAsync(string cartId);
        public Task<CartDetail?> GetByIdAsync(string id);
        public Task CreateAsync(CartDetail cartDetail);
        public void Update(CartDetail updateCartDetail);
        public void Remove(CartDetail deteleCartDetail);
    }
}