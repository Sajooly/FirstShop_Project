using _0_Framework.Application;
using _01_ShopQuery.Contracts.Article;
using BlogManagement.Domain.ArticleCategoryAgg;
using BlogManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _01_ShopQuery.Query
{
    public class ArticleQuery : IArticleQuery
    {
        private readonly BlogContext _blogContext;

        public ArticleQuery(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        public ArticleQueryModel GetArticleDetailsBy(string slug)
        {
            var article = _blogContext.Articles
                .Include(a => a.Category)
                .Select(x => new ArticleQueryModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ShortDescription = x.ShortDescription,
                    PublishDate = x.PublishDate.ToFarsi(),

                    Slug = x.Slug,
                    Keyword = x.Keyword,
                    CanonicalAddress = x.CanonicalAddress,
                    MetaDescription = x.MetaDescription,

                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,

                    //CategoryId = x.CategoryId,
                    CategoryName = x.Category.Name,
                    CategorySlug = x.Category.Slug,

                })
                .AsNoTracking()
                .FirstOrDefault(x => x.Slug == slug);

                article.KeywordList = article.Keyword.Split(' ', ',').ToList();

            return article;
        }

        public List<ArticleQueryModel> GetLatestArticles()
        {
            return _blogContext.Articles
                .Include(a => a.Category)
                .Where(a => a.PublishDate <= DateTime.Now)
                .Select(x => new ArticleQueryModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    PublishDate = x.PublishDate.ToFarsi(),
                    //Description = x.Description,
                    ShortDescription = x.ShortDescription,

                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,

                    Slug = x.Slug,
                    //Keyword = x.Keyword,
                    //MetaDescription = x.MetaDescription,
                    //CanonicalAddress = x.CanonicalAddress,

                    //CategoryId = x.CategoryId,
                    //CategoryName = x.Category.Name,
                    //CategorySlug = x.Category.Slug,
                })
                .OrderByDescending(x => x.Id)
                .Take(5)
                .AsNoTracking()
                .ToList();
        }
    }
}
