using Microsoft.Extensions.Configuration;
using Beginor.AspNetCore.Middlewares.SpaFailback;

namespace Microsoft.Extensions.DependencyInjection {

    public static class ServiceCollectionExtensions {

        public static IServiceCollection ConfigureSpaFailback(this IServiceCollection services, IConfigurationSection config) {
            services.Configure<SpaFailbackOptions>(config);
            return services;
        }

    }

}
