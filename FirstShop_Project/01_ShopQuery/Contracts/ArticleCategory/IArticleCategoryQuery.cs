using System.Collections.Generic;

namespace _01_ShopQuery.Contracts.ArticleCategory
{
    public interface IArticleCategoryQuery
    {
        List<ArticleCategoryQueryModel> GetArticleCategories();
        ArticleCategoryQueryModel GetArticleCategoryBy(string slug);
    }
}
