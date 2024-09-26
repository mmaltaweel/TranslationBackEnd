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
    public List<TaskResponse> Tasks { get; set; }
}