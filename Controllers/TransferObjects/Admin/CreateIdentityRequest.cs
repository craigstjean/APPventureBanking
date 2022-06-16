using APPventureBanking.Models;

namespace APPventureBanking.Controllers.TransferObjects.Admin;

public class CreateIdentityRequest
{
    public PartyType Type { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EntityName { get; set; }
    public string PrimaryEmailAddress { get; set; }
    public CreateAddressRequest MailingAddress { get; set; }
}
