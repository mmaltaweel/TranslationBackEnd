using Core.DTO.RequestDTO;
using Core.DTO.Shared;

namespace API.RequestDTO;

public class ProjectTaskRequest
{
    public string Title { get; set; }              
    public string Description { get; set; }         
    public DateTime DueDate { get; set; }           
    public string AssignedTranslatorId { get; set; } 
}