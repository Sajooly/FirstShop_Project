using _0_Framework.Application;
using BlogManagement.Application.Contract.Article;
using BlogManagement.Domain.ArticleAgg;
using BlogManagement.Domain.ArticleCategoryAgg;
using System.Collections.Generic;

namespace BlogManagement.Application
{
    public class ArticleApplication : IArticleApplication
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IFileUploader _fileUploader;
        private readonly IArticleCategoryRepository _articleCategoryRepository;

        public ArticleApplication(IArticleRepository articleRepository, IFileUploader fileUploader, IArticleCategoryRepository articleCategoryRepository)
        {
            _articleRepository = articleRepository;
            _fileUploader = fileUploader;
            _articleCategoryRepository = articleCategoryRepository;
        }

        public OperationResult Create(CreateArticle command)
        {
            var operation = new OperationResult();

            if (_articleRepository.Exists(a => a.Title == command.Title))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);


            var slug = command.Slug.Slugify();
            var categorySlug = _articleCategoryRepository.GetSlugBy(command.CategoryId);

            var fileName = _fileUploader.Upload(command.Picture, $"Blog/{categorySlug}/{slug}");

            _articleRepository.Create(new Article(command.Title, command.ShortDescription, command.Description, command.PublishDate.ToGeorgianDateTime(), fileName,
                command.PictureAlt, command.PictureTitle, slug, command.Keyword, command.MetaDescription, command.CanonicalAddress, command.CategoryId));

            _articleRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Edit(EditArticle command)
        {
            var operation = new OperationResult();

            var article = _articleRepository.GetWithCategory(command.Id);

            if (article == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_articleRepository.Exists(a => a.Title == command.Title && a.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);


            var slug = command.Slug.Slugify();

            var fileName = _fileUploader.Upload(command.Picture, $"Blog/{article.Category.Slug}/{slug}");


            article.Edit(command.Title, command.ShortDescription, command.Description, command.PublishDate.ToGeorgianDateTime(), fileName,
                command.PictureAlt, command.PictureTitle, slug, command.Keyword, command.MetaDescription, command.CanonicalAddress, command.CategoryId);


            _articleRepository.SaveChanges();
            return operation.Succedded();
        }

        public EditArticle GetDetails(int id)
        {
            return _articleRepository.GetDetails(id);
        }

        public List<ArticleViewModel> Search(ArticleSearchModel searchModel)
        {
            return _articleRepository.Search(searchModel);
        }
    }
}
