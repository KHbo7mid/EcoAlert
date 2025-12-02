namespace EcoAlert.DTOs
{
    public class IssueImageDto
    {
        public int Id { get; set; }
        public required string ImageUrl { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class UploadImageDto
    {
        public required IFormFile Image { get; set; }
        public int IssueId { get; set; }
    }
}
