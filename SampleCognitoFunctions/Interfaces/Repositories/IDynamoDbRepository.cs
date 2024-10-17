namespace SampleCognitoFunctions.Interfaces.Repositories;

public interface IDynamoDbRepository<T>
{
    Task<IEnumerable<T>> GetAllItemsAsync();
    Task<T> GetItemAsync(string id);
    Task AddItemAsync(T item);
    Task UpdateItemAsync(T item);
    Task DeleteItemAsync(string id);
}