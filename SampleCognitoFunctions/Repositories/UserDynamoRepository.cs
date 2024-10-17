using SampleCognitoFunctions.Interfaces.Repositories;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using SampleCognitoFunctions.Enums;
using SampleCognitoFunctions.Models;
using SampleCognitoFunctions.Models.ServiceResponse;

namespace SampleCognitoFunctions.Repositories;

public class UserDynamoRepository(IAmazonDynamoDB dynamoDbClient): IUsersDynamoDbRepository
{
    private readonly IAmazonDynamoDB _dynamoDbClient = dynamoDbClient;
    private readonly DynamoDBContext _context = new(dynamoDbClient);

    public async Task SaveAsync(UserModel user)
    {
        await _context.SaveAsync(user);
    }

    public async Task<UserModel> GetByIdAsync(string id)
    {
        return await _context.LoadAsync<UserModel>(id);
    }

    public async Task DeleteByIdAsync(string id)
    {
        await _context.DeleteAsync<UserModel>(id);
    }

    public async Task<IEnumerable<UserModel>> GetAllAsync()
    {
        var conditions = new List<ScanCondition>();
        return await _context.ScanAsync<UserModel>(conditions).GetRemainingAsync();
    }


    public async Task<DynamoDbResponseModel<IEnumerable<UserModel>?>> GetAllItemsAsync() => await GetUsers();

    public Task<DynamoDbResponseModel<UserModel>> GetItemAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<DynamoDbResponseModel<UserModel>> GetUserByLegacyIdAsync(string id)
    {
        var conditions = new List<ScanCondition>
        {
            new ScanCondition(nameof(UserModel.LegacyAuthId), ScanOperator.Equal, id),
        };
        
        var response = await GetUsers(conditions);

        return response.Status == ServiceResponseStatus.Error ?
            new DynamoDbResponseModel<UserModel>(null, response.Status, new DynamoDbResponseModel<UserModel>.ErrorResponse(response.Error!.InnerException)) :
            new DynamoDbResponseModel<UserModel>(response.Data?.First(), ServiceResponseStatus.Ok, null);
    }

    public async Task<DynamoDbResponseModel<UserModel>> GetUserByAuthIdAsync(string id)
    {
        var conditions = new List<ScanCondition>
        {
            new ScanCondition(nameof(UserModel.AuthUserId), ScanOperator.Equal, id),
        };
        
        var response = await GetUsers(conditions);

        return response.Status == ServiceResponseStatus.Error ?
            new DynamoDbResponseModel<UserModel>(null, response.Status, new DynamoDbResponseModel<UserModel>.ErrorResponse(response.Error!.InnerException)) :
            new DynamoDbResponseModel<UserModel>(response.Data?.First(), ServiceResponseStatus.Ok, null);
    }

    public async Task<DynamoDbResponseModel<UserModel>> GetUserByEmailAsync(string email)
    {
        var conditions = new List<ScanCondition>
        {
            new ScanCondition("EmailList.data", ScanOperator.Contains, email)
        };

        var response = await GetUsers(conditions);

        return response.Status == ServiceResponseStatus.Error ?
            new DynamoDbResponseModel<UserModel>(null, response.Status, new DynamoDbResponseModel<UserModel>.ErrorResponse(response.Error!.InnerException)) :
            new DynamoDbResponseModel<UserModel>(response.Data?.FirstOrDefault(), ServiceResponseStatus.Ok, null);
    }
    public async Task<DynamoDbResponseModel<UserModel>> AddItemAsync(UserModel item)
    {
        var response = await SaveData(item);
        return response.Data ?
            new DynamoDbResponseModel<UserModel>(item, ServiceResponseStatus.Ok, null) :
            new DynamoDbResponseModel<UserModel>(item, ServiceResponseStatus.Error, new DynamoDbResponseModel<UserModel>.ErrorResponse(response.Error!.InnerException));    
            
    }

    public async Task<DynamoDbResponseModel<bool>> UpdateItemAsync(UserModel item)
    {
        return await SaveData(item);
    }

    public async Task<DynamoDbResponseModel<bool>> DeleteItemAsync(string id)
    {
        try{
            await _context.DeleteAsync(id);
            return new DynamoDbResponseModel<bool>(true, ServiceResponseStatus.Ok, null);
        }
        catch (Exception ex)
        {
            return new DynamoDbResponseModel<bool>(false, ServiceResponseStatus.Error, new DynamoDbResponseModel<bool>.ErrorResponse(ex));
        }
    }

    private async Task<DynamoDbResponseModel<bool>> SaveData<TData>(TData data)
    {
        try
        {
            await _context.SaveAsync(data);
            return new DynamoDbResponseModel<bool>(true, ServiceResponseStatus.Ok, null);
        }
        catch (Exception ex)
        {
            return new DynamoDbResponseModel<bool>(false, ServiceResponseStatus.Error, new DynamoDbResponseModel<bool>.ErrorResponse(ex));
        }
    }
    
    private async Task<DynamoDbResponseModel<IEnumerable<UserModel>?>> GetUsers(IEnumerable<ScanCondition>? conditions = null)
    {
        try
        {
            var d = await _context.ScanAsync<UserModel>(conditions ?? Array.Empty<ScanCondition>()).GetRemainingAsync();
            return new DynamoDbResponseModel<IEnumerable<UserModel>?>(d.ToList(), ServiceResponseStatus.Ok, null);
        }
        catch (Exception ex)
        {
            return new DynamoDbResponseModel<IEnumerable<UserModel>?>(
                null,
                ServiceResponseStatus.Error,
                new DynamoDbResponseModel<IEnumerable<UserModel>?>.ErrorResponse(ex));
        }
    }
}