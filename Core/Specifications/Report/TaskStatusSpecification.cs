using API.RequestDTO.Report;
using Core.Enities.ProjectAggregate;

namespace Core.Specifications.Report;

public class TaskFilterSpecification : BaseSpecification<ProjectTask>
{
    public TaskFilterSpecification(ReportFilterDto filter)
        : base(t =>
            (!filter.DueDate.HasValue || t.DueDate.Date == filter.DueDate.Value.Date) &&
            (!filter.ProjectId.HasValue || t.ProjectId == filter.ProjectId.Value) &&
            (string.IsNullOrEmpty(filter.TranslatorId) || t.AssignedTranslatorId == filter.TranslatorId))
    {
        AddInclude(x=>x.AssignedTranslator);
    }
}