using Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Repositories
{
    public interface IProductRepository
    {
        Task<ProductModel> CreateProductAsync(ProductModel product);
        Task<ProductModel> UpdateProductAsync(ProductModel product);
        Task<ProductModel> DeleteProductByIdAsync(Guid productId);
        Task<ProductModel> GetProductByIdAsync(Guid productId);
        Task<List<ProductModel>> GetAllProductsAsync();
        Task<bool> UpdateProductsInStockAsync(Dictionary<Guid, int> products);
    }
}
