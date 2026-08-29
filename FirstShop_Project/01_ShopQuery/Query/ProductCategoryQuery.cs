using _0_Framework.Application;
using _01_ShopQuery.Contracts.Product;
using _01_ShopQuery.Contracts.ProductCategory;
using DiscountManagement.Infrastructure.EFCore;
using InventoryManagement.Domain.InventoryAgg;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _01_ShopQuery.Query
{
    public class ProductCategoryQuery : IProductCategoryQuery
    {
        private readonly ShopContext _shopContext;
        private readonly InventoryContext _inventoryContext;
        private readonly DiscountContext _discountContext;

        public ProductCategoryQuery(ShopContext shopContext, InventoryContext inventoryContext, DiscountContext discountContext)
        {
            _shopContext = shopContext;
            _inventoryContext = inventoryContext;
            _discountContext = discountContext;
        }

        public List<ProductCategoryQueryModel> GetProductCategories()
        {
            return _shopContext.ProductCategories.Select(x => new ProductCategoryQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Slug = x.Slug

            }).ToList();
        }

        public List<ProductCategoryQueryModel> GetProductCategoriesWithProduct()
        {
            var inventory = _inventoryContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice }).ToList();

            var discounts = _discountContext.CustomerDiscounts.Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).Select(x => new { x.DiscountRate, x.ProductId }).ToList();

            var category = _shopContext.ProductCategories
                .Include(x => x.Products)
                //.ThenInclude(x => x.Category)
                .Select(x => new ProductCategoryQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle,
                    Slug = x.Slug,

                    Products = x.Products.Select(p => new ProductQueryModel
                    {
                        Id = p.Id,
                        Category = p.Category.Name,
                        Name = p.Name,
                        Picture = p.Picture,
                        PictureAlt = p.PictureAlt,
                        PictureTitle = p.PictureTitle,
                        Slug = p.Slug,
                    }).OrderByDescending(p => p.Id).ToList()
                }).ToList();


            category.ForEach(c =>
                   c.Products.ForEach(p =>
                   {
                       var price = inventory.FirstOrDefault(x => x.ProductId == p.Id)?.UnitPrice ?? 0;
                       p.Price = price.ToMoney();

                       p.DiscountRate = discounts.FirstOrDefault(x => x.ProductId == p.Id)?.DiscountRate ?? 0;
                       p.HasDiscount = p.DiscountRate > 0;

                       var discountAmount = Math.Round((Convert.ToDouble(price) * p.DiscountRate) / 100);

                       var finalPrice = Convert.ToDouble(price) - discountAmount;
                       p.PriceWithDiscount = finalPrice.ToMoney();
                   }
                   ));

            return category;
        }

        public ProductCategoryQueryModel GetProductCategoryWithProductBy(string slug)
        {
            var inventory = _inventoryContext.Inventory.Select(x => new { x.ProductId, x.UnitPrice }).ToList();

            var discounts = _discountContext.CustomerDiscounts.Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now).Select(x => new { x.DiscountRate, x.ProductId, x.EndDate }).ToList();

            var category = _shopContext.ProductCategories
                .Include(x => x.Products)
                .Select(pc => new ProductCategoryQueryModel
                {
                    Id = pc.Id,
                    Name = pc.Name,
                    Picture = pc.Picture,
                    PictureAlt = pc.PictureAlt,
                    PictureTitle = pc.PictureTitle,
                    Slug = pc.Slug,
                    Keyword = pc.Keywords,
                    MetaDescription = pc.MetaDescription,
                    Description = pc.Description,

                    Products = pc.Products.Select(p => new ProductQueryModel
                    {
                        Id = p.Id,
                        Category = p.Category.Name,
                        Name = p.Name,
                        Picture = p.Picture,
                        PictureAlt = p.PictureAlt,
                        PictureTitle = p.PictureTitle,
                        Slug = p.Slug,
                    }).OrderByDescending(p => p.Id).ToList()

                }).FirstOrDefault(x => x.Slug == slug);


            foreach (var product in category.Products)
            {
                var price = inventory.FirstOrDefault(x => x.ProductId == product.Id)?.UnitPrice ?? 0;
                product.Price = price.ToMoney();

                product.DiscountRate = discounts.FirstOrDefault(x => x.ProductId == product.Id)?.DiscountRate ?? 0;
                product.HasDiscount = product.DiscountRate > 0;


                var discountAmount = Math.Round((price * product.DiscountRate) / 100);

                var finalPrice = price - discountAmount;
                product.PriceWithDiscount = finalPrice.ToMoney();

                product.DiscountExpireDate = discounts.FirstOrDefault(x => x.ProductId == product.Id)?.EndDate.ToDiscountFormat();
            }

            return category;
        }
    }
}
