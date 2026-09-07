using _01_ShopQuery.Contracts.ArticleCategory;
using BlogManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace _01_ShopQuery.Query
{
    public class ArticleCategoryQuery : IArticleCategoryQuery
    {
        private readonly BlogContext _blogContext;

        public ArticleCategoryQuery(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public List<ArticleCategoryQueryModel> GetArticleCategories()
        {
            return _blogContext.ArticleCategories
                .Include(ac => ac.Articles)
                .Select(ac => new ArticleCategoryQueryModel
                {
                    Name = ac.Name,
                    //Description = ac.Description,
                    //ShowOrder = ac.ShowOrder,

                    Picture = ac.Picture,
                    PictureAlt = ac.PictureAlt,
                    PictureTitle = ac.PictureTitle,

                    Slug = ac.Slug,
                    //CanonicalAddress = ac.CanonicalAddress,
                    //MetaDescription = ac.MetaDescription,
                    //Keyword = ac.Keyword,

                    ArticlesCount = ac.Articles.Count

                })
                .AsNoTracking()
                .ToList();
        }
    }
}
