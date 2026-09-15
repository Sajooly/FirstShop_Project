using _0_Framework.Application;
using _01_ShopQuery.Contracts.Comment;
using _01_ShopQuery.Contracts.Product;
using CommentManagement.Infrastructure.EFCore;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _01_ShopQuery.Query
{
    public class ProductQuery : IProductQuery
    {
        private readonly ShopContext _shopContext;
        private readonly InventoryContext _inventoryContext;
        private readonly DiscountContext _discountContext;
        private readonly CommentContext _commentContext;

        public ProductQuery(ShopContext shopContext, InventoryContext inventoryContext, DiscountContext discountContext, CommentContext commentContext)
        {
            _shopContext = shopContext;
            _inventoryContext = inventoryContext;
            _discountContext = discountContext;
            _commentContext = commentContext;
        }

        public ProductQueryModel GetProductDetailsBy(string slug)
        {
            var inventory = _inventoryContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice, x.InStock }).ToList();

            var discounts = _discountContext.CustomerDiscounts.Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).Select(x => new { x.ProductId, x.DiscountRate, x.EndDate }).ToList();


            var product = _shopContext.Products
              .Include(x => x.Category)
              .Include(x => x.ProductPictures)
              .Select(x => new ProductQueryModel
              {
                  Id = x.Id,
                  Name = x.Name,
                  Picture = x.Picture,
                  PictureAlt = x.PictureAlt,
                  PictureTitle = x.PictureTitle,
                  Category = x.Category.Name,
                  Slug = x.Slug,
                  CategorySlug = x.Category.Slug,
                  ShortDiscription = x.ShortDescription,
                  Code = x.Code,
                  Description = x.Description,
                  Keyword = x.Keywords,
                  MetaDescription = x.MetaDescription,


                  ProductPictures = x.ProductPictures.Select(pp => new ProductPictureQueryModel
                  {
                      Picture = pp.Picture,
                      PictureAlt = pp.PictureAlt,
                      PictureTitle = pp.PictureTitle,
                      IsRemoved = pp.IsRemoved,
                      ProductId = pp.ProductId

                  }).Where(x => !x.IsRemoved).ToList(),




              })
              .AsNoTracking()
              .FirstOrDefault(x => x.Slug == slug);




            var comments = _commentContext.Comments
                .Where(x => x.Type == CommentType.Product && x.OwnerRecordId == product.Id)
                .Where(x => x.IsConfirmed && !x.IsCanceled)
                .Select(x => new CommentQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Message = x.Message,
                })
                .OrderByDescending(c => c.Id)
                .AsNoTracking()
                .ToList();

            if (comments != null)
                product.Comments = comments;




            if (product == null)
                return new ProductQueryModel();

            var price = inventory.FirstOrDefault(x => x.ProductId == product.Id)?.UnitPrice ?? 0;
            product.Price = price.ToMoney();

            product.DiscountRate = discounts.FirstOrDefault(x => x.ProductId == product.Id)?.DiscountRate ?? 0;
            product.HasDiscount = product.DiscountRate > 0;

            var discountAmount = Math.Round(price * product.DiscountRate) / 100;
            var finalPrice = price - discountAmount;
            product.PriceWithDiscount = finalPrice.ToMoney();


            product.DiscountExpireDate = discounts.FirstOrDefault(x => x.ProductId == product.Id)?.EndDate.ToDiscountFormat();

            product.IsInStock = inventory.FirstOrDefault(x => x.ProductId == product.Id)?.InStock ?? false;







            return product;
        }

        public List<ProductQueryModel> GetLatestProducts()
        {
            var inventory = _inventoryContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice }).ToList();

            var discounts = _discountContext.CustomerDiscounts.Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).Select(x => new { x.ProductId, x.DiscountRate }).ToList();

            var products = _shopContext.Products
                .Include(x => x.Category)
                .Select(x => new ProductQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Category = x.Category.Name,
                    Slug = x.Slug,
                })
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Take(6)
                .ToList();


            foreach (var product in products)
            {
                var price = inventory.FirstOrDefault(x => x.ProductId == product.Id)?.UnitPrice ?? 0;
                product.Price = price.ToMoney();

                product.DiscountRate = discounts.FirstOrDefault(x => x.ProductId == product.Id)?.DiscountRate ?? 0;
                product.HasDiscount = product.DiscountRate > 0;

                var discountAmount = Math.Round((Convert.ToDouble(price) * product.DiscountRate) / 100);
                var finalPrice = Convert.ToDouble(price) - discountAmount;
                product.PriceWithDiscount = finalPrice.ToMoney();
            }


            return products;
        }

        public List<ProductQueryModel> Search(string searchInProducts)
        {
            var inventory = _inventoryContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice }).ToList();

            var discounts = _discountContext.CustomerDiscounts.Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now)
                .Select(x => new { x.DiscountRate, x.ProductId, x.EndDate }).ToList();

            var query = _shopContext.Products
                .Include(x => x.Category)
                .Select(x => new ProductQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureTitle = x.PictureTitle,
                    PictureAlt = x.PictureAlt,
                    Slug = x.Slug,
                    Category = x.Category.Name,
                    ShortDiscription = x.ShortDescription,
                    CategorySlug = x.Category.Slug
                }).AsNoTracking();

            if (!string.IsNullOrEmpty(searchInProducts))
                query = query.Where(x => x.Name.Contains(searchInProducts) || x.ShortDiscription.Contains(searchInProducts));

            var products = query.OrderByDescending(x => x.Id).ToList();


            products.ForEach(product =>
            {
                var price = inventory.FirstOrDefault(x => x.ProductId == product.Id)?.UnitPrice ?? 0;
                product.Price = price.ToMoney();

                var discountRate = discounts.FirstOrDefault(x => x.ProductId == product.Id)?.DiscountRate ?? 0;
                product.DiscountRate = discountRate;
                product.HasDiscount = product.DiscountRate > 0;

                var discountAmount = Math.Round((price * product.DiscountRate) / 100);
                var finalPrice = price - discountAmount;
                product.PriceWithDiscount = finalPrice.ToMoney();


                product.DiscountExpireDate = discounts.FirstOrDefault(x => x.ProductId == product.Id)?.EndDate.ToDiscountFormat();
            });

            return products;
        }

        public List<CartItem> ChechInventoryStatus(List<CartItem> items)
        {
            var inventory = _inventoryContext.Inventory.ToList();


            foreach (var cartitem in items)
            {
                var inventoryItem = inventory.FirstOrDefault(x => x.ProductId == cartitem.Id);
                cartitem.IsInStock = inventoryItem.CalculateInStock() >= cartitem.Count;
            }

            return items;
        }
    }
}
