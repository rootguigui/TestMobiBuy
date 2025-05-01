using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using TestMobiBuy.Api.Middlewares;

namespace TestMobiBuy.Api.Configuration;

[ExcludeFromCodeCoverage]
public static class MiddlewareExtensions
{
    public static void UseMiddleware(this WebApplication app, IConfiguration config)
    {
        app.UseStaticFiles();
        app.UseSwaggerDocs();
        app.UseCompression();
        app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        app.UseHttpsRedirection();
        app.UseResponseCaching();
        app.UseMiddleware<SecurityHeadersMiddleware>();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }

    public static void UseCompression(this WebApplication app)
    {
        app.UseMiddleware<CompressionMiddleware>();
        app.UseResponseCompression();
    }

    public static void UseSwaggerDocs(this WebApplication app)
    {
        if (app is null) throw new ArgumentNullException(nameof(app));
        
        app.Use(async (context, next) =>
        {
            if (context.Request.Path.Equals("/"))
            {
                context.Response.Redirect("/swagger");
                return;
            }

            await next();
        });

        app.UseSwagger();
        app.UseSwaggerUI(Options => 
        {
            Options.SwaggerEndpoint($"/swagger/v1/swagger.json", "v1");
            Options.DefaultModelExpandDepth(-1);            
        });
    }
}
