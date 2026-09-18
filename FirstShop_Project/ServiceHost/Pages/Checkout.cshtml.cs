using _0_Framework.Application;
using _0_Framework.Application.ZarinPal;
using _01_ShopQuery.Contracts;
using _01_ShopQuery.Contracts.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShopManagement.Application.Contracts.Order;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceHost.Pages
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        private readonly ICartCalculatorService _cartCalculatorService;
        public const string CookieName = "cart-items";
        public Cart Cart;

        private readonly ICartService _cartService;
        private readonly IProductQuery _productQuery;
        private readonly IOrderApplication _orderApplication;

        private readonly IZarinPalFactory _zarinPalFactory;
        private readonly IAuthHelper _authHelper;

        public bool PayStatus;
        public long PayRefId;
        public int OrderId;


        public CheckoutModel(ICartCalculatorService cartCalculatorService, ICartService cartService, IProductQuery productQuery, IOrderApplication orderApplication, IZarinPalFactory zarinPalFactory, IAuthHelper authHelper)
        {
            _cartCalculatorService = cartCalculatorService;
            _cartService = cartService;
            _productQuery = productQuery;
            _orderApplication = orderApplication;
            _zarinPalFactory = zarinPalFactory;
            _authHelper = authHelper;
            Cart = new Cart();
        }

        public void OnGet()
        {
            var cookieValue = Request.Cookies[CookieName];

            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cookieValue);
            foreach (var item in cartItems)
                item.CalculateTotalItemPrice();

            Cart = _cartCalculatorService.ComputeCart(cartItems);
            _cartService.Set(Cart);
        }

        public IActionResult OnGetPay()
        {
            var cart = _cartService.Get();
            var result = _productQuery.ChechInventoryStatus(cart.Items);

            if (result.Any(x => !x.IsInStock))
                return RedirectToPage("/Cart");

            OrderId = _orderApplication.PlaceOrder(cart);




            //var paymentResponce = _zarinPalFactory.CreatePaymentRequest(cart.PayAmount.ToString(), "", "", "خرید از فروشگاه سجولی", orderId);



            //Create Custom Authority And Custume PayStatus 
            var OneOrTwo = new Random().Next(1, 4);
            PayStatus = OneOrTwo == 1 ? true : false;

            var random = new Random();
            long id;
            do
            {
                id = random.Next(1_000_000_000, 2_147_483_647) * 1_000_000L
                         + random.Next(0, 1_000_000);

            } while (_orderApplication.CheckOrderIdAuthority(id));
            PayRefId = id;
            //End



            //return Redirect($"https://{_zarinPalFactory.Prefix}.zarinpal.com/pg/StartPay/{paymentResponce.Authority}");

            TempData["PayStatus"] = PayStatus;
            TempData["PayRefId"] = PayRefId.ToString();
            TempData["OrderId"] = OrderId;
            var payStatus = TempData.Peek("PayStatus");
            var payRefId = TempData.Peek("PayRefId");
            var orderId = TempData.Peek("OrderId");
            return RedirectToPage("/Checkout", "CallBack");
        }


        //public IActionResult OnGetCallBack([FromQuery] string authority, [FromQuery] string status, [FromQuery] int oId)  // For Real Payment 
        public IActionResult OnGetCallBack()
        {
            var payStatus = (bool)TempData["PayStatus"];
            var payRefId = long.Parse(TempData["PayRefId"].ToString());
            var orderId = (int)TempData["OrderId"];

            var orderAmount = _orderApplication.GetAmountBy(orderId);

            var result = new PaymentResult();
            if (payStatus)
            {
                var trackNo = _orderApplication.PaymentSucceded(orderId, payRefId);
                Response.Cookies.Delete(CookieName);
                result = result.Succeeded("پرداخت با موفقیت انجام شد", trackNo);
                return RedirectToPage("/PaymentResult", result);
            }
            else
            {
                result = result.Failed("پرداخت با موفقیت انجام نشد.در صورت کسر وجه از حساب شما تا 24 ساعت دیگر به حساب شما باز میگردد.");
                return RedirectToPage("/PaymentResult", result);
            }

        }

    }
}
