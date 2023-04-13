using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureCustomHeader(builder.Configuration.GetSection("customHeader"));
builder.Services.ConfigureSpaFailback(builder.Configuration.GetSection("spaFailback"));
builder.Services.ConfigureGzipStatic();
builder.Services.AddControllers();

var app = builder.Build();
app.UsePathBase(new PathString("/webtest"));
app.UseCustomHeader();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
}
app.UseDefaultFiles();
app.UseGzipStatic();
app.UseSpaFailback();
var rootPath = app.Environment.ContentRootPath;
app.UseStaticFiles(
    new StaticFileOptions {
        FileProvider = new CompositeFileProvider(
            new PhysicalFileProvider(Path.Combine(rootPath, "wwwroot")),
            new PhysicalFileProvider(Path.Combine(rootPath, "../../../../javascript"))
        )
    }
);

app.UseAuthorization();

app.MapControllers();

app.Run();
