using System;

namespace ShopManagement.Application.Contracts.Order
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UnitPrice { get; set; }
        public string Picture { get; set; }
        public int Count { get; set; }
        public double TotalItemPrice { get; set; }
        public bool IsInStock { get; set; }


        public int DiscountRate { get; set; }
        public double DiscountAmount { get; set; }
        public double ItemPayAmount { get; set; }


        public void CalculateTotalItemPrice()
        {
            var price = double.Parse(UnitPrice.Replace("٬", ""));
            TotalItemPrice = price * Count;
        }
    }
}
