using ShopManagement.Application.Contracts.Order;
using System.Collections.Generic;

namespace _01_ShopQuery.Contracts
{
    public interface ICartCalculatorService
    {
        Cart ComputeCart(List<CartItem> cartItems);
    }
}
