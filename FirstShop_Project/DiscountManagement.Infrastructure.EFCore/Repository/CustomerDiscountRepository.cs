using _0_Framework.Application;
using _0_Framework.Infrastructure;
using DiscountManagement.Application.Contract.CustomerDiscount;
using DiscountManagement.Domain.CustomerDiscountAgg;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;

namespace DiscountManagement.Infrastructure.EFCore.Repository
{
    public class CustomerDiscountRepository : RepositoryBase<int, CustomerDiscount>, ICustomerDiscountRepository
    {
        private readonly DiscountContext _context;
        private readonly ShopContext _shopContext;

        public CustomerDiscountRepository(DiscountContext context, ShopContext shopContext) : base(context)
        {
            _context = context;
            _shopContext = shopContext;
        }

        public EditCustomerDiscount GetDetails(int id)
        {
            return _context.CustomerDiscounts.Select(x => new EditCustomerDiscount
            {
                Id = x.Id,
                DiscountRate = x.DiscountRate,
                StartDate = x.StartDate.ToFarsi(),
                EndDate = x.EndDate.ToFarsi(),
                ProductId = x.ProductId,
                Reason = x.Reason

            }).FirstOrDefault(x => x.Id == id);
        }

        public List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel)
        {
            var products = _shopContext.Products.Select(x => new { x.Id, x.Name }).ToList();

            var query = _context.CustomerDiscounts
                .Select(x => new CustomerDiscountViewModel
                {
                    Id = x.Id,
                    DiscountRate = x.DiscountRate,
                    Reason = x.Reason,
                    ProductId = x.ProductId,
                    StartDate = x.StartDate.ToFarsi(),
                    EndDate = x.EndDate.ToFarsi(),
                    StartDateEnglish = x.StartDate,
                    EndDateEnglish = x.EndDate,
                    CreationDate = x.CreationDate.ToFarsi()
                });
            if (searchModel.ProductId > 0 && searchModel.EndDate == searchModel.StartDate)
                query = query.Where(x => x.ProductId == searchModel.ProductId);

            if (!string.IsNullOrWhiteSpace(searchModel.StartDate) && searchModel.StartDate != searchModel.EndDate)
            {
                query = query.Where(x => x.StartDateEnglish <= searchModel.StartDate.ToGeorgianDateTime());
            }
            if (!string.IsNullOrWhiteSpace(searchModel.EndDate) && searchModel.StartDate != searchModel.EndDate)
            {
                query = query.Where(x => x.EndDateEnglish >= searchModel.EndDate.ToGeorgianDateTime());
            }

            var result = query.OrderByDescending(x => x.Id).ToList();

            result.ForEach(result => result.ProductName = products.FirstOrDefault(x => x.Id == result.ProductId)?.Name);

            return result;


            #region Second Code For This Method but it was bug
            //var query2 = _context.CustomerDiscounts
            //    .Join(_shopContext.Products,
            //    customerDiscount => customerDiscount.ProductId,
            //    product => product.Id,
            //    (C, P) => new CustomerDiscountViewModel
            //    {
            //        Id = C.Id,
            //        DiscountRate = C.DiscountRate,
            //        Reason = C.Reason,
            //        ProductId = C.ProductId,
            //        StartDate = C.StartDate.ToString(),
            //        EndDate = C.EndDate.ToString(),
            //        StartDateEnglish = C.StartDate,
            //        EndDateEnglish = C.EndDate,
            //        ProductName = P.Name
            //    });

            //if (searchModel.ProductId > 0)
            //    query2 = query2.Where(x => x.ProductId == searchModel.ProductId);
            //if (!string.IsNullOrWhiteSpace(searchModel.StartDate))
            //{
            //    query2 = query2.Where(x => x.StartDateEnglish < searchModel.StartDate.ToGeorgianDateTime());
            //}
            //if (!string.IsNullOrWhiteSpace(searchModel.EndDate))
            //{
            //    query2 = query2.Where(x => x.EndDateEnglish > searchModel.EndDate.ToGeorgianDateTime());
            //}
            //if (!string.IsNullOrWhiteSpace(searchModel.ProductName))
            //    query2 = query2.Where(x => x.ProductName.Contains(searchModel.ProductName));
            //return query2.OrderByDescending(x => x.Id).ToList();
            #endregion
        }
    }
}
