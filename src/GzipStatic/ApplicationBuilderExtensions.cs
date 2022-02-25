using Beginor.AspNetCore.Middlewares.GzipStatic;

namespace Microsoft.AspNetCore.Builder; 

public static class ApplicationBuilderExtensions {

    public static IApplicationBuilder UseGzipStatic(this IApplicationBuilder app) {
        app.UseMiddleware<GzipStaticMiddleware>();
        return app;
    }

}