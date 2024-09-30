using API.RequestDTO.Report;
using Core.Enities.ProjectAggregate;

namespace Core.Specifications.Report;

public class ProjectCompletionTrendSpecification : BaseSpecification<Project>
{
    public ProjectCompletionTrendSpecification(ReportFilterDto filter)
        : base(p =>
           p.Status==ProjectStatus.Completed && (!filter.ProjectId.HasValue || p.Id == filter.ProjectId.Value) &&
            (!filter.StartDate.HasValue || p.StartDate >= filter.StartDate.Value) &&
            (!filter.EndDate.HasValue || p.EndDate <= filter.EndDate.Value))
    {
    }
}
