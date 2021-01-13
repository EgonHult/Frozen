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

        public async Task<OrderModel> CreateOrderAsync(OrderModel order)
        {
            if (order != null)
            {
                bool orderExistInDatabase = await CheckIfOrderExistInDatabaseAsync(order.Id);

                if (!orderExistInDatabase)
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
                else
                    return null;
            }
            else
                return null;
        }

        public async Task<OrderModel> DeleteOrderByOrderIdAsync(Guid orderId)
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

        public async Task<List<OrderModel>> GetAllOrdersAsync()
        {
            var result = await _context.Order.OrderBy(x => x.Date).Include(x => x.Status).ToListAsync();
            return result;
        }

        public async Task<OrderModel> GetOrderByOrderIdAsync(Guid orderId)
        {
          
            if (orderId == Guid.Empty)
                return null;

            var order = await _context.Order.Where(x => x.Id == orderId).Include(x => x.Status).Include(x => x.OrderProduct).FirstOrDefaultAsync();
                return order ?? null;           
        }

        public async Task<List<OrderModel>> GetOrdersByUserIdAsync(Guid userId)
        {
            bool userWithOrdersExistInDatabase = await _context.Order.AnyAsync(x => x.UserId == userId);

            if (!userWithOrdersExistInDatabase)
                return null;

            var order = await _context.Order.Where(x => x.UserId == userId).Include(x => x.Status).ToListAsync();
                return order;
        }

        public async Task<OrderModel> UpdateOrderAsync(OrderModel order)
        {       
            bool orderExistInDatabase = await CheckIfOrderExistInDatabaseAsync(order.Id);

            if (orderExistInDatabase && order.Id != Guid.Empty)
            {
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return order;
            }
            else
                return null;
        }

        public async Task<bool> CheckIfOrderExistInDatabaseAsync(Guid id)
        => await _context.Order.AnyAsync(x => x.Id == id);
    }
}
