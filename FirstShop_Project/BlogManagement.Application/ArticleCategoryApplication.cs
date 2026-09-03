using BlogManagement.Application.Contract.ArticleCategory;
using System.Collections.Generic;
using _0_Framework.Application;
using BlogManagement.Domain.ArticleCategoryAgg;
using System.IO;

namespace BlogManagement.Application
{
    public class ArticleCategoryApplication : IArticleCategoryApplication
    {
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly IFileUploader _fileUploader;

        public ArticleCategoryApplication(IArticleCategoryRepository articleCategoryRepository, IFileUploader fileUploader)
        {
            _articleCategoryRepository = articleCategoryRepository;
            _fileUploader = fileUploader;
        }

        public OperationResult Create(CreateArticleCategory command)
        {
            var operation = new OperationResult();

            if (_articleCategoryRepository.Exists(x => x.Name == command.Name))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var slug = command.Slug.Slugify();
            var fileName = _fileUploader.Upload(command.Picture, $"Blog/{slug}");

            _articleCategoryRepository.Create(new ArticleCategory(command.Name, fileName, command.PictureAlt, command.PictureTitle, command.Description,
                command.ShowOrder, slug, command.Keyword, command.MetaDescription, command.CanonicalAddress));

            _articleCategoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Edit(EditArticleCategory command)
        {
            var operation = new OperationResult();

            var entity = _articleCategoryRepository.GetBy(command.Id);

            if (entity == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            if (_articleCategoryRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);


            var slug = command.Slug.Slugify();
            var fileName = _fileUploader.Upload(command.Picture, $"Blog/{slug}");

            entity.Edit(command.Name, fileName, command.PictureAlt, command.PictureTitle, command.Description,
                command.ShowOrder, command.Slug, command.Keyword, command.MetaDescription, command.CanonicalAddress);
            _articleCategoryRepository.SaveChanges();
            return operation.Succedded();
        }

        public EditArticleCategory GetDetails(int id)
        {
            return _articleCategoryRepository.GetDetails(id);
        }

        public List<ArticleCategoryViewModel> Search(ArticleCategorySearchModel searchModel)
        {
            return _articleCategoryRepository.Search(searchModel);
        }
    }
}
