using System.Security.Claims;
using Core.Interfaces;

namespace Core.DTO.Shared;

public class SharedParamFilter:PaginationFilter
{
    public ClaimsPrincipal User{ get; set; }
}