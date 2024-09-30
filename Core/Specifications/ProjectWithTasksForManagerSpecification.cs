using Core.DTO.Shared;
using Core.Enities.ProjectAggregate;
using Core.Helpers;

namespace Core.Specifications;

public class ProjectWithTasksForManagerSpecification : BaseSpecification<Project>
{
    public ProjectWithTasksForManagerSpecification(SharedParamFilter input)
        : base(x => x.ProjectManagerId == input.User.GetUserId())
    {
        AddInclude(x => x.Tasks);
        ApplyPaging(input.Skip, input.Take);
        ApplyOrderByDescending(x => x.Id);
    }
    
}