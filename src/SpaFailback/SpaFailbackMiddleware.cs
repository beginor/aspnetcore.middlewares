using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Beginor.AspNetCore.Middlewares.SpaFailback;

public class SpaFailbackMiddleware {

    private readonly RequestDelegate next;
    private readonly IWebHostEnvironment env;
    private readonly ILogger<SpaFailbackMiddleware> logger;
    private SpaFailbackOptions options;

    public SpaFailbackMiddleware(
        RequestDelegate next,
        IWebHostEnvironment env,
        ILogger<SpaFailbackMiddleware> logger,
        IOptionsMonitor<SpaFailbackOptions> monitor
    ) {
        this.next = next ?? throw new ArgumentNullException(nameof(next));
        this.env = env ?? throw new ArgumentNullException(nameof(env));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        options = monitor.CurrentValue ?? throw new ArgumentNullException(nameof(monitor));
        monitor.OnChange(newVal => {
            options = newVal;
        });
    }

    public Task InvokeAsync(HttpContext context) {
        var request = context.Request;
        var reqPath = request.Path.ToString();
        if (request.IsAjaxRequest()) {
            return next(context);
        }
        if (string.IsNullOrEmpty(reqPath)) {
            return next(context);
        }
        // if (string.IsNullOrEmpty(request.Headers.Referer.ToString())) {
        //     return next(context);
        // }

        var fileProvider = context.RequestServices.GetService<IFileProvider>();
        if (fileProvider == null) {
            fileProvider = env.WebRootFileProvider;
        }
        var fileInfo = fileProvider.GetFileInfo(reqPath);
        if (!fileInfo.Exists) {
            var failback = options.Rules.FirstOrDefault(
                f => f.PathBaseRegex != null && f.PathBaseRegex.IsMatch(reqPath)
            );
            if (failback != null) {
                request.Path = failback.Failback;
                logger.LogInformation($"SpaFailback: {reqPath} -> {failback.Failback}");
            }
        }
        return next(context);
    }
}
