using Products.Context;
using Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductsDbContext _context;

        public ProductRepository(ProductsDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Create a new Product in database
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Created Product</returns>
        public async Task<ProductModel> CreateProductAsync(ProductModel product)
        {
            try
            {
                _context.Product.Add(product);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return product;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ProductModel> DeleteProductByIdAsync(Guid productId)
        {
            try
            {
                var product = await _context.Product.FindAsync(productId);

                if (product != null)
                {
                    _context.Product.Remove(product);
                    await _context.SaveChangesAsync();
                    return product;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task<List<ProductModel>> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public Task<ProductModel> GetProductByIdAsync(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductModel> UpdateProductAsync(ProductModel product)
        {
            throw new NotImplementedException();
        }
    }
}
