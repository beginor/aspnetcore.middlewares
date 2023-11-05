using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

var webAppOpts = new WebApplicationOptions { };
var builder = WebApplication.CreateBuilder();

// Add services to the container.

builder.Services.ConfigureCustomHeader(builder.Configuration.GetSection("customHeader"));
var rootPath = builder.Environment.ContentRootPath;
var fileProvider = new CompositeFileProvider(
    new PhysicalFileProvider(Path.Combine(rootPath, "wwwroot"))
);
builder.Services.AddSingleton<IFileProvider>(fileProvider);

builder.Services.ConfigureSpaFailback(builder.Configuration.GetSection("spaFailback"));
if (builder.Environment.IsProduction()) {
    builder.Services.ConfigureGzipStatic();
}
builder.Services.AddControllers();

var app = builder.Build();
app.UsePathBase(new PathString("/test"));
app.UseCustomHeader();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
}
app.UseDefaultFiles();
if (app.Environment.IsProduction()) {
    app.UseGzipStatic();
}
app.UseSpaFailback();

var provider = app.Services.GetService<IFileProvider>();
if (provider == null) {
    app.UseStaticFiles();
}
else {
    app.UseStaticFiles(new StaticFileOptions { FileProvider = provider });
}
app.UseAuthorization();

app.MapControllers();

app.Run();
