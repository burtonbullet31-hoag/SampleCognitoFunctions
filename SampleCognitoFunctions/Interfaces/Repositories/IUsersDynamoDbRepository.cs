using SampleCognitoFunctions.Models;
using SampleCognitoFunctions.Models.ServiceResponse;

namespace SampleCognitoFunctions.Interfaces.Repositories;

public interface IUsersDynamoDbRepository: IDynamoDbRepository<UserModel>
{
    public Task<DynamoDbResponseModel<UserModel>> GetUserByLegacyIdAsync(string id);
    public Task<DynamoDbResponseModel<UserModel>> GetUserByAuthIdAsync(string id);
    public Task<DynamoDbResponseModel<UserModel>> GetUserByEmailAsync(string email);
}