using _0_Framework.Domain;
using BlogManagement.Application.Contract.Article;
using System.Collections.Generic;

namespace BlogManagement.Domain.ArticleAgg
{
    public interface IArticleRepository : IRepository<int, Article>
    {
        List<ArticleViewModel> Search(ArticleSearchModel searchModel);
        EditArticle GetDetails(int id);
        Article GetWithCategory(int id);
    }
}
