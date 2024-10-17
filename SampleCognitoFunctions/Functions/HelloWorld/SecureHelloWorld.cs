using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SampleCognitoFunctions.Functions.HelloWorld;

public class SecureHelloWorld
{
    private readonly ILogger<SecureHelloWorld> _logger;

    public SecureHelloWorld(ILogger<SecureHelloWorld> logger)
    {
        _logger = logger;
    }

    [Function("SecureHelloWorld")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
        
    }

}