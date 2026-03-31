using YoutubeClone.WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCore(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseMiddleware<ErrorHandleMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
