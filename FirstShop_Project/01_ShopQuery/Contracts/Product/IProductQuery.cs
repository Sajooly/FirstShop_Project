using ShopManagement.Application.Contracts.Order;
using System.Collections.Generic;

namespace _01_ShopQuery.Contracts.Product
{
    public interface IProductQuery
    {
        List<ProductQueryModel> GetLatestProducts();
        List<ProductQueryModel> Search(string searchInProducts);
        ProductQueryModel GetProductDetailsBy(string slug);
        List<CartItem> ChechInventoryStatus(List<CartItem> items);
    }
}
