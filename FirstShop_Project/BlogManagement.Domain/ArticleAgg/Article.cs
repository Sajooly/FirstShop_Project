using _0_Framework.Domain;
using BlogManagement.Domain.ArticleCategoryAgg;
using System;

namespace BlogManagement.Domain.ArticleAgg
{
    public class Article : EntityBase
    {
        public string Title { get; private set; }
        public string ShortDescription { get; private set; }
        public string Description { get; private set; }

        public DateTime PublishDate { get; private set; }

        public string Picture { get; private set; }
        public string PictureAlt { get; private set; }
        public string PictureTitle { get; private set; }

        //For Seo
        public string Slug { get; private set; }
        public string Keyword { get; private set; }
        public string MetaDescription { get; private set; }
        public string CanonicalAddress { get; private set; }

        //Navigation Property
        public int CategoryId { get; private set; }
        public ArticleCategory Category { get; private set; }

        public Article(string title, string shortDescription, string description, DateTime publishDate, string picture, string pictureAlt,
            string pictureTitle, string slug, string keyword, string metaDescription, string canonicalAddress, int categoryId)
        {
            Title = title;
            ShortDescription = shortDescription;
            Description = description;
            PublishDate = publishDate;
            Picture = picture;
            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
            Slug = slug;
            Keyword = keyword;
            MetaDescription = metaDescription;
            CanonicalAddress = canonicalAddress;
            CategoryId = categoryId;
        }
        public void Edit(string title, string shortDescription, string description, DateTime publishDate, string picture, string pictureAlt,
           string pictureTitle, string slug, string keyword, string metaDescription, string canonicalAddress, int categoryId)
        {
            Title = title;
            ShortDescription = shortDescription;
            Description = description;
            PublishDate = publishDate;

            if (!string.IsNullOrWhiteSpace(picture))
                Picture = picture;

            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
            Slug = slug;
            Keyword = keyword;
            MetaDescription = metaDescription;
            CanonicalAddress = canonicalAddress;
            CategoryId = categoryId;
        }
    }
}
