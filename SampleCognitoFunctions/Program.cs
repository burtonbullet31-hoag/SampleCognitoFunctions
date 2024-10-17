using System.Text;
using Amazon;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SampleCognitoFunctions.Interfaces.Repositories;
using SampleCognitoFunctions.Repositories;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("COGNITO_USER_POOL_ID"),
                    ValidAudience = Environment.GetEnvironmentVariable("COGNITO_CLIENT_ID"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(Environment.GetEnvironmentVariable("COGNITO_USER_POOL_JWT_SIGNING_KEY") ??
                                  String.Empty))
                };
            });
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        //services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>(new AmazonDynamoDBConfig { RegionEndpoint = RegionEndpoint.USEast2 });
        services.AddSingleton<IUsersDynamoDbRepository, UserDynamoRepository>();
    })
    .Build();

host.Run();