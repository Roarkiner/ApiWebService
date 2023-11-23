using Microsoft.AspNetCore.Mvc;

namespace ApiWebService.Api.Extensions
{
    public static class Extensions
    {
        public static CreatedResult Created(this ControllerBase controllerBase, HttpContext httpContext, string endpointsName, object? value, string valueEndpoint)
        {
            return controllerBase.Created($"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/Note/{endpointsName}/{valueEndpoint}", value);
        }
    }
}
