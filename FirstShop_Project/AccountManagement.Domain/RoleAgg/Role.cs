using _0_Framework.Domain;
using AccountManagement.Domain.AccountAgg;
using System.Collections.Generic;

namespace AccountManagement.Domain.RoleAgg
{
    public class Role : EntityBase
    {
        public string Name { get; private set; }


        //Navigation Property For Relationship
        public List<Account> Accounts { get; private set; }

        protected Role()
        {
            Accounts = new List<Account>();
        }

        public Role(string name)
        {
            Name = name;
        }
        public void Edit(string name)
        {
            Name = name;
        }
    }
}
