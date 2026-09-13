using _0_Framework.Infrastructure;
using System.Collections.Generic;

namespace AccountManagement.Application.Contract.Role
{
    public class EditRole : CreateRole
    {
        public int Id { get; set; }
        public List<PermissionDto> MappedPermissions { get; set; }

    }
}
