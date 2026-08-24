using _0_Framework.Application;
using System.Collections.Generic;

namespace ShopManagement.Application.Contracts.Product
{
    public interface IProductApplication
    {
        OperationResult Create(CreateProduct command);
        OperationResult Edit(EditProduct command);
        EditProduct GetDetails(int id);
        List<ProductViewModel> Search(ProductSearchModel searchModel);
        //OperationResult AddInStore(int id); //This Property Added To Inventory Model
        //OperationResult RemoveInStore(int id); //This Property Added To Inventory Model
        List<ProductViewModel> GetProductsName();
    }
}
