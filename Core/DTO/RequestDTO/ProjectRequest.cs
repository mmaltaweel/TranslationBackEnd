using Core.DTO.Shared;

namespace API.RequestDTO;

public class ProjectRequest:PaginationFilter
{
    public string Name { get; set; }            
    public string Description { get; set; }    
    public DateTime StartDate { get; set; }    
    public DateTime EndDate { get; set; }      
 }