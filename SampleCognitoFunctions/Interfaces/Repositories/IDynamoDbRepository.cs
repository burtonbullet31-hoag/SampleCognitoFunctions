using SampleCognitoFunctions.Models;
using SampleCognitoFunctions.Models.ServiceResponse;

namespace SampleCognitoFunctions.Interfaces.Repositories;

public interface IDynamoDbRepository<TData>
{
    Task<DynamoDbResponseModel<IEnumerable<UserModel>?>> GetAllItemsAsync();
    Task<DynamoDbResponseModel<TData>> GetItemAsync(string id);
    Task<DynamoDbResponseModel<TData>> AddItemAsync(TData item);
    Task<DynamoDbResponseModel<bool>> UpdateItemAsync(TData item);
    Task<DynamoDbResponseModel<bool>> DeleteItemAsync(string id);
}