using System.Net.Http.Headers;
using System.Text;

namespace JWT.Middlewares;

public class CustomExceptionHandler
{
    private readonly RequestDelegate _next;
    public CustomExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }
    
    // Implement exception handling here
    public async Task InvokeAsync(HttpContext context)
    {
        // Code to execute before the next middleware
        //Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
        
        //await _next(context);
        
        // Code to execute after the next middleware
        //Console.WriteLine($"Response: {context.Response.StatusCode}");
        
        if (context.Request.Headers.ContainsKey("Authorization"))  
        {            var authHeader = 
                AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]!);  
  
            if (authHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) &&  
                authHeader.Parameter != null)  
            {                var credentials = 
                    Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');  
                var username = credentials[0];  
                var password = credentials[1];  
                
                if (IsAuthorized(username, password))  
                {                    await _next(context);  
                    return;  
                }            }        }  
        context.Response.Headers["WWW-Authenticate"] = $"Basic realm=\"Authentication\"";  
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;  
    }
    private bool IsAuthorized(string username, string password)  
    { 
        return username == "admin" && password == "qwerty";
        
    }
}