using _0_Framework.Application;
using System.Collections.Generic;

namespace AccountManagement.Application.Contract.Account
{
    public interface IAccountApplication
    {
        OperationResult RegisterAccount(RegisterAccount commnad);
        OperationResult EditAccount(EditAccount commnad);
        OperationResult ChangePassword(ChangePassword command);
        List<AccountViewModel> Search(AccountSearchModel searchModel);
        EditAccount GetDetailsBy(int id);
        OperationResult Login(LoginAccount command);
        void LogOut();
    }
}
