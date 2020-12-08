using Orders.Context;
using Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }


        public async Task<Order> CreateOrderAsync(Order order)
        {
            try
            {
                 _context.Order.Add(order);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                    return order;
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }

            
        }

        public Task<Order> DeleteOrderAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetAllOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<Order> UpdateOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
