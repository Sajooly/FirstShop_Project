using _0_Framework.Domain;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Domain.InventoryAgg
{
    public class Inventory : EntityBase
    {
        public int ProductId { get; private set; }
        public double UnitPrice { get; private set; }
        public bool InStock { get; private set; }


        public List<InventoryOperation> Operations { get; private set; }


        protected Inventory()
        {
            Operations = new List<InventoryOperation>();
        }
        public Inventory(int productId, double unitPrice)
        {
            ProductId = productId;
            UnitPrice = unitPrice;
            InStock = false;
            Operations = new List<InventoryOperation>();
        }
        public void Edit(int productId, double unitPrice)
        {
            ProductId = productId;
            UnitPrice = unitPrice;
        }

        public int CalculateInStock()
        {
            var plus = Operations.Where(x => x.Operation).Select(x => x.Count).Sum();
            var minus = Operations.Where(x => !x.Operation).Sum(x => x.Count);
            return plus - minus;
        }

        public void Increase(int count, int operatorId, string description)
        {
            var currentCount = CalculateInStock() + count;
            Operations.Add(new InventoryOperation(true, count, operatorId, currentCount, description, 0, this.Id));
            InStock = currentCount > 0;
        }
        public void Reduction(int count, int operatorId, string description, int orderId)
        {
            var currentCount = CalculateInStock() - count;
            Operations.Add(new InventoryOperation(false, count, operatorId, currentCount, description, orderId, this.Id));
            InStock = currentCount > 0;
        }

    }
}