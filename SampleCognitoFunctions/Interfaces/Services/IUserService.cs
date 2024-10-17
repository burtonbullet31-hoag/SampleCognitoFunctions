using SampleCognitoFunctions.Models;

namespace SampleCognitoFunctions.Interfaces.Services;

using System.Threading.Tasks;

public interface IUserService
{
    Task CreateUserAsync(string username, string password);
    Task<UserModel> GetUserByUsernameAsync(string username);
    Task<bool> ValidateUserCredentialsAsync(string username, string password);
    Task DeleteUserAsync(string username);
}
