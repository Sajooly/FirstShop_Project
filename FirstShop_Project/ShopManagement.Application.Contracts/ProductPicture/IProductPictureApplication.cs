using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;
using System.Collections.Generic;

namespace ShopManagement.Application.Contracts.ProductPicture
{
    public interface IProductPictureApplication
    {
        OperationResult Create(CreateProductPicture command);
        OperationResult RemovePicture(int id);
        List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel);
        OperationResult Edit(EditProductPicture command);
        OperationResult RestorePicture(int id);
        EditProductPicture GetDetails(int id);
    }
}
