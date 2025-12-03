using EcoAlert.DTOs;
using EcoAlert.Models;
using EcoAlert.Services;
using EcoAlert.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static EcoAlert.DTOs.IssueUpdateDto;
namespace EcoAlert.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IssueController : ControllerBase
    {
        private readonly IIssueService _issueService;
        private readonly ILogger<IssueService> _logger;
        public IssueController(IIssueService issueService, ILogger<IssueService> logger)
        {
            _issueService = issueService;
            _logger = logger;
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateIssue([FromForm] CreateIssueDto createIssueDto)
        {
            try
            {
                _logger.LogInformation("Creating new issue: {Title}", createIssueDto.Title);

                // Get userId from JWT token
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var issue = await _issueService.CreateIssueAsync(createIssueDto, userId);

                return CreatedAtAction(nameof(GetIssue), new { id = issue.Id }, new
                {
                    Success = true,
                    Message = "Issue created successfully",
                    Data = issue
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating issue: {Title}", createIssueDto.Title);
                return BadRequest(new
                {
                    Success = false,
                    Message = "Error creating issue",
                    Error = ex.Message
                });
            }

        }
        [HttpGet("{id}")]
        [AllowAnonymous] // Allow public access to view single issue
        public async Task<IActionResult> GetIssue(int id)
        {
            try
            {
                _logger.LogInformation("Getting issue with ID: {IssueId}", id);

                var issue = await _issueService.GetIssueByIdAsyc(id);

                return Ok(new
                {
                    Success = true,
                    Data = issue
                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Issue not found: {IssueId}", id);
                return NotFound(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting issue {IssueId}", id);
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Error retrieving issue",
                    Error = ex.Message
                });
            }
        }
        [HttpGet]
        [AllowAnonymous] // Allow public access to view issues
        public async Task<IActionResult> GetIssues([FromQuery] IssueFilterDto filter)
        {
            try
            {
                _logger.LogInformation("Getting issues with filter: {@Filter}", filter);

                var issues = await _issueService.GetIssuesAsync(filter);

                return Ok(new
                {
                    Success = true,
                    Total = issues.Count,
                    Page = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Data = issues
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting issues");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Error retrieving issues",
                    Error = ex.Message
                });
            }
        }
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Official,Admin")] // Only officials/admins can update status
        public async Task<IActionResult> UpdateIssueStatus(int id, [FromBody] UpdateIssueStatusDto updateDto)
        {
            try
            {
                _logger.LogInformation("Updating issue {IssueId} status to {NewStatus}", id, updateDto.NewStatusId);

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var issue = await _issueService.UpdateIssueStatusAsync(id, updateDto, userId);

                return Ok(new
                {
                    Success = true,
                    Message = "Issue status updated successfully",
                    Data = issue
                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Issue not found for status update: {IssueId}", id);
                return NotFound(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized status update for issue {IssueId}", id);
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating issue {IssueId} status", id);
                return BadRequest(new
                {
                    Success = false,
                    Message = "Error updating issue status",
                    Error = ex.Message
                });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIssue(int id)
        {
            try
            {
                _logger.LogInformation("Deleting issue {IssueId}", id);

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                await _issueService.DeleteIssueAsync(id, userId);

                return Ok(new
                {
                    Success = true,
                    Message = "Issue deleted successfully"
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized delete attempt for issue {IssueId}", id);
                return Forbid();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Issue not found for deletion: {IssueId}", id);
                return NotFound(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting issue {IssueId}", id);
                return BadRequest(new
                {
                    Success = false,
                    Message = "Error deleting issue",
                    Error = ex.Message
                });
            }
        }
        [HttpPost("{id}/upvote")]
        public async Task<IActionResult> UpvoteIssue(int id)
        {
            try
            {
                _logger.LogInformation("Upvoting issue {IssueId}", id);

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var issue = await _issueService.UpvoteIssueAsync(id, userId);

                return Ok(new
                {
                    Success = true,
                    Message = "Issue upvoted successfully",
                    Data = issue
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error upvoting issue {IssueId}", id);
                return BadRequest(new
                {
                    Success = false,
                    Message = "Error upvoting issue",
                    Error = ex.Message
                });
            }
        }
        [HttpPost("{id}/comments")]
        public async Task<IActionResult> AddComment(int id, [FromBody] CreateCommentDto commentDto)
        {
            try
            {
                _logger.LogInformation("Adding comment to issue {IssueId}", id);

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var comment = await _issueService.AddCommentAsync(id, commentDto, userId);

                return Ok(new
                {
                    Success = true,
                    Message = "Comment added successfully",
                    Data = comment
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment to issue {IssueId}", id);
                return BadRequest(new
                {
                    Success = false,
                    Message = "Error adding comment",
                    Error = ex.Message
                });
            }
        }
        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetComments(int id)
        {
            try
            {
                var comments = await _issueService.GetIssueCommentsAsync(id);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting comments for issue {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
