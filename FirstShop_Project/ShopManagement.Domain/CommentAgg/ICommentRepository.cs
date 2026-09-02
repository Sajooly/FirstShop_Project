using _0_Framework.Domain;
using _0_Framework.Infrastructure;
using ShopManagement.Application.Contracts.Comment;
using System.Collections.Generic;

namespace ShopManagement.Domain.CommentAgg
{
    public interface ICommentRepository : IRepository<int, Comment>
    {
        List<CommentViewModel> Search(CommentSearchModel searchModel);
    }
}
