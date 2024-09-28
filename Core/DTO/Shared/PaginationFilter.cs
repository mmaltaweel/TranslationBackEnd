using Core.Interfaces;

namespace Core.DTO.Shared;

public abstract class PaginationFilter 
{
    public int Skip { get; set; }
    public int Take { get; set; }
}