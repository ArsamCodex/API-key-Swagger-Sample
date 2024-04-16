using authSwagger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(/*x=>x.Filters.Add<ApiKeyAuthFilter>()*/);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Autorise inline config call from swagger whit key
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Enter key",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Name = "x-api-key",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Scheme = "ApiKeyScheme"

    });
    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header,
    };
    var requirment = new OpenApiSecurityRequirement
{
    {scheme , new List<string>() }
};
    c.AddSecurityRequirement(requirment);
});
builder.Services.AddScoped<ApiKeyAuthFilter>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseMiddleware<ApiKeyAuthMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
