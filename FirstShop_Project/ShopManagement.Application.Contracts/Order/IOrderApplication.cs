using System.Collections.Generic;

namespace ShopManagement.Application.Contracts.Order
{
    public interface IOrderApplication
    {
        int PlaceOrder(Cart cart);

        string PaymentSucceded(int id, long refId);
        double GetAmountBy(int id);
        bool CheckOrderIdAuthority(long id);


        List<OrderViewModel> Search(OrderSearchModel searchModel);
        void Cancel(int orderId);
        List<OrderItemViewModel> GetOrderItemsBy(int orderId);
    }
}
