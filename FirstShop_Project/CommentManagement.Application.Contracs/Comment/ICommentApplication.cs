using _0_Framework.Application;
using System.Collections.Generic;

namespace CommentManagement.Application.Contracs.Comment
{
    public interface ICommentApplication
    {
        OperationResult Add(AddComment command);
        OperationResult Cancel(int id);
        OperationResult Confirm(int id);
        List<CommentViewModel> Search(CommentSearchModel searchModel);
    }
}
