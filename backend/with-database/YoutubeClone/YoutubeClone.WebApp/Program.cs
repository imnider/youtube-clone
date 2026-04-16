using Serilog;
using YoutubeClone.WebApp.Extensions;
using YoutubeClone.WebApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddCore(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ErrorHandleMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();
