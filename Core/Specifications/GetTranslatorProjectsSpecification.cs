using Core.Enities.ProjectAggregate;

namespace Core.Specifications;

internal class GetTranslatorProjectsSpecification : BaseSpecification<Project>
{
    public GetTranslatorProjectsSpecification(string assignedTranslatorId)
        : base(x => x.Tasks.Any(t => t.AssignedTranslatorId == assignedTranslatorId))
    {
 
    }
}
