using EcoAlert.DTOs;
using EcoAlert.Models;

namespace EcoAlert.Services.Interface
{
    public interface IIssueService
    {
        Task<IssueResponseDto> CreateIssueAsync(CreateIssueDto createIssueDto, int userId);
        Task<IssueResponseDto> GetIssueByIdAsyc(int id);
        Task<List<IssueDto>> GetIssuesAsync(IssueFilterDto filter);
        Task<IssueResponseDto> UpdateIssueStatusAsync(int issueId, UpdateIssueStatusDto updateDto, int userId);
        Task<bool> DeleteIssueAsync(int issueId, int userId);
       
        Task<IssueResponseDto> UpvoteIssueAsync(int issueId, int userId);
        Task<CommentDto> AddCommentAsync(int issueId, CreateCommentDto commentDto, int userId);
        Task<List<CommentDto>> GetIssueCommentsAsync(int issueId);
        //Task<IEnumerable<IssueDto>> GetNearbyIssuesAsync(double latitude, double longitude, double radiusKm);
        //Task ProcessAIPrioritizationAsync(int issueId);
    }
}
