using _0_Framework.Infrastructure;
using System.Collections.Generic;

namespace InventoryManagement.Configuration.Permissions
{
    public class InventoryPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new Dictionary<string, List<PermissionDto>>
            {
                {
                    "Inventory",new List<PermissionDto>
                    {
                        new PermissionDto(InventoryPermissions.ListInventory,"ListInventory"),
                        new PermissionDto(InventoryPermissions.SearchInventory,"SearchInventory"),
                        new PermissionDto(InventoryPermissions.CreateInventory,"CreateInventory"),
                        new PermissionDto(InventoryPermissions.EditInventory,"EditInventory"),
                        new PermissionDto(InventoryPermissions.Increase,"Increase"),
                        new PermissionDto(InventoryPermissions.Decrease,"Decrease"),
                        new PermissionDto(InventoryPermissions.OperationLog,"OperationLog")
                    }
                }
            };
        }
    }
}
