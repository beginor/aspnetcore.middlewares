using Beginor.AspNetCore.Middlewares.SpaFailback;

namespace Microsoft.AspNetCore.Builder {

    public static class ApplicationBuilderExtensions {

        public static IApplicationBuilder UseSpaFailback(
            this IApplicationBuilder app
        ) {
            return app.UseMiddleware<SpaFailbackMiddleware>();
        }
    }


}
