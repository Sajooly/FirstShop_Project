using InventoryManagement.Application.Contract.Inventory;
using System.Collections.Generic;
using _0_Framework.Application;
using InventoryManagement.Domain.InventoryAgg;

namespace InventoryManagement.Application
{
    public class InventoryApplication : IInventoryApplication
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryApplication(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public OperationResult CreateInventory(CreateInventory command)
        {
            var operation = new OperationResult();

            if (_inventoryRepository.Exists(x => x.ProductId == command.ProductId))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            _inventoryRepository.Create(new Inventory(command.ProductId, command.UnitPrice));
            _inventoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Decrease(List<DecreaseInventory> command)
        {
            var operation = new OperationResult();
            const int operatorId = 1;

            foreach (var item in command)
            {
                var inventory = _inventoryRepository.GetInventoryBy(item.ProductId);
                if (inventory != null)
                    inventory.Reduction(item.Count, operatorId, item.Description, item.OrderId);
            }
            _inventoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Decrease(DecreaseInventory command)
        {
            var operation = new OperationResult();

            var inventory = _inventoryRepository.GetBy(command.InventoryId);

            if (inventory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            const int operatorId = 1; //چون اینجا هنوز سیستم مدیریت کاربران نساختیم این رو اینجا مینویسیم و بعدش ک سیستم  اکانتینگ رو ساختیم این رو درست میکنیم.
            inventory.Reduction(command.Count, operatorId, command.Description, 0);
            _inventoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult EditInventory(EditInventory command)
        {
            var operation = new OperationResult();
            var inventory = _inventoryRepository.GetBy(command.Id);

            if (inventory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_inventoryRepository.Exists(x => x.ProductId == command.ProductId && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            inventory.Edit(command.ProductId, command.UnitPrice);
            _inventoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public EditInventory GetDetails(int id)
        {
            return _inventoryRepository.GetDetails(id);
        }

        public List<InventoryOperationViewModel> GetOperationLogBy(int inventoryId)
        {
            return _inventoryRepository.GetOperationBy(inventoryId);
        }

        public OperationResult Increase(IncreaseInventory command)
        {
            var operation = new OperationResult();

            var inventory = _inventoryRepository.GetBy(command.InventoryId);

            if (inventory == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            const int operatorId = 1; //چون اینجا هنوز سیستم مدیریت کاربران نساختیم این رو اینجا مینویسیم و بعدش ک سیستم  اکانتینگ رو ساختیم این رو درست میکنیم.
            inventory.Increase(command.Count, operatorId, command.Description);
            _inventoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public List<InventoryViewModel> Search(InventorySearchModel searchModel)
        {
            return _inventoryRepository.Search(searchModel);
        }
    }
}
