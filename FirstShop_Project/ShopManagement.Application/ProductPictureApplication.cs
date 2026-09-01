using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductPictureAgg;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ShopManagement.Application
{
    public class ProductPictureApplication : IProductPictureApplication
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductPictureRepository _productPictureRepository;
        private readonly IFileUploader _fileUploader;

        public ProductPictureApplication(IProductPictureRepository productPictureRepository, IProductRepository productRepository, IFileUploader fileUploader)
        {
            _productPictureRepository = productPictureRepository;
            _productRepository = productRepository;
            _fileUploader = fileUploader;
        }

        public OperationResult Create(CreateProductPicture command)
        {
            var operation = new OperationResult();
            //if (_productPictureRepository.Exists(x => x.Picture == command.Picture && x.ProductId == command.ProductId))
            //    return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var product = _productRepository.GetProductWithCategory(command.ProductId);

            var path = $"{product.Category.Slug}/{product.Slug}";
            var pictureName = _fileUploader.Upload(command.Picture, path);

            _productPictureRepository.Create(new ProductPicture(command.ProductId, pictureName, command.PictureAlt, command.PictureTitle));
            _productPictureRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult Edit(EditProductPicture command)
        {
            var operation = new OperationResult();
            var productPicture = _productPictureRepository.GetWithProductAndCategory(command.Id);

            if (productPicture == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);

            //if (_productPictureRepository.Exists(x => x.Picture == command.Picture && x.ProductId == command.ProductId && x.Id != command.Id))
            //    return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var path = $"{productPicture.Product.Category.Slug}/{productPicture.Product.Slug}";
            var pictureName = _fileUploader.Upload(command.Picture, path);

            productPicture.Edit(command.ProductId, pictureName, command.PictureAlt, command.PictureTitle);
            _productPictureRepository.SaveChanges();
            return operation.Succedded();
        }

        public EditProductPicture GetDetails(int id)
        {
            return _productPictureRepository.GetDetails(id);
        }

        public OperationResult RemovePicture(int id)
        {
            var operation = new OperationResult();
            var productPicture = _productPictureRepository.GetBy(id);

            if (productPicture == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);


            productPicture.Remove();
            _productPictureRepository.SaveChanges();
            return operation.Succedded();
        }

        public OperationResult RestorePicture(int id)
        {
            var operation = new OperationResult();
            var productPicture = _productPictureRepository.GetBy(id);

            if (productPicture == null)
                return operation.Failed(ApplicationMessages.RecordNotFound);


            productPicture.Restore();
            _productPictureRepository.SaveChanges();
            return operation.Succedded();
        }

        public List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel)
        {
            return _productPictureRepository.Search(searchModel);
        }
    }
}
