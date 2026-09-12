using _0_Framework.Domain;
using AccountManagement.Application.Contract.Account;
using System.Collections.Generic;

namespace AccountManagement.Domain.AccountAgg
{
    public interface IAccountRepository : IRepository<int, Account>
    {
        List<AccountViewModel> Search(AccountSearchModel searchModel);
        EditAccount GetDetailsBy(int id);
        Account GetBy(string name);
    }
}
