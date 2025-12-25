using AutoMapper;
using TrackItAPI.DTOs.Issue;
using TrackItAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TrackItAPI.Mappings
{
    public class IssueProfile : Profile
    {
        public IssueProfile()
        {
            // Entity → Read DTO
            CreateMap<Issue, IssueReadDto>();

            // Create DTO → Entity
            CreateMap<IssueCreateDto, Issue>();

            // Update DTO → Entity
            CreateMap<IssueUpdateDto, Issue>();
        }
    }
}
