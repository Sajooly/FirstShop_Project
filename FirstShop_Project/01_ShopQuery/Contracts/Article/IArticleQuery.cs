using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_ShopQuery.Contracts.Article
{
    public interface IArticleQuery
    {
        List<ArticleQueryModel> GetLatestArticles();
        ArticleQueryModel GetArticleDetailsBy(string slug);
    }
}
