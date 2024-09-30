using System.Security.Claims;
using Core.DTO.Shared;

namespace API.Controllers.Helpers;

public static class ControllersHelper
{
    public static SharedParamFilter CreateSharedParamFilter(int skip, int take, ClaimsPrincipal user)
    {
        return new SharedParamFilter
        {
            Skip = skip,
            Take = take,
            User = user
        };
    }
}