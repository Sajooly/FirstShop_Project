using _0_Framework.Domain;
using DiscountManagement.Application.Contract.CustomerDiscount;
using System.Collections.Generic;

namespace DiscountManagement.Domain.CustomerDiscountAgg
{
    public interface ICustomerDiscountRepository : IRepository<int, CustomerDiscount>
    {
        List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel);
        EditCustomerDiscount GetDetails(int id);
    }
}
