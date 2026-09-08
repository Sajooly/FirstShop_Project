using System.Collections.Generic;

namespace _01_ShopQuery.Contracts.Comment
{
    public class CommentQueryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string Website { get; set; }
        public int ParentId { get; set; }
        public List<CommentQueryModel> Children { get; set; } = new();
    }
}
