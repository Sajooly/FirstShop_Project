using _0_Framework.Domain;
using AccountManagement.Application.Contract.Role;
using System.Collections.Generic;

namespace AccountManagement.Domain.RoleAgg
{
    public interface IRoleRepository : IRepository<int, Role>
    {
        EditRole GetDetails(int id);
        List<RoleViewModel> GetRoles();
    }
}
