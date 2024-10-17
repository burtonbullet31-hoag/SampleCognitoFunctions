using SampleCognitoFunctions.Models;

namespace SampleCognitoFunctions.Interfaces.Services;

public interface IUserService
{
    Task CreateUserAsync(string username, string password);
    Task<UserModel> GetUserByUsernameAsync(string username);
    Task<bool> ValidateUserCredentialsAsync(string username, string password);
    Task DeleteUserAsync(string username);
}