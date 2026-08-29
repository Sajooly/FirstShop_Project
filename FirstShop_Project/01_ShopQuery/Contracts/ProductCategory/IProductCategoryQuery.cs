using _01_ShopQuery.Contracts.Product;
using System.Collections.Generic;

namespace _01_ShopQuery.Contracts.ProductCategory
{
    public interface IProductCategoryQuery
    {
        List<ProductCategoryQueryModel> GetProductCategories();
        List<ProductCategoryQueryModel> GetProductCategoriesWithProduct();
        ProductCategoryQueryModel GetProductCategoryWithProductBy(string slug);
    }
}
