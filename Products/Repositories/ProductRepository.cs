using Microsoft.EntityFrameworkCore;
using Products.Context;
using Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

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
            if (product != null)
            {
                bool productExistInDataBase = await CheckIfProductExistInDatabaseAsync(product.Id);
                if (!productExistInDataBase)
                {
                    _context.Product.Add(product);
                    var result = await _context.SaveChangesAsync();

                    if (result > 0)
                        return product;
                    else
                        return null;
                }
                else
                    return null;
            }
            else
                return null;
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

        public async Task<List<ProductModel>> GetAllProductsAsync()
        {
           
            var listOfProducts = await _context.Product.OrderBy(x => x.Name).ToListAsync();
            return listOfProducts;
        }

        public async Task<ProductModel> GetProductByIdAsync(Guid productId)
        {
            if (productId == Guid.Empty)
                return null;

            var product = await _context.Product.FindAsync(productId);
            return product;
        }

        public async Task<ProductModel> UpdateProductAsync(ProductModel product)
        {
            if (product != null)
            {
                bool productExistInDatabase = await CheckIfProductExistInDatabaseAsync(product.Id);

                if (productExistInDatabase && product.Id != Guid.Empty)
                {
                    _context.Entry(product).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return product;
                }
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// Remove items in stock by providing a Dictionary<Guid, int>() that represents
        /// product id and quantity to be reduced in database
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public async Task<bool> UpdateProductsInStockAsync(Dictionary<Guid, int> products)
        {
            try
            {
                // Start a transaction that will roll back all changes in case of SQL-failure!
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var item in products)
                    {
                        var product = await GetProductByIdAsync(item.Key);
                        product.Quantity -= item.Value;

                        if (product.Quantity < 0)
                            product.Quantity = 0;

                        await UpdateProductAsync(product);
                    }

                    transaction.Complete();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CheckIfProductExistInDatabaseAsync(Guid id)
            => await _context.Product.AnyAsync(x => x.Id == id);
    }
}
