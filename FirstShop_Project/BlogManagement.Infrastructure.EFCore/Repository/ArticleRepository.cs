using _0_Framework.Application;
using _0_Framework.Infrastructure;
using BlogManagement.Application.Contract.Article;
using BlogManagement.Domain.ArticleAgg;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogManagement.Infrastructure.EFCore.Repository
{
    public class ArticleRepository : RepositoryBase<int, Article>, IArticleRepository
    {
        private readonly BlogContext _context;

        public ArticleRepository(BlogContext context) : base(context)
        {
            _context = context;
        }

        public EditArticle GetDetails(int id)
        {
            return _context.Articles.Select(a => new EditArticle
            {
                Id = a.Id,
                Title = a.Title,
                ShortDescription = a.ShortDescription,
                Description = a.Description,
                Slug = a.Slug,
                Keyword = a.Keyword,
                MetaDescription = a.MetaDescription,
                CanonicalAddress = a.CanonicalAddress,
                PublishDate = a.PublishDate.ToFarsi(),
                PictureAlt = a.PictureAlt,
                PictureTitle = a.PictureTitle,
                CategoryId = a.CategoryId

            }).FirstOrDefault(a => a.Id == id);
        }

        public Article GetWithCategory(int id)
        {
            return _context.Articles
                .Include(a => a.Category)
                .FirstOrDefault(a => a.Id == id);
        }

        public List<ArticleViewModel> Search(ArticleSearchModel searchModel)
        {
            var query = _context.Articles
                .Include(ac => ac.Category)
                .Select(a => new ArticleViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    ShortDescription = a.ShortDescription.Substring(0, Math.Min(a.ShortDescription.Length, 50)) + "...",
                    PublishDate = a.PublishDate.ToFarsi(),
                    Picture = a.Picture,
                    CategoryName = a.Category.Name
                });


            if (!string.IsNullOrWhiteSpace(searchModel.Title))
                query = query.Where(a => a.Title.Contains(searchModel.Title));


            if (searchModel.CategoryId > 0)
                query = query.Where(a => a.CategoryId == searchModel.CategoryId);

            return query.OrderByDescending(a => a.Id).ToList();
        }
    }
}
