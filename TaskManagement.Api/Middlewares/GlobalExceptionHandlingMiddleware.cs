using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace TaskManagement.Api.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = ex.GetType().Name,
                    Title = ex.Message,
                    Detail = ex.StackTrace
                };

                string json = JsonSerializer.Serialize(problem);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
        }

    }
}
