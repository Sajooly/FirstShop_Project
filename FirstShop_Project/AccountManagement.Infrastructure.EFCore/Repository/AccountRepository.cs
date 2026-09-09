using _0_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Domain.AccountAgg;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountManagement.Infrastructure.EFCore.Repository
{
    public class AccountRepository : RepositoryBase<int, Account>, IAccountRepository
    {
        private readonly AccountContext _context;

        public AccountRepository(AccountContext context) : base(context)
        {
            _context = context;
        }

        public EditAccount GetDetailsBy(int id)
        {
            return _context.Accounts.Select(a => new EditAccount
            {
                Id = a.Id,
                FullName = a.FullName,
                UserName = a.UserName,
                Mobile = a.Mobile,
                RoleId = a.RoleId,

            }).FirstOrDefault(a => a.Id == id);
        }

        public List<AccountViewModel> Search(AccountSearchModel searchModel)
        {
            var query = _context.Accounts.Select(a => new AccountViewModel
            {
                Id = a.Id,
                FullName = a.FullName,
                UserName = a.UserName,
                Mobile = a.Mobile,
                ProfilePhoto = a.ProfilePhoto,
                Role = "مدیر سیستم",
                RoleId = 2
            });

            if (searchModel.RoleId > 0)
                query = query.Where(x => x.RoleId == searchModel.RoleId);

            if (!string.IsNullOrWhiteSpace(searchModel.FullName))
                query = query.Where(a => a.FullName.Contains(searchModel.FullName));

            if (!string.IsNullOrWhiteSpace(searchModel.UserName))
                query = query.Where(a => a.UserName.Contains(searchModel.UserName));

            if (!string.IsNullOrWhiteSpace(searchModel.Mobile))
                query = query.Where(a => a.Mobile.Contains(searchModel.Mobile));


            return query.OrderByDescending(a => a.Id).ToList();
        }
    }
}
