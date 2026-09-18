using _0_Framework.Domain;
using System.Collections.Generic;

namespace ShopManagement.Domain.OrderAgg
{
    public class Order : EntityBase
    {
        public int AccountId { get; private set; }
        public double TotalAmount { get; private set; }
        public double DiscountAmount { get; private set; }
        public double PayAmount { get; private set; }
        public bool IsPaid { get; private set; }
        public bool IsCanceled { get; private set; }
        public string IssueTrackingNo { get; private set; }
        public long RefId { get; private set; }
        public List<OrderItem> Items { get; private set; }

        protected Order()
        {
            Items = new List<OrderItem>();
        }
        public Order(int accountId, double totalAmount, double discountAmount, double payAmount)
        {
            AccountId = accountId;
            TotalAmount = totalAmount;
            DiscountAmount = discountAmount;
            PayAmount = payAmount;
            IsPaid = false;
            IsCanceled = false;
            RefId = 0;
            Items = new List<OrderItem>();
        }

        public void PaymentSucceded(long refId)
        {
            IsPaid = true;

            if (refId != 0)
                RefId = refId;
        }
        public void Cancel()
        {
            IsCanceled = true;
        }
        public void SetIssueTrackingNumber(string number)
        {
            IssueTrackingNo = number;
        }
        public void AddItem(OrderItem item)
        {
            Items.Add(item);
        }
    }

    public class OrderItem : EntityBase
    {
        public int ProductId { get; private set; }
        public int Count { get; private set; }
        public double UnitPrice { get; private set; }
        public int DiscountRate { get; private set; }



        //Navigation Property For Relationship
        public Order Order { get; private set; }
        public int OrderId { get;private set; }

        public OrderItem(int productId, int count, double unitPrice, int discountRate)
        {
            ProductId = productId;
            Count = count;
            UnitPrice = unitPrice;
            DiscountRate = discountRate;
        }
    }
}
