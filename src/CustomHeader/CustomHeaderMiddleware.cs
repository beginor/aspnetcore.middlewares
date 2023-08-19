using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Beginor.AspNetCore.Middlewares.CustomHeader;

public class CustomHeaderMiddleware {
    private readonly RequestDelegate next;
    private IEnumerable<CustomHeaderRule> headerRules;

    public CustomHeaderMiddleware(
        RequestDelegate next,
        IOptionsMonitor<Dictionary<string, Dictionary<string, string>>> monitor
    ) {
        this.next = next ?? throw new ArgumentNullException(nameof(next));
        if (monitor == null) {
            throw new ArgumentNullException(nameof(monitor));
        }
        this.headerRules = BuildRules(monitor.Get(Consts.OptionsName));
        monitor.OnChange(newValue => {
            this.headerRules = BuildRules(monitor.Get(Consts.OptionsName));
        });
    }

    public Task InvokeAsync(HttpContext context) {
        context.Response.OnStarting(state => {
            var ctx = (HttpContext) state;
            var res = ctx.Response;
            // status is not success;
            if (res.StatusCode is < 200 or >= 300) {
                return Task.CompletedTask;
            }
            var reqPath = ctx.Request.Path.ToString();
            var rules = headerRules.Where(r => r.Pattern.IsMatch(reqPath));
            foreach (var rule in rules) {
                foreach (var pair in rule.Headers) {
                    if (string.IsNullOrEmpty(pair.Value) && res.Headers.ContainsKey(pair.Key)) {
                        res.Headers.Remove(pair.Key);
                    }
                    else {
                        res.Headers[pair.Key] = pair.Value;
                    }
                }
            }
            return Task.CompletedTask;
        }, context);
        return next(context);
    }

    private IEnumerable<CustomHeaderRule> BuildRules(Dictionary<string, Dictionary<string, string>> dict) {
        var keys = dict.Keys;
        return keys.Select(key => new CustomHeaderRule(key, dict[key]));
    }

}
