using BlogManagement.Application.Contract.ArticleCategory;
using System.Collections.Generic;
using System.ComponentModel;

namespace BlogManagement.Application.Contract.Article
{
    public class ArticleSearchModel
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }

        [Description("دلیل استفاده این رو موقعی که خواستی استفاده بکنی برگرد و نگاه کن")]
        public List<ArticleCategoryViewModel> Categories { get; set; }
    }
}
