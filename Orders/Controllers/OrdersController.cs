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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var result = await _orderRepository.GetAllOrdersAsync();
            return Ok(result);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var orderExist = OrderExists(id);

            if (!orderExist)
                return NotFound();

            var result = await _orderRepository.GetOrderByIdAsync(id);

            if (result != null)
                return Ok(result);

            return BadRequest();
        }

        

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(Order order)
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


        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("create")]
        public async Task<ActionResult<Order>> PostOrder(Order order)
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
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(Guid id)
        {
            if (id != Guid.Empty)
            {
                var result = await _orderRepository.DeleteOrderByIdAsync(id);

                if (result != null)
                    return Ok(result);
            }

            return BadRequest();
        }


        private bool OrderExists(Guid id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
