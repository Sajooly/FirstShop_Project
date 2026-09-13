using _0_Framework.Application;
using _0_Framework.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Application.Contract.Role
{
    public class CreateRole
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Name { get; set; }

        public List<int> Permissions { get; set; }
    }
}
