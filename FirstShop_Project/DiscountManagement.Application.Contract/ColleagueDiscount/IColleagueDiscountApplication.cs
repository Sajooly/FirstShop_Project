using _0_Framework.Application;
using System.Collections.Generic;

namespace DiscountManagement.Application.Contract.ColleagueDiscount
{
    public interface IColleagueDiscountApplication
    {
        OperationResult Define(DefineColleagueDiscount command);
        OperationResult Edit(EditColleagueDiscount command);
        EditColleagueDiscount GetDetails(int id);
        List<ColleagueDiscountViewModel> Search(ColleagueDiscountSearchModel searchModel);
        OperationResult Remove(int id) ;
        OperationResult Restore(int id);
    }
}
