using Microsoft.OpenApi.Models;
using PasswordManager.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
    x.SwaggerDoc("v1",
        new OpenApiInfo { Title = "Password Manager v2", Description = "Easily protect sensitive passwords with this API", Version = "v2" }
    ));
builder.Services.AddIoC();

var app = builder.Build();

#region App Configuration

app.UseHttpsRedirection(); 
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "Task tracker API"));
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
app.Run();

#endregion