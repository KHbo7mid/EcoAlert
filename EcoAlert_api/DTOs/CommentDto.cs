namespace EcoAlert.DTOs
{
    public class CreateCommentDto
    {
        public required  string Content { get; set; }
    }

    public class CommentDto
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public required UserDto User { get; set; }
    }
}
