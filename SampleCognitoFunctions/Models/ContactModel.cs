using SampleCognitoFunctions.Enums;

namespace SampleCognitoFunctions.Models;

public abstract class ContactModel(ContactMethodEnum method)
{
    public ContactMethodEnum Method { get; private set; } = method;
    public ContactTypeEnum Type { get; set; }
    public required string Data { get; set; }
    public required string Id { get; set; }
}

public class EmailModel() : ContactModel(ContactMethodEnum.Email);

public class PhoneModel() : ContactModel(ContactMethodEnum.Phone);