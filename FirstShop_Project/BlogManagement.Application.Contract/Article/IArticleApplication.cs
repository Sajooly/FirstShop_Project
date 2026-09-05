using _0_Framework.Application;
using System.Collections.Generic;

namespace BlogManagement.Application.Contract.Article
{
    public interface IArticleApplication
    {
        OperationResult Create(CreateArticle command);
        OperationResult Edit(EditArticle command);
        List<ArticleViewModel> Search(ArticleSearchModel searchModel);
        EditArticle GetDetails(int id);
    }
}
