using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Constriva.API.Filters;

[AttributeUsage(AttributeTargets.Method)]
public class RawBodyAttribute : Attribute, IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(
        ResourceExecutingContext context,
        ResourceExecutionDelegate next)
    {
        context.HttpContext.Request.EnableBuffering();

        using var reader = new StreamReader(
            context.HttpContext.Request.Body,
            encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            leaveOpen: true);

        var rawBody = await reader.ReadToEndAsync(context.HttpContext.RequestAborted);

        context.HttpContext.Items["RawBody"] = rawBody;
        context.HttpContext.Request.Body.Position = 0;

        await next();
    }
}
