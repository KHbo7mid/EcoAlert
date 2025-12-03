using AutoMapper;
using EcoAlert.DTOs;
using EcoAlert.Models;

namespace EcoAlert
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role,
                    opt => opt.MapFrom(src => src.Role.Name));

            // Issue mappings
            CreateMap<Issue, IssueDto>()
                .ForMember(dest => dest.Category,
                    opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.Name))
                .ForMember(dest => dest.Priority,
                    opt => opt.MapFrom(src => src.Priority.Name ))
                .ForMember(dest => dest.CommentCount,
                    opt => opt.MapFrom(src => src.Comments.Count))
                .ForMember(dest => dest.Reporter,
                    opt => opt.MapFrom(src => src.Reporter))
                 .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Issueimages));

            CreateMap<Issue, IssueResponseDto>()
                .IncludeBase<Issue, IssueDto>()
                .ForMember(dest => dest.Images,
                    opt => opt.MapFrom(src => src.Issueimages))
                .ForMember(dest => dest.Comments,
                    opt => opt.MapFrom(src => src.Comments));

            CreateMap<CreateIssueDto, Issue>();

            // Comment mappings
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.User,
                    opt => opt.MapFrom(src => src.User));

            // Image mappings
            CreateMap<Issueimage, IssueImageDto>();
        }
    }
}
