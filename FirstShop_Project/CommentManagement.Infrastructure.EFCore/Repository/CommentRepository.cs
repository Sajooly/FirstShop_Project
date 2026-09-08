using _0_Framework.Application;
using _0_Framework.Infrastructure;
using CommentManagement.Application.Contracs.Comment;
using CommentManagement.Domain.CommentAgg;
using System.Collections.Generic;
using System.Linq;

namespace CommentManagement.Infrastructure.EFCore.Repository
{
    public class CommentRepository : RepositoryBase<int, Comment>, ICommentRepository
    {
        private readonly CommentContext _context;

        public CommentRepository(CommentContext context) : base(context)
        {
            _context = context;
        }

        public List<CommentViewModel> Search(CommentSearchModel searchModel)
        {
            var query = _context.Comments
                .Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Message = c.Message,
                    IsCanceled = c.IsCanceled,
                    IsConfirmed = c.IsConfirmed,
                    CommentDate = c.CreationDate.ToFarsi(),

                    OwnerRecordId = c.OwnerRecordId,
                    Website = c.Website,
                    Type = c.Type
                });

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(c => c.Name.Contains(searchModel.Name));

            if (!string.IsNullOrWhiteSpace(searchModel.Email))
                query = query.Where(c => c.Email.Contains(searchModel.Email));

            return query.OrderByDescending(c => c.Id).ToList();
        }
    }
}
