using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
    private IEnumerable<SpaFailbackRule> failbackRules;

    public SpaFailbackMiddleware(
        RequestDelegate next,
        IWebHostEnvironment env,
        ILogger<SpaFailbackMiddleware> logger,
        IOptionsMonitor<Dictionary<string, string>> monitor
    ) {
        this.next = next ?? throw new ArgumentNullException(nameof(next));
        this.env = env ?? throw new ArgumentNullException(nameof(env));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.failbackRules = BuildFailbackRules(monitor.Get(Consts.OptionsName));
        monitor.OnChange(_ => {
            this.failbackRules = BuildFailbackRules(monitor.Get(Consts.OptionsName));
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
        var fileProvider = context.RequestServices.GetService<IFileProvider>()
            ?? env.WebRootFileProvider;
        var fileInfo = fileProvider.GetFileInfo(reqPath);
        if (fileInfo.Exists) {
            return next(context);
        }
        var failbackRule = failbackRules.FirstOrDefault(
            f => f.Pattern.IsMatch(reqPath)
        );
        if (failbackRule == null) {
            return next(context);
        }
        request.Path = failbackRule.Failback;
        logger.LogInformation($"SpaFailback: {reqPath} -> {failbackRule.Failback}");
        return next(context);
    }

    private IEnumerable<SpaFailbackRule> BuildFailbackRules(Dictionary<string, string> dict) {
        var keys = dict.Keys;
        return keys.Select(key => new SpaFailbackRule(key, dict[key]));
    }
}
