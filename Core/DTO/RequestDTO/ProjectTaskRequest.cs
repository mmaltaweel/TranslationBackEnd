namespace API.RequestDTO;

public class ProjectTaskRequest
{
    public  int Id { get;   set; }
    public string Title { get; set; }              
    public string Description { get; set; }         
    public DateTime DueDate { get; set; }           
    public string AssignedTranslatorId { get; set; } 
}