using Beginor.AspNetCore.Middlewares.CustomHeader;

namespace Microsoft.AspNetCore.Builder {

    public static class ApplicationBuilderExtensions {

        public static IApplicationBuilder UseCustomHeader(
            this IApplicationBuilder app
        ) {
            app.UseMiddleware<CustomHeaderMiddleware>();
            return app;
        }

    }

}
