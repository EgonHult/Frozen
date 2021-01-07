using Microsoft.AspNetCore.Mvc;
using Orders.Context;
using Orders.Models;
using Orders.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrderProductsController : ControllerBase
    {
        private readonly OrderDbContext _dbContext;
        private readonly IOrderRepository _orderRepository;

        public OrderProductsController(OrderDbContext dbContext, IOrderRepository orderRepository)
        {
            this._dbContext = dbContext;
            this._orderRepository = orderRepository;
        }

        // POST: api/OrderProducts
        [HttpPost]
        public async Task<ActionResult<Order>> CreateNewOrderAsync(Order order)
        {
            try
            {
                var result = await _orderRepository.CreateOrderAsync(order);
                if (result != null)
                    return result;
            }
            catch (Exception)
            {

                throw;
            }
            return BadRequest();
        }

        // Put: api/OrderProducts
        [HttpPut]
        public async Task<ActionResult<Order>> UpdateOrderAsync(Order order)
        {
            try
            {
                var result = await _orderRepository.UpdateOrderAsync(order);
                if (result != null)
                    return result;
            }
            catch (Exception)
            {
                if (!OrderExists(order))
                    return BadRequest();
                throw;
            }
            return BadRequest();

        }
        // DELETE: api/OrderProducts/2
        [HttpDelete("{orderId}")]
        public async Task<ActionResult<Order>> DeleteOrderByIdAsync(Guid orderId)
        {
            try
            {
                var result = await _orderRepository.DeleteOrderByIdAsync(orderId);
                if (result != null)
                    return result;
            }
            catch (Exception)
            {
                if (!OrderByIdExists(orderId))
                    return BadRequest();
                throw;
            }
            return BadRequest();
        }

        // GET: api/OrderProducts/2
        [HttpGet("{orderId}")]
        public async Task<ActionResult<Order>>GetOrderByIdAsync(Guid orderId)
        {
            try
            {
                var result = await _orderRepository.GetOrderByIdAsync(orderId);
                if (result != null)
                    return result;
            }
            catch (Exception)
            {
                if (!OrderByIdExists(orderId))
                    return BadRequest();
                throw;
            }
            return BadRequest();
        }

        // GET: api/OrderProducts
        [HttpGet]
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            var result = await _orderRepository.GetAllOrdersAsync();
            if (result != null) 
                return result;
           
            return null;
        }


        private bool OrderExists(Order Order)
        {
            return _dbContext.Order.Any(e => e.Id == Order.Id);
        }
        private bool OrderByIdExists(Guid id)
        {
            return _dbContext.Order.Any(e => e.Id == id);
        }
    }
}
