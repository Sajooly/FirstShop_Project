using _01_ShopQuery.Contracts.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShopManagement.Application.Contracts.Order;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceHost.Pages
{
    public class CartModel : PageModel
    {
        private readonly IProductQuery _productQuery;
        public List<CartItem> CartItems;
        public const string CookieName = "cart-items";

        public CartModel(IProductQuery productQuery)
        {
            CartItems = new List<CartItem>();
            this._productQuery = productQuery;
        }

        public void OnGet()
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];



            var items = serializer.Deserialize<List<CartItem>>(value);

            CartItems = _productQuery.ChechInventoryStatus(items);
            foreach (var item in CartItems)
            {
                //This Write Befor We Create CalculateTotalItemPrice Method
                //var price = double.Parse(item.UnitPrice.Replace("٬", ""));
                //item.TotalItemPrice = price * item.Count;
                //End

                item.CalculateTotalItemPrice();
            }
        }

        public IActionResult OnGetRemoveFromCart(int id)
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            Response.Cookies.Delete(CookieName);


            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            var itemToRemove = cartItems.FirstOrDefault(x => x.Id == id);
            cartItems.Remove(itemToRemove);

            //JsonConvert.SerializeObject(cartItems);
            //var cookieOptions = new CookieOptions { Expires = DateTime.Now.AddDays(2) };
            Response.Cookies.Append(CookieName, serializer.Serialize(cartItems), new CookieOptions { Expires = DateTime.Now.AddDays(2), IsEssential = true });
            return RedirectToPage("/Cart");
        }

        public IActionResult OnGetGoToCheckout()
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];



            var items = serializer.Deserialize<List<CartItem>>(value);

            CartItems = _productQuery.ChechInventoryStatus(items);
            foreach (var item in CartItems)
            {
                var price = double.Parse(item.UnitPrice.Replace("٬", ""));
                item.TotalItemPrice = price * item.Count;
            }


            return RedirectToPage(CartItems.All(x => x.IsInStock) ? "/Checkout" : "/Cart");
        }
    }
}
