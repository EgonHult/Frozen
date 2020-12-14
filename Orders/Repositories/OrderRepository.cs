using Microsoft.EntityFrameworkCore;
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

        public async Task<Order> DeleteOrderByIdAsync(Guid orderId)
        {
            try
            {
                var order = await _context.Order.FindAsync(orderId);

                if (order != null)
                {
                    _context.Order.Remove(order);
                    await _context.SaveChangesAsync();
                    return order;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            var result = await _context.Order.OrderBy(x => x.StatusId).ToListAsync();
            return result;
        }


        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            if(orderId == Guid.Empty)
                return null;
           
            var order = await _context.Order.FindAsync(orderId);

            return order ?? null;           
        }
        public async Task<List<Order>> GetAllOrdersByUserIdAsync(Guid userId)
        {
            bool userWithOrdersExistInDatabase = await _context.Order.AnyAsync(x => x.UserId == userId);

            if (!userWithOrdersExistInDatabase)
                return null;
            var order = await _context.Order.Where(x => x.UserId == userId).ToListAsync();
                return order;

        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            bool orderExistInDatabase  = await _context.Order.AnyAsync(x => x.Id == order.Id);

            if (orderExistInDatabase && order.Id != Guid.Empty)
            {
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return order;
            }
            else
                return null;
        }
    }
}
