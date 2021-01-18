using Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Repositories
{
    public interface IOrderRepository
    {
        Task<OrderModel> CreateOrderAsync(OrderModel order);
        Task<OrderModel> UpdateOrderAsync(OrderModel order);
        Task<OrderModel> DeleteOrderByOrderIdAsync(Guid orderId);
        Task<OrderModel> GetOrderByOrderIdAsync(Guid orderId);
        Task<List<OrderModel>> GetAllOrdersAsync();
        Task<List<OrderModel>> GetOrdersByUserIdAsync(Guid userId);
        Task<bool> UpdateOrderStatusAsync(int statusId, Guid orderId);
    }
}
