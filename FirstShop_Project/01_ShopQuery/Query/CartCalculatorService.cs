using _0_Framework.Application;
using _0_Framework.Infrastructure;
using _01_ShopQuery.Contracts;
using DiscountManagement.Infrastructure.EFCore;
using ShopManagement.Application.Contracts.Order;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _01_ShopQuery.Query
{
    public class CartCalculatorService : ICartCalculatorService
    {
        private readonly IAuthHelper _authHelper;
        private readonly DiscountContext _discountContext;

        public CartCalculatorService(IAuthHelper authHelper, DiscountContext discountContext)
        {
            _authHelper = authHelper;
            _discountContext = discountContext;
        }


        public Cart ComputeCart(List<CartItem> cartItems)
        {
            var cart = new Cart();
            var currentAccountRole = _authHelper.CurrentAccountRole();


            var colleagueDiscounts = _discountContext.ColleagueDiscounts
                .Where(x => !x.IsRemoved)
                .Select(x => new { x.ProductId, x.DiscountRate }).ToList();
            
            var customerDiscounts = _discountContext.CustomerDiscounts
                .Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now)
                .Select(x => new { x.ProductId, x.DiscountRate }).ToList();



            foreach (var item in cartItems)
            {
                if (currentAccountRole == Roles.ColleagueUser)
                {
                    item.DiscountRate = colleagueDiscounts.FirstOrDefault(x => x.ProductId == item.Id)?.DiscountRate ?? 0;
                }
                else
                {
                    item.DiscountRate = customerDiscounts.FirstOrDefault(x => x.ProductId == item.Id)?.DiscountRate ?? 0;

                }
                item.CalculateTotalItemPrice();
                item.DiscountAmount = ((item.DiscountRate * item.TotalItemPrice) / 100);
                item.ItemPayAmount = item.TotalItemPrice - item.DiscountAmount;

                cart.Add(item);
            }

            return cart;
        }
    }
}
