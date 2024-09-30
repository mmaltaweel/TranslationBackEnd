using Core.DTO.Shared;
using Core.Enities.ProjectAggregate;
using Core.Helpers;

namespace Core.Specifications;

public class TasksForTranslatorSpecification : BaseSpecification<ProjectTask>
{
    public TasksForTranslatorSpecification(SharedParamFilter input)
        : base(x => x.AssignedTranslatorId == input.User.GetUserId())
    {
        ApplyPaging(input.Skip, input.Take);
        ApplyOrderByDescending(x => x.Id);
    }
    
}