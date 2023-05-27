using webapi.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            //Allow the cors for the react frontend endpoint
            builder.WithOrigins("https://localhost:3000")
                .AllowAnyHeader()
                .WithMethods("GET", "POST")
                .AllowCredentials();
        });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors();

app.MapHub<AsyncExtractorHub>("/asyncExtractorHub");

app.Run();
