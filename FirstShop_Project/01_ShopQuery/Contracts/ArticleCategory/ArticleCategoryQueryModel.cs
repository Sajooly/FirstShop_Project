using _01_ShopQuery.Contracts.Article;
using System.Collections.Generic;

namespace _01_ShopQuery.Contracts.ArticleCategory
{
    public class ArticleCategoryQueryModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ShowOrder { get; set; }

        //Images
        public string Picture { get; set; }
        public string PictureAlt { get; set; }
        public string PictureTitle { get; set; }



        //For SEO 
        public string Slug { get; set; }
        public string Keyword { get; set; }
        public string MetaDescription { get; set; }
        public string CanonicalAddress { get; set; }

        public int ArticlesCount { get; set; }

        public List<ArticleQueryModel> Articles { get; set; }

        public List<string> KeywordList { get; set; }

    }
}
