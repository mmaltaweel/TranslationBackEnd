using Core.Enities.ProjectAggregate;

namespace Core.Specifications;
 
internal class GetTranslatorsAuthorizedTasksSpecification : BaseSpecification<Project>
{
    public GetTranslatorsAuthorizedTasksSpecification(int taskId, string userId) 
        : base(x => x.Tasks.Any(task => task.Id == taskId && task.AssignedTranslatorId == userId))
    {
       
    }
}
