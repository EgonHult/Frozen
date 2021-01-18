using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class OrdersController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly IOrderRepository _orderRepository;

        public OrdersController(OrderDbContext context, IOrderRepository orderRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
        }


        // GET: api/Orders
        [Authorize(Roles = "Admin")]
        [HttpGet("getall")]
        public async Task<ActionResult<List<OrderModel>>> GetOrders()
        {
            var result = await _orderRepository.GetAllOrdersAsync();
            return Ok(result.OrderByDescending(x => x.Date));
        }

        // GET: api/Orders/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderModel>> GetOrder(Guid id)
        {
            var orderExist = OrderExists(id);

            if (!orderExist)
                return NotFound();

            var result = await _orderRepository.GetOrderByOrderIdAsync(id);

            if (result != null)
                return Ok(result);

            return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{statusId}/{id}")]
        public async Task<ActionResult<OrderModel>> OrderStatus(int statusId, Guid id)
        {
            var orderExist = OrderExists(id);

            if (!orderExist)
                return NotFound();

            var result = await _orderRepository.UpdateOrderStatusAsync(statusId, id);

            if (result)
                return Ok(true);

            return BadRequest();
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(OrderModel order)
        {
            if (order.Id == Guid.Empty)
                return BadRequest();

            try
            {
                var result = await _orderRepository.UpdateOrderAsync(order);
                if (result != null)
                    return Ok(result);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.Id))
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

        // GET: api/Orders/user/id      
        [Authorize]
        [HttpGet("user/{id}")]
        public async Task<ActionResult<List<OrderModel>>> GetOrdersByUserId(Guid id)
        {
            var orderExist = OrdersWithUserIdExists(id);

            if (!orderExist)
                return NotFound();

            var result = await _orderRepository.GetOrdersByUserIdAsync(id);

            if (result != null)
                return Ok(result);

            return BadRequest();

        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<OrderModel>> PostOrder(OrderModel order)
        {
            if (order != null)
            {
                try
                {
                    var newOrder = await _orderRepository.CreateOrderAsync(order);
                    if (newOrder != null)
                        return Ok(newOrder);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }

            return BadRequest();
        }

        // DELETE: api/Orders/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderModel>> DeleteOrder(Guid id)
        {
            if (id != Guid.Empty)
            {
                var result = await _orderRepository.DeleteOrderByOrderIdAsync(id);

                if (result != null)
                    return Ok(result);
            }

            return BadRequest();
        }


        private bool OrderExists(Guid id)
        {
            return _context.Order.Any(e => e.Id == id);
        }

        private bool OrdersWithUserIdExists(Guid userId)
        {
            return _context.Order.Any(x => x.UserId == userId);
        }

    }
}
