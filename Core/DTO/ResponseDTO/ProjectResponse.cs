using Core.Enities.ProjectAggregate;

namespace Core.DTO.ResponseDTO;

public class ProjectResponse
{
    public  int Id { get;   set; }
    public string Name { get; set; }            
    public string Description { get; set; }    
    public DateTime StartDate { get; set; }    
    public DateTime EndDate { get; set; } 
    public string StatusDisplayName { get; set; } 
    public ProjectStatus Status { get; set; }
    public IReadOnlyCollection<TaskResponse> Tasks { get; set; }

    public int progress
    {
        get
        {
             int totalTasks = this.Tasks.Count();
             if (totalTasks == 0)
             {
                return 0;
             } 
             int completedTasks = this.Tasks.Count(x => x.Status == ProjectStatus.Completed);
             return completedTasks / totalTasks;
        }
    }
}