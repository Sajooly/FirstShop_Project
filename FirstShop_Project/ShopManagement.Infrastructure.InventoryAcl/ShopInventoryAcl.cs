using InventoryManagement.Application.Contract.Inventory;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;
using System;
using System.Collections.Generic;

namespace ShopManagement.Infrastructure.InventoryAcl
{
    public class ShopInventoryAcl : IShopInventoryAcl
    {
        private readonly IInventoryApplication _inventoryApplication;

        public ShopInventoryAcl(IInventoryApplication inventoryApplication)
        {
            _inventoryApplication = inventoryApplication;
        }

        public bool ReduceFromInventory(List<OrderItem> items)
        {
            var command = new List<DecreaseInventory>();

            foreach (var orderItem in items)
            {
                var item = new DecreaseInventory { ProductId = orderItem.ProductId, Count = orderItem.Count, Description = "خرید مشتری", OrderId = orderItem.OrderId };
                command.Add(item);
            }
            return _inventoryApplication.Decrease(command).IsSuccedded;
        }
    }
}
