using APPventureBanking.Models;

namespace APPventureBanking.Controllers.TransferObjects.Admin;

public class UpdatePartyRequest
{
    public PartyType Type { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EntityName { get; set; }
    public string PrimaryEmailAddress { get; set; }
    public string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string City { get; set; }
    public string StateCode { get; set; }
    public string PostalCode { get; set; }
}
