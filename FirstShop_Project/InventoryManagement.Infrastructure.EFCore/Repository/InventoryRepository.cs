using _0_Framework.Application;
using _0_Framework.Infrastructure;
using InventoryManagement.Application.Contract.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using ShopManagement.Infrastructure.EFCore;
using System.Collections.Generic;
using System.Linq;

namespace InventoryManagement.Infrastructure.EFCore.Repository
{
    public class InventoryRepository : RepositoryBase<int, Inventory>, IInventoryRepository
    {
        private readonly InventoryContext _context;
        private readonly ShopContext _shopContext;

        public InventoryRepository(InventoryContext context, ShopContext shopContext) : base(context)
        {
            _context = context;
            _shopContext = shopContext;
        }

        public EditInventory GetDetails(int id)
        {
            return _context.Inventory.Select(x => new EditInventory
            {
                Id = x.Id,
                UnitPrice = x.UnitPrice,
                ProductId = x.ProductId
            }).FirstOrDefault(x => x.Id == id);
        }

        public Inventory GetInventoryBy(int ProductId)
        {
            return _context.Inventory.FirstOrDefault(x => x.ProductId == ProductId);
        }

        public List<InventoryOperationViewModel> GetOperationBy(int inventoryId)
        {
            return _context.Inventory
                 .Where(x => x.Id == inventoryId)
                 .SelectMany(x => x.Operations.Select(o => new InventoryOperationViewModel
                 {
                     Id = o.Id,
                     Count = o.Count,
                     CurrentCount = o.CurrentCount,
                     Description = o.Description,
                     Operation = o.Operation,
                     OperationDate = o.OperationDate.ToFarsi(),
                     OperatorName = "مدیر سیستم",
                     OperatorId = o.OperatorId,
                     OrderId = o.OrderId,
                 }))
                 .ToList()
                 .OrderByDescending(x => x.Id)
                 .ToList();

        }

        public List<InventoryViewModel> Search(InventorySearchModel searchModel)
        {
            var products = _shopContext.Products.Select(x => new { x.Id, x.Name }).ToList();

            var query = _context.Inventory.Select(x => new InventoryViewModel
            {
                Id = x.Id,
                UnitPrice = x.UnitPrice,
                InStock = x.InStock,
                ProductId = x.ProductId,
                CurrentCount = x.CalculateInStock(),
                CreationDate = x.CreationDate.ToFarsi()
            });

            if (searchModel.ProductId > 0)
                query = query.Where(x => x.ProductId == searchModel.ProductId);

            if (searchModel.InStock)
                query = query.Where(x => !x.InStock);

            var result = query.OrderByDescending(x => x.Id).ToList();

            result.ForEach(x =>
            {
                x.ProductName = products.FirstOrDefault(p => p.Id == x.ProductId)?.Name;
            });

            return result;
        }
    }
}
