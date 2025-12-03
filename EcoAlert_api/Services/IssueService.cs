using AutoMapper;
using EcoAlert.DTOs;
using EcoAlert.Models;
using EcoAlert.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static EcoAlert.DTOs.IssueUpdateDto;
namespace EcoAlert.Services
{
    public class IssueService : IIssueService

    {
        private readonly EcoAlertDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<IssueService> _logger;
        private readonly IImageService _imageService;

        public IssueService(EcoAlertDbContext context, IMapper mapper, ILogger<IssueService> logger, IImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _imageService = imageService;
        }

      

        public async Task<IssueResponseDto> CreateIssueAsync(CreateIssueDto dto, int userId)
        {
            try
            {
                // Validate user exists
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    throw new Exception("User not found");



                // Create issue
                var issue = new Issue
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    Latitude = dto.Latitude,
                    Longitude = dto.Longitude,
                    Address = dto.Address,
                    City = dto.City,
                    CategoryId = dto.CategoryId,
                    StatusId = 1, // Reported
                    PriorityId = 2, // Medium
                    ReporterId = userId,
                    ReportedAt = DateTime.UtcNow,
                    AipriorityScore = 50.0m,
                    UpvoteCount = 0,
                    CommentCount = 0,
                    ViewCount = 0
                };

                _context.Issues.Add(issue);
                await _context.SaveChangesAsync();
                // Handle image uploads if any
                if (dto.Images != null && dto.Images.Any())
                {
                    var imageUrls = await _imageService.UploadMultipleImageAsync(dto.Images);

                    for (int i = 0; i < imageUrls.Count; i++)
                    {
                        var issueImage = new Issueimage
                        {
                            IssueId = issue.Id,
                            ImageUrl = imageUrls[i],
                            IsPrimary = i == 0, // First image is primary
                            UploadedAt = DateTime.UtcNow
                        };

                        _context.Issueimages.Add(issueImage);
                    }

                    await _context.SaveChangesAsync();
                }

                _logger.LogInformation($"Issue {issue.Id} created by user {userId}");

                // Return the created issue
                return await GetIssueByIdAsyc(issue.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating issue");
                throw;
            }
        }

        public async Task<bool> DeleteIssueAsync(int issueId, int userId)
        {
            try
            {
                var issue = await _context.Issues.FindAsync(issueId);
                if (issue == null)
                    return false;

                // Check permissions (simplified)
                if (issue.ReporterId != userId)
                    throw new UnauthorizedAccessException("You can only delete your own issues");

                _context.Issues.Remove(issue);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Issue {issueId} deleted by user {userId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting issue {issueId}");
                throw;
            }
        }

        public async Task<IssueResponseDto> GetIssueByIdAsyc(int id)
        {
            try
            {
                var issue = await _context.Issues
                    .AsNoTracking() 
                    .Include(i => i.Category)
                    .Include(i => i.Status)
                    .Include(i => i.Priority)
                    .Include(i => i.Reporter)
                        .ThenInclude(r => r.Role)
                    .Include(i => i.AssignedTo)
                        .ThenInclude(a => a.Role)
                    .Include(i => i.Issueimages)
                    .Include(i => i.Comments)
                        .ThenInclude(c => c.User)
                            .ThenInclude(u => u.Role)
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (issue == null)
                    throw new Exception($"Issue with ID {id} not found");

                // Increment view count
                issue.ViewCount++;
                await _context.SaveChangesAsync();

                return _mapper.Map<IssueResponseDto>(issue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting issue {id}");
                throw;
            }
        }

        public async Task<List<CommentDto>> GetIssueCommentsAsync(int issueId)
        {
            var issue = await _context.Issues
                .Include(i => i.Comments)
                    .ThenInclude(c => c.User)
                        .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(i => i.Id == issueId);

            if (issue == null)
                throw new KeyNotFoundException("Issue not found");

            var commentsDto = issue.Comments
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt ?? DateTime.MinValue,
                    User = _mapper.Map<UserDto>(c.User)
                })
                .ToList();

            return _mapper.Map<List<CommentDto>>(commentsDto);
        }

        public  async Task<List<IssueDto>> GetIssuesAsync(IssueFilterDto filter)
        {
            try
            {
                var query = _context.Issues
                    .AsNoTracking()
                    .Include(i => i.Category)
                    .Include(i => i.Status)
                    .Include(i => i.Priority)
                    .Include(i => i.Reporter)
                    .Include(i =>i.Issueimages)
                    .AsQueryable();

                // Apply filters
                if (filter.CategoryId.HasValue)
                    query = query.Where(i => i.CategoryId == filter.CategoryId.Value);

                if (filter.StatusId.HasValue)
                    query = query.Where(i => i.StatusId == filter.StatusId.Value);

                if (!string.IsNullOrEmpty(filter.City))
                    query = query.Where(i => i.City == filter.City);
                query = query.OrderByDescending(i => i.ReportedAt);
                // Pagination
                query = query
                    .OrderByDescending(i => i.ReportedAt)
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);

                var issues = await query.ToListAsync();
                return _mapper.Map<List<IssueDto>>(issues);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting issues");
                throw;
            }
        }

        public async Task<IssueResponseDto> UpdateIssueStatusAsync(int issueId, UpdateIssueStatusDto updateDto, int userId)
        {
            try
            {
                var issue = await _context.Issues.FindAsync(issueId);
                if (issue == null)
                    throw new Exception($"Issue with ID {issueId} not found");

                var oldStatusId = issue.StatusId;
                issue.StatusId = updateDto.NewStatusId;

                if (updateDto.NewStatusId == 4) // Resolved
                    issue.ResolvedAt = DateTime.UtcNow;

                // Record the update
                var update = new Issueupdate
                {
                    IssueId = issueId,
                    OldStatusId = oldStatusId,
                    NewStatusId = updateDto.NewStatusId,
                    Description = updateDto.Description,
                    UpdatedById = userId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Issueupdates.Add(update);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Issue {issueId} status updated by user {userId}");
                return await GetIssueByIdAsyc(issueId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating issue {issueId} status");
                throw;
            }
        }

        public async Task<IssueResponseDto> UpvoteIssueAsync(int issueId, int userId)
        {
            try
            {
                var issue = await _context.Issues.FindAsync(issueId);
                if (issue == null)
                    throw new Exception($"Issue with ID {issueId} not found");

                issue.UpvoteCount++;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Issue {issueId} upvoted by user {userId}");
                return await GetIssueByIdAsyc(issueId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error upvoting issue {issueId}");
                throw;
            }
        }
        public async Task<CommentDto> AddCommentAsync(int issueId, CreateCommentDto commentDto, int userId)
        {
            try
            {
                var issue = await _context.Issues.FindAsync(issueId);
                if (issue == null)
                    throw new Exception($"Issue with ID {issueId} not found");

                var comment = new Comment
                {
                    IssueId = issueId,
                    UserId = userId,
                    Content = commentDto.Content,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Comments.Add(comment);
                issue.CommentCount++;
                await _context.SaveChangesAsync();

                // Get the full comment with user info
                var savedComment = await _context.Comments
                    .AsNoTracking()
                    .Include(c => c.User)
                        .ThenInclude(u => u.Role)
                    .FirstOrDefaultAsync(c => c.Id == comment.Id);

                return _mapper.Map<CommentDto>(savedComment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding comment to issue {issueId}");
                throw;
            }
        }
    }
}
