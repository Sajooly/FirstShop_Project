using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopManagement.Application.Contracts.Order;
using System.Collections.Generic;

namespace ServiceHost.Areas.Administration.Pages.Shop.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public OrderSearchModel SearchModel;
        public List<OrderViewModel> Orders;
        public SelectList Accounts;

        private readonly IOrderApplication _orderApplication;
        private readonly IAccountApplication _accountApplication;

        public IndexModel(IOrderApplication orderApplication, IAccountApplication accountApplication)
        {
            _orderApplication = orderApplication;
            _accountApplication = accountApplication;
        }

        public void OnGet(OrderSearchModel searchModel)
        {
            Accounts = new SelectList(_accountApplication.GetAccounts(), "Id", "FullName");
            Orders = _orderApplication.Search(searchModel);
        }

        public IActionResult OnGetConfirm(int orderId)
        {
            _orderApplication.PaymentSucceded(orderId, 0);
            return RedirectToPage("Index");
        }
        public IActionResult OnGetCancel(int orderId)
        {
            _orderApplication.Cancel(orderId);
            return RedirectToPage("Index");
        }

        public IActionResult OnGetItems(int id)
        {
            var items=_orderApplication.GetOrderItemsBy(id);
            return Partial("Items", items);
        }

    }
}
