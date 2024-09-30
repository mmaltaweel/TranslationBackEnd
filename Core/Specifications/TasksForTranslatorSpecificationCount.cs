using Core.DTO.Shared;
using Core.Enities.ProjectAggregate;
using Core.Helpers;

namespace Core.Specifications;

public class TasksForTranslatorSpecificationCount : BaseSpecification<ProjectTask>
{
    public TasksForTranslatorSpecificationCount(SharedParamFilter input)
        : base(x => x.AssignedTranslatorId == input.User.GetUserId())
    {
        
    }
    
}