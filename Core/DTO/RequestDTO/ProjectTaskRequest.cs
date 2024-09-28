using Core.DTO.Shared;

namespace API.RequestDTO;

public class ProjectTaskRequest:PaginationFilter
{
    public  int Id { get;   set; }
    public string Title { get; set; }              
    public string Description { get; set; }         
    public DateTime DueDate { get; set; }           
    public string AssignedTranslatorId { get; set; } 
}