using Core.DTO.Shared;
using Core.Enities.ProjectAggregate;
using Core.Helpers;

namespace Core.Specifications;

public class ProjectWithTasksForManagerSpecificationCount : BaseSpecification<Project>
{
    public ProjectWithTasksForManagerSpecificationCount(SharedParamFilter input)
        : base(x => x.ProjectManagerId == input.User.GetUserId())
    {
        
    }
    
}