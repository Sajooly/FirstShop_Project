using _0_Framework.Domain;
using CommentManagement.Application.Contracs.Comment;
using System.Collections.Generic;

namespace CommentManagement.Domain.CommentAgg
{
    public interface ICommentRepository : IRepository<int, Comment>
    {
        List<CommentViewModel> Search(CommentSearchModel searchModel);
    }
}
