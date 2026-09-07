using _0_Framework.Application;
using _01_ShopQuery.Contracts.Article;
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

        public ArticleCategoryQueryModel GetArticleCategoryBy(string slug)
        {
            var articleCategory = _blogContext.ArticleCategories
                .Include(ac => ac.Articles)
                .Select(x => new ArticleCategoryQueryModel
                {
                    Name = x.Name,
                    MetaDescription = x.MetaDescription,
                    Description = x.Description,

                    Slug = x.Slug,
                    CanonicalAddress = x.CanonicalAddress,
                    Keyword = x.Keyword,

                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,

                    Articles = x.Articles.Select(a => new ArticleQueryModel
                    {
                        Id = a.Id,
                        Slug = a.Slug,
                        ShortDescription = a.ShortDescription,
                        Title = a.Title,
                        PublishDate = a.PublishDate.ToFarsi(),

                        Picture = a.Picture,
                        PictureAlt = a.PictureAlt,
                        PictureTitle = a.PictureTitle,

                    }).OrderByDescending(a => a.Id).ToList(),



                    ArticlesCount = x.Articles.Count,

                })
                .AsNoTracking()
                .FirstOrDefault(x => x.Slug == slug);

                articleCategory.KeywordList = articleCategory.Keyword.Split(',', ' ').ToList();

            return articleCategory;
        }
    }
}
