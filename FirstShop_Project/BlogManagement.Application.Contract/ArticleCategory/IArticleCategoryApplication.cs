using _0_Framework.Application;
using System.Collections.Generic;

namespace BlogManagement.Application.Contract.ArticleCategory
{
    public interface IArticleCategoryApplication
    {
        OperationResult Create(CreateArticleCategory command);
        OperationResult Edit(EditArticleCategory command);
        List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel searchModel);
        EditArticleCategory GetDetails(int id);
        List<ArticleCategoryViewModel> GetAllCategoryNames();

    }
}
