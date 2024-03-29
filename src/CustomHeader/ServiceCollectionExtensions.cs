using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Beginor.AspNetCore.Middlewares.CustomHeader;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions {

    public static IServiceCollection ConfigureCustomHeader(this IServiceCollection services, IConfigurationSection config) {
        services.Configure<Dictionary<string, Dictionary<string, string>>>(Consts.OptionsName, config);
        return services;
    }

}
