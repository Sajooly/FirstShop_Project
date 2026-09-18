using _0_Framework.Application;
using Microsoft.Extensions.Configuration;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;
using System.Collections.Generic;

namespace ShopManagement.Application
{
    public class OrderApplication : IOrderApplication
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAuthHelper _authHelper;
        private readonly IConfiguration _configuration;
        private readonly IShopInventoryAcl _shopInventoryAcl;

        public OrderApplication(IOrderRepository orderRepository, IAuthHelper authHelper, IConfiguration configuration, IShopInventoryAcl shopInventoryAcl)
        {
            _orderRepository = orderRepository;
            _authHelper = authHelper;
            _configuration = configuration;
            _shopInventoryAcl = shopInventoryAcl;
        }

        public void Cancel(int orderId)
        {
            var order = _orderRepository.GetBy(x => x.Id == orderId);
            //var order=_orderRepository.GetBy(orderId);


            if (order == null)
                return;

            order.Cancel();
            _orderRepository.SaveChanges();
        }

        public bool CheckOrderIdAuthority(long id)
        {
            return _orderRepository.CheckOrderIdAuthority(id);
        }

        public double GetAmountBy(int id)
        {
            return _orderRepository.GetAmountBy(id);
        }

        public List<OrderItemViewModel> GetOrderItemsBy(int orderId)
        {
            return _orderRepository.GetOrderItemsBy(orderId);
        }

        public string PaymentSucceded(int id, long refId)
        {
            var order = _orderRepository.GetBy(id);
            order.PaymentSucceded(refId);

            var symbol = _configuration.GetValue<string>("Symbol");
            var issueTrackingNo = CodeGenerator.Generate(symbol);

            order.SetIssueTrackingNumber(issueTrackingNo);

            if (_shopInventoryAcl.ReduceFromInventory(order.Items))
            {
                _orderRepository.SaveChanges();

                return issueTrackingNo;
            }
            return "";
        }

        public int PlaceOrder(Cart cart)
        {
            var currentAccountId = _authHelper.CurrentAccountId();

            var order = new Order(currentAccountId, cart.TotalAmount, cart.DiscountAmount, cart.PayAmount);


            foreach (var item in cart.Items)
            {
                var orderItem = new OrderItem(item.Id, item.Count, double.Parse(item.UnitPrice.Replace("٬", "")), item.DiscountRate);
                order.AddItem(orderItem);
            }

            _orderRepository.Create(order);
            _orderRepository.SaveChanges();
            return order.Id;
        }

        public List<OrderViewModel> Search(OrderSearchModel searchModel)
        {
            return _orderRepository.Search(searchModel);
        }
    }
}
