using BookServer.Controllers;
using BookServer.Extensions;
using BookServer.Services;
using Shared.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = "XSRF-TOKEN";
    options.SuppressXFrameOptionsHeader = true;
    options.HeaderName = "X-XSRF-TOKEN";
});

builder.Services.Register();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSignalR();

var app = builder.Build();
app.UseCors();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    options.InjectJavascript("/swagger/custom-swagger.js");
});

app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapHub<TableHub>(HubConstants.TableHubUrl);

app.UseStaticFiles();
app.MapBookEndpoints();

app.UseWhen(
    context => !context.Request.Path.StartsWithSegments("/swagger"),
    subApp => subApp.UseAntiforgery()
);

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DataSeeder.Seed(dbContext);
}

app.Run();
