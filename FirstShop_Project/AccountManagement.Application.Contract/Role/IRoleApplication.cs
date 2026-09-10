using _0_Framework.Application;
using System.Collections.Generic;

namespace AccountManagement.Application.Contract.Role
{
    public interface IRoleApplication
    {
        OperationResult Create(CreateRole command);
        OperationResult Edit(EditRole command);
        EditRole GetDetails(int id);
        List<RoleViewModel> Get();
    }
}
