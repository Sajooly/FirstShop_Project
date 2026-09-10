using AccountManagement.Application.Contract.Account;
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

        public IndexModel(IAccountApplication accountApplication)
        {
            _accountApplication = accountApplication;
        }

        public void OnGet(AccountSearchModel searchModel)
        {
            Accounts = _accountApplication.Search(searchModel);
        }
        public IActionResult OnGetCreate()
        {
            return Partial("Create");
        }
        public JsonResult OnPostCreate(CreateAccount command)
        {
            var result = _accountApplication.CreateAccount(command);
            return new JsonResult(result);
        }
        public IActionResult OnGetEdit(int id)
        {
            var account = _accountApplication.GetDetailsBy(id);
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
