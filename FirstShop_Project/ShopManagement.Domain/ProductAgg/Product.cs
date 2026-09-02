using _0_Framework.Domain;
using ShopManagement.Domain.CommentAgg;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Domain.ProductPictureAgg;
using System.Collections.Generic;

namespace ShopManagement.Domain.ProductAgg
{
    public class Product : EntityBase
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        //public double UnitPrice { get; private set; } //This Property Added To Inventory Model
        //public bool IsInStore { get; private set; }   //This Property Added To Inventory Model
        public string ShortDescription { get; private set; }
        public string Description { get; private set; }
        public string Picture { get; private set; }
        public string PictureAlt { get; private set; }
        public string PictureTitle { get; private set; }

        //SEO Property
        public string Keywords { get; private set; }
        public string MetaDescription { get; private set; }
        public string Slug { get; private set; }

        //Navigation Property For Realation
        public int CategoryId { get; private set; }
        public ProductCategory Category { get; private set; }
        public List<ProductPicture> ProductPictures { get; private set; }
        public List<Comment> Comments { get;private set; }

        public Product(string name, string code/*, double unitPrice*/, string shortDescription, string description,
            string picture, string pictureAlt, string pictureTitle, string keywords, string metaDescription, string slug, int categoryId)
        {
            Name = name;
            Code = code;
            //UnitPrice = unitPrice; //This Property Added To Inventory Model
            ShortDescription = shortDescription;
            Description = description;
            Picture = picture;
            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
            Keywords = keywords;
            MetaDescription = metaDescription;
            Slug = slug;
            CategoryId = categoryId;
            //IsInStore = true; //This Property Added To Inventory Model
        }

        public void Edit(string name, string code/*, double unitPrice*/, string shortDescription, string description,
            string picture, string pictureAlt, string pictureTitle, string keywords, string metaDescription, string slug, int categoryId)
        {
            Name = name;
            Code = code;
            //UnitPrice = unitPrice; //This Property Added To Inventory Model
            ShortDescription = shortDescription;
            Description = description;

            if (!string.IsNullOrWhiteSpace(picture))
                Picture = picture;

            PictureAlt = pictureAlt;
            PictureTitle = pictureTitle;
            Keywords = keywords;
            MetaDescription = metaDescription;
            Slug = slug;
            CategoryId = categoryId;
        }

        //public void AddInStore()
        //{
        //    IsInStore = true; //This Property Added To Inventory Model
        //}
        //public void RemoveInStore()
        //{
        //    IsInStore = false; //This Property Added To Inventory Model
        //}
    }
}
