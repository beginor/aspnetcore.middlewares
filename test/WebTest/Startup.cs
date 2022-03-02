using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebTest;

public class Startup {

    public Startup(IConfiguration configuration) {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
        services.ConfigureCustomHeader(Configuration.GetSection("customHeader"));
        services.ConfigureSpaFailback(Configuration.GetSection("spaFailback"));
        services.ConfigureGzipStatic();
        services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        app.UsePathBase(new PathString("/webtest"));
        app.UseCustomHeader();
        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }
        app.UseDefaultFiles();
        app.UseGzipStatic();
        app.UseSpaFailback();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
        });
    }

}
