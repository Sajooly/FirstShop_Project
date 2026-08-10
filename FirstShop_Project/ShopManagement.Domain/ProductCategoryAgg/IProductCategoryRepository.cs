using _0_Framework.Domain;
using ShopManagement.Application.Contracts.ProductCategory;
using System.Collections.Generic;

namespace ShopManagement.Domain.ProductCategoryAgg
{
    public interface IProductCategoryRepository : IRepository<int, ProductCategory>
    {
        #region Implement From Generic Repository 

        //void Create(ProductCategory entity);
        //ProductCategory GetBy(int id);
        //List<ProductCategory> GetAll();
        //bool Exists(Expression<Func<ProductCategory, bool>> expression);
        //void SaveChanges();

        #endregion

        EditProductCategory GetDetails(int id);
        List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel);
        List<ProductCategoryViewModel> GetProductCategoryName();

    }
}
