
using System.ComponentModel.DataAnnotations;

namespace EcoAlert.DTOs
{
    public class CreateIssueDto
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public int CategoryId { get; set; }
        public List<IFormFile>? Images { get; set; }
    }

    public class UpdateIssueStatusDto
    {
        public int NewStatusId { get; set; }
        public string? Description { get; set; }
    }

    public class IssueDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Category { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public int UpvoteCount { get; set; }
        public int CommentCount { get; set; }
        public DateTime ReportedAt { get; set; }
        public required UserDto Reporter { get; set; }
        public List<IssueImageDto>? Images { get; set; }

    }
    public class IssueResponseDto : IssueDto
    {
        public List<IssueImageDto>? Images { get; set; }
        public List<CommentDto>? Comments { get; set; }
    }
    public class IssueUpdateDto
    {
        public int IssueId { get; set; }
        public int? OldStatusId { get; set; }
        public int NewStatusId { get; set; }
        public string? Description { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
    public class IssueFilterDto
        {
            public int? CategoryId { get; set; }
            public int? StatusId { get; set; }
            public string? City { get; set; }
           
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 20;
           
        }
    }
