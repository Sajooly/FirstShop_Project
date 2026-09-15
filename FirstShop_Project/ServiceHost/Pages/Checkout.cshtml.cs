using _01_ShopQuery.Contracts;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShopManagement.Application.Contracts.Order;
using System.Collections.Generic;
using System.Linq;

namespace ServiceHost.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly ICartCalculatorService _cartCalculatorService;
        public const string CookieName = "cart-items";
        public Cart Cart;

        public CheckoutModel(ICartCalculatorService cartCalculatorService)
        {
            _cartCalculatorService = cartCalculatorService;
        }

        public void OnGet()
        {
            var cookieValue = Request.Cookies[CookieName];

            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cookieValue);
            foreach (var item in cartItems)
                item.CalculateTotalItemPrice();

            Cart = _cartCalculatorService.ComputeCart(cartItems);

        }
    }
}
