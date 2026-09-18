using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Infrastructure.EFCore;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;
using System.Collections.Generic;
using System.Linq;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class OrderRepository : RepositoryBase<int, Order>, IOrderRepository
    {
        private readonly ShopContext _shopContext;
        private readonly AccountContext _accountContext;

        public OrderRepository(ShopContext shopContext, AccountContext accountContext) : base(shopContext)
        {
            _shopContext = shopContext;
            _accountContext = accountContext;
        }

        public bool CheckOrderIdAuthority(long id)
        {
            return _shopContext.Orders.Any(x => x.RefId == id);
        }

        public double GetAmountBy(int id)
        {
            return _shopContext.Orders.Select(x => new { x.Id, x.PayAmount }).FirstOrDefault(x => x.Id == id)?.PayAmount ?? 0;
            //return _shopContext.Orders.FirstOrDefault(x => x.Id == id).PayAmount;
        }

        public List<OrderItemViewModel> GetOrderItemsBy(int orderId)
        {
            var products = _shopContext.Products.Select(x => new { x.Id, x.Name }).ToList();

            var order = _shopContext.Orders.FirstOrDefault(x => x.Id == orderId);
            if (order == null)
                return new List<OrderItemViewModel>();


            var item = order.Items.Select(x => new OrderItemViewModel
            {
                Id = x.Id,
                DiscountRate = x.DiscountRate,
                Count = x.Count,
                UnitPrice = x.UnitPrice,
                OrderId = x.OrderId,
                ProductId = x.ProductId
            }).ToList();

            item.ForEach(i => i.ProductName = products.FirstOrDefault(p => p.Id == i.ProductId)?.Name);

            return item;
        }

        public List<OrderViewModel> Search(OrderSearchModel searchModel)
        {
            var account = _accountContext.Accounts.Select(x => new { x.Id, x.FullName }).ToList();

            var query = _shopContext.Orders.Select(x => new OrderViewModel
            {
                Id = x.Id,
                IsCanceled = x.IsCanceled,
                IsPaid = x.IsPaid,
                IssueTrackingNo = x.IssueTrackingNo,
                DiscountAmount = x.DiscountAmount,
                TotalAmount = x.TotalAmount,
                RefId = x.RefId,
                PayAmount = x.PayAmount,
                AccountId = x.AccountId,
                CreationDate = x.CreationDate.ToFarsi()
            });

            //if (searchModel.IsCanceled == true) //بجای این از کد زیر استفاده میکنیم
            //    orders.Where(x => x.IsCanceled);
            query = query.Where(x => x.IsCanceled == searchModel.IsCanceled);


            if (!string.IsNullOrWhiteSpace(searchModel.IssueTrackingNo))
                query = query.Where(x => x.IssueTrackingNo.Contains(searchModel.IssueTrackingNo));


            if (!string.IsNullOrWhiteSpace(searchModel.RefId))
                query = query.Where(x => x.RefId.ToString().Contains(searchModel.RefId));

            if (searchModel.AccountId > 0)
                query = query.Where(x => x.AccountId == searchModel.AccountId);


            var orders = query.ToList();


            orders.ForEach(x => x.AccountFullName = account.FirstOrDefault(a => a.Id == x.AccountId)?.FullName);

            return orders;

        }
    }
}
