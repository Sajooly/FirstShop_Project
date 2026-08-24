using _0_Framework.Application;
using System;
using System.Collections.Generic;

namespace InventoryManagement.Application.Contract.Inventory
{
    public interface IInventoryApplication
    {
        OperationResult CreateInventory(CreateInventory command);
        OperationResult EditInventory(EditInventory command);
        EditInventory GetDetails(int id);
        List<InventoryViewModel> Search(InventorySearchModel model);
        OperationResult Increase(IncreaseInventory command);
        OperationResult Decrease(List<DecreaseInventory> command);
        OperationResult Decrease(DecreaseInventory command);
        List<InventoryOperationViewModel> GetOperationLogBy(int inventoryId);
    }
}
