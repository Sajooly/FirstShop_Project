using _0_Framework.Application;
using System.Collections.Generic;

namespace DiscountManagement.Application.Contract.CustomerDiscount
{
    public interface ICustomerDiscountApplication
    {
        OperationResult Define(DefineCustomerDiscount command);
        OperationResult Edit(EditCustomerDiscount command);
        List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel);
        EditCustomerDiscount GetDetails(int id);
    }
}
