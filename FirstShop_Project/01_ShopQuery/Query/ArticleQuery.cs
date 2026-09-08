using _0_Framework.Application;
using _01_ShopQuery.Contracts.Article;
using _01_ShopQuery.Contracts.Comment;
using BlogManagement.Domain.ArticleCategoryAgg;
using BlogManagement.Infrastructure.EFCore;
using CommentManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _01_ShopQuery.Query
{
    public class ArticleQuery : IArticleQuery
    {
        private readonly BlogContext _blogContext;
        private readonly CommentContext _commentContext;

        public ArticleQuery(BlogContext blogContext, CommentContext commentContext)
        {
            _blogContext = blogContext;
            _commentContext = commentContext;
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



            var Comments = _commentContext.Comments
                 .Where(c => c.IsConfirmed && !c.IsCanceled)
                 .Where(c => c.Type == CommentType.Article && c.OwnerRecordId == article.Id)
                 .Select(x => new CommentQueryModel
                 {
                     Id = x.Id,
                     Message = x.Message,
                     Name = x.Name,
                     CreationDate = x.CreationDate.ToFarsi(),

                     ParentId = x.ParentId,

                 })
                 .OrderByDescending(x => x.Id)
                 .AsNoTracking()
                 .ToList();

            Comments.ForEach(x => x.ParentName = (Comments.FirstOrDefault(c => c.Id == x.ParentId)?.Name));




            article.Comments = Comments;

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
