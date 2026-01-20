using MyCv.ViewModels;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            await HandleExceptionAsync(context,ex); 
        }
    }
    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)StatusCodes.Status500InternalServerError;
        var now = DateTime.UtcNow;
        var errorResponse = new ErrorResultModel
        {
            StatusCode = context.Response.StatusCode,
            Message = ex.Message,
            Timestamp = now
        };
        return context.Response.WriteAsJsonAsync(errorResponse);
    }
    
}