using Amazon.DynamoDBv2.DataModel;

namespace SampleCognitoFunctions.Models;

[DynamoDBTable("Users")]
public class UserModel
{
    [DynamoDBHashKey]
    public int Id { get; set; }
    public string LegacyAuthId {get; set; }
    public string AuthUserId {get; set; }
    public string DeviceToken { get; set; }
    public string AvatarUriPath { get; set; }
    public DateOnly Birthdate { get; set; }
    public IEnumerable<EmailModel> EmailList { get; set; }
    public IEnumerable<PhoneModel> PhoneList { get; set; }
    public NameModel Name  { get; set; }
}
