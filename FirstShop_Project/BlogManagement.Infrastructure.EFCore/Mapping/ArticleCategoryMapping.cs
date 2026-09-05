using BlogManagement.Domain.ArticleCategoryAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogManagement.Infrastructure.EFCore.Mapping
{
    public class ArticleCategoryMapping : IEntityTypeConfiguration<ArticleCategory>
    {
        public void Configure(EntityTypeBuilder<ArticleCategory> builder)
        {
            builder.ToTable("ArticleCategories");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name).HasMaxLength(500);
            builder.Property(a => a.Description).HasMaxLength(2000);
            builder.Property(a => a.Picture).HasMaxLength(750);
            builder.Property(a => a.PictureAlt).HasMaxLength(500);
            builder.Property(a => a.PictureTitle).HasMaxLength(500);
            builder.Property(a => a.Slug).HasMaxLength(600);
            builder.Property(a => a.Keyword).HasMaxLength(100);
            builder.Property(a => a.MetaDescription).HasMaxLength(150);
            builder.Property(a => a.CanonicalAddress).HasMaxLength(1000);


            builder.HasMany(ac => ac.Articles).WithOne(a => a.Category).HasForeignKey(a => a.CategoryId);
        }
    }
}
