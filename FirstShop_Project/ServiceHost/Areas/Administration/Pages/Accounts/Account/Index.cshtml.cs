using AccountManagement.Application.Contract.Account;
using AccountManagement.Application.Contract.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Account
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }

        public AccountSearchModel SearchModel;
        public List<AccountViewModel> Accounts;
        public SelectList Roles;

        private readonly IAccountApplication _accountApplication;
        private readonly IRoleApplication _roleApplication;

        public IndexModel(IAccountApplication accountApplication, IRoleApplication roleApplication)
        {
            _accountApplication = accountApplication;
            _roleApplication = roleApplication;
        }

        public void OnGet(AccountSearchModel searchModel)
        {
            Roles = new SelectList(_roleApplication.Get(), "Id", "Name");
            Accounts = _accountApplication.Search(searchModel);
        }
        public IActionResult OnGetCreate()
        {
            var account = new RegisterAccount()
            {
                Roles = _roleApplication.Get()
            };
            return Partial("Create", account);
        }
        public JsonResult OnPostCreate(RegisterAccount command)
        {
            var result = _accountApplication.RegisterAccount(command);
            return new JsonResult(result);
        }
        public IActionResult OnGetEdit(int id)
        {
            var account = _accountApplication.GetDetailsBy(id);
            account.Roles = _roleApplication.Get();
            return Partial("Edit", account);
        }
        public JsonResult OnPostEdit(EditAccount command)
        {
            var result = _accountApplication.EditAccount(command);
            return new JsonResult(result);
        }
        public IActionResult OnGetChangePassword(int id)
        {
            return Partial("ChangePassword", new ChangePassword { Id = id });
        }
        public JsonResult OnPostChangePassword(ChangePassword command)
        {
            var result = _accountApplication.ChangePassword(command);
            return new JsonResult(result);
        }
    }
}
