using _01_ShopQuery;
using _01_ShopQuery.Contracts.ArticleCategory;
using _01_ShopQuery.Contracts.ProductCategory;
using Microsoft.AspNetCore.Mvc;
using ShopManagement.Domain.ProductCategoryAgg;
using System.Collections.Generic;

namespace ServiceHost.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IProductCategoryQuery _productCategoryQuery;
        private readonly IArticleCategoryQuery _articleCategoryQuery;

        public MenuModel menuModel { get; set; } = new MenuModel();

        public MenuViewComponent(IProductCategoryQuery productCategoryQuery, IArticleCategoryQuery articleCategoryQuery)
        {
            _productCategoryQuery = productCategoryQuery;
            _articleCategoryQuery = articleCategoryQuery;
        }

        public IViewComponentResult Invoke()
        {
            #region کد قبلی این متد
            //var result = new MenuModel
            //{
            //    ProductCategories = _productCategoryQuery.GetProductCategories(),
            //    ArticleCategories = _articleCategoryQuery.GetArticleCategories()
            //};
            //return View(result);
            #endregion

            menuModel.ArticleCategories = _articleCategoryQuery.GetArticleCategories();
            menuModel.ProductCategories = _productCategoryQuery.GetProductCategories();

            return View(menuModel);
        }
    }
}
