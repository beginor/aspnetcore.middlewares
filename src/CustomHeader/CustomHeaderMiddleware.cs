using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Beginor.AspNetCore.Middlewares.CustomHeader; 

public class CustomHeaderMiddleware {
    private readonly RequestDelegate next;
    private CustomHeaderOptions options;

    public CustomHeaderMiddleware(
        RequestDelegate next,
        IOptionsMonitor<CustomHeaderOptions> monitor
    ) {
        this.next = next ?? throw new ArgumentNullException(nameof(next));
        this.options = monitor.CurrentValue ?? throw new ArgumentNullException(nameof(monitor));
        monitor.OnChange(newValue => options = newValue);
    }

    public Task InvokeAsync(HttpContext context) {
        context.Response.OnStarting(state => {
            var ctx = (HttpContext) state;
            var res = ctx.Response;
            foreach (var pair in options.Headers) {
                if (string.IsNullOrEmpty(pair.Value) && res.Headers.ContainsKey(pair.Key)) {
                    res.Headers.Remove(pair.Key);
                }
                else {
                    res.Headers[pair.Key] = pair.Value;
                }
            }
            return Task.CompletedTask;
        }, context);
        return next(context);
    }

}