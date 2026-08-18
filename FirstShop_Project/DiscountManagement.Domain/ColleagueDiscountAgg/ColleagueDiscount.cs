using _0_Framework.Domain;

namespace DiscountManagement.Domain.ColleagueDiscountAgg
{
    public class ColleagueDiscount : EntityBase
    {
        public int ProductId { get; private set; }
        public int DiscountRate { get; private set; }
        public bool IsRemoved { get; set; }


        //public bool IsColleague { get; private set; } //خودم برای این خواستم باشه ک شاید طرف بعدا از لیست همکار های ما خارج شد 

        public ColleagueDiscount(int productId, int discountRate)
        {
            ProductId = productId;
            DiscountRate = discountRate;
            IsRemoved = false;
        }
        public void Edit(int productId, int discountRate)
        {
            ProductId = productId;
            DiscountRate = discountRate;
        }
        public void Remove()
        {
            IsRemoved = true;
        }
        public void Restore()
        {
            IsRemoved = false;
        }

    }
}
