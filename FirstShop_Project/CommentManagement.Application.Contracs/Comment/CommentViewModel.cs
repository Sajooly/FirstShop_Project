namespace CommentManagement.Application.Contracs.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsCanceled { get; set; }
        public string CommentDate { get; set; }


        public int OwnerRecordId { get; set; }
        public string OwnerName { get; set; }
        public string Website { get; set; }
        public int Type { get; set; }
    }
}
