using Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task<Order> DeleteOrderByIdAsync(Guid orderId);
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<List<Order>> GetAllOrdersAsync();
        Task<List<Order>> GetAllOrdersByUserIdAsync(Guid userId);

    }
}
