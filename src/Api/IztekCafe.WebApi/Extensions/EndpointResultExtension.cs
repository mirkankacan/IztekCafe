using IztekCafe.Application.Dtos.Common;
using System.Net;

namespace IztekCafe.WebApi.Extensions
{
    public static class EndpointResultExtension
    {
        public static IResult ToGenericResult<T>(this ServiceResult<T> serviceResult)
        {
            return serviceResult.StatusCode switch
            {
                HttpStatusCode.OK => Results.Ok(serviceResult.Data),
                HttpStatusCode.Created => Results.Created(serviceResult.UrlAsCreated, serviceResult.Data),
                HttpStatusCode.NotFound => Results.NotFound(serviceResult.Fail!),
                _ => Results.Problem(serviceResult.Fail!)
            };
        }

        public static IResult ToResult(this ServiceResult serviceResult)
        {
            return serviceResult.StatusCode switch
            {
                HttpStatusCode.NoContent => Results.NoContent(),
                HttpStatusCode.NotFound => Results.NotFound(serviceResult.Fail!),
                _ => Results.Problem(serviceResult.Fail!)
            };
        }
    }
}