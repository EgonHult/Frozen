using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Products.Context;
using Products.Models;
using Products.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsDbContext _context;
        private readonly IProductRepository _productsRepository;
        public ProductsController(ProductsDbContext context, IProductRepository productRepository)
        {
            _context = context;
            this._productsRepository = productRepository;
        }
        // GET: api/<ProductsController>
        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProductsAsync()
        {
            var result = await _productsRepository.GetAllProductsAsync();
            return Ok(result);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetProductAsync(Guid id)
        {
            if (ProductExists(id) == true)
            {
                var result = await _productsRepository.GetProductByIdAsync(id);
                if(result != null)
                {
                    return Ok(result);
                }
                return BadRequest();
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/<ProductsController>
        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<ActionResult<ProductModel>> PostProduct(ProductModel product)
        {
            if(product != null)
            {
                try
                {
                    var result = await _productsRepository.CreateProductAsync(product);
                    if(result != null)
                    {
                        return Ok(result);
                    }
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        // PUT api/<ProductsController>/5
        [Authorize(Roles = "Admin")]
        [HttpPut("edit/{id}")]
        public async Task<ActionResult<ProductModel>> UpdateProduct(Guid id, ProductModel product)
        {
            if(id != product.Id) { return BadRequest(); }
            try
            {
                var result = await _productsRepository.UpdateProductAsync(product);
                if (result != null)
                    return Ok(result);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [Authorize]
        [HttpPost("updatestock")]
        public async Task<ActionResult> UpdateProductQuantity(Dictionary<Guid, int> products)
        {
            var result = await _productsRepository.UpdateProductsInStockAsync(products);

            if (result)
                return Ok();
            else
                return BadRequest();
        }

        // DELETE api/<ProductsController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<ProductModel>> DeleteProduct(Guid id)
        {
            if (id != Guid.Empty)
            {
                var result = await _productsRepository.DeleteProductByIdAsync(id);

                if (result != null)
                    return Ok(result);
            }
            return BadRequest();
        }
        private bool ProductExists(Guid id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
