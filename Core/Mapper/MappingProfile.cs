using Core.DTO.ResponseDTO;
using Core.Enities.ProjectAggregate;

namespace Core.Mapper;

using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Create a map between Project and ProjectResponse
        CreateMap<Project, ProjectResponse>();
        CreateMap<ProjectResponse, Project>();
    }
}
