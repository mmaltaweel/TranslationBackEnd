using Core.Enities.ProjectAggregate;

namespace Core.Specifications;


internal class GetManagerAuthorizedProjectsSpecification : BaseSpecification<Project> 
{
        public GetManagerAuthorizedProjectsSpecification(int projectId,string userId) : base(x => (x.ProjectManagerId == userId) && (x.Id == projectId))
        {

        } 
    }