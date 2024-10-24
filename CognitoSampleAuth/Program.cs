using System.Security.Claims;
using CognitoSampleAuth;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.ConfigureOptions<JwtBearerConfigureOptions>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireMomUserType", policy =>
    {
        policy.RequireClaim("nona:userType", "Mom");
    })
    .AddPolicy("RequireGuideUserType", policy =>
    {
        policy.RequireClaim("nona:userType", "Guide");
    })
    .AddPolicy("RequireLCUserType", policy =>
    {
        policy.RequireClaim("nona:userType", "LC");
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapGet("/helloWorld", () => "Hello From An Open World")
    .WithName("HelloWorld")
    .WithOpenApi();

app.MapGet("/secureHelloWorld", () => "Secure Hello From A Locked World")
    .WithName("SecureHelloWorld")
    .WithOpenApi()
    .RequireAuthorization();

app.MapGet("/secureClaimedHelloWorld", (ClaimsPrincipal claims) => claims.Claims.Select(claim => new Claim(claim.Type, claim.Value)).ToArray())
    .WithName("SecureClaimedHelloWorld")
    .WithOpenApi()
    .RequireAuthorization();

app.MapGet("/secureMomHelloWorld", () => "Secure Hello From A Locked World To A Mom User")
    .WithName("SecureMomHelloWorld")
    .WithOpenApi()
    .RequireAuthorization("RequireMomUserType");

app.MapGet("/secureGuideHelloWorld", () => "Secure Hello From A Locked World To A Guide User")
    .WithName("SecureGuideHelloWorld")
    .WithOpenApi()
    .RequireAuthorization("RequireGuideUserType");

app.MapGet("/secureLCHelloWorld", () => "Secure Hello From A Locked World To A LC User")
    .WithName("SecureLCHelloWorld")
    .WithOpenApi()
    .RequireAuthorization("RequireLCUserType");

app.UseAuthentication();
app.UseAuthorization();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}