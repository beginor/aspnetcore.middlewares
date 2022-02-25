using Microsoft.AspNetCore.StaticFiles;

namespace Microsoft.Extensions.DependencyInjection; 

public static class ServiceCollectionExtensions {

    public static IServiceCollection ConfigureGzipStatic(this IServiceCollection services) {
        services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();
        return services;
    }

}