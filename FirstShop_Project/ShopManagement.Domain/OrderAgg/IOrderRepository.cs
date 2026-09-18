using _0_Framework.Domain;
using ShopManagement.Application.Contracts.Order;
using System.Collections.Generic;

namespace ShopManagement.Domain.OrderAgg
{
    public interface IOrderRepository : IRepository<int, Order>
    {
        double GetAmountBy(int id);
        bool CheckOrderIdAuthority(long id);
        List<OrderViewModel> Search(OrderSearchModel searchModel);
        List<OrderItemViewModel> GetOrderItemsBy(int orderId);
    }
}
