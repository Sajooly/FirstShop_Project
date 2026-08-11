using _0_Framework.Application;
using System.Collections.Generic;

namespace ShopManagement.Application.Contracts.Slide
{
    public interface ISlideApplication
    {
        OperationResult Create(CreateSlide command);
        OperationResult Edit(EditSlide command);
        OperationResult RemoveSlide(int id);
        OperationResult RestoreSlide(int id);
        EditSlide GetDetails(int id);
        List<SlideViewModel> GetList();
    }

}
