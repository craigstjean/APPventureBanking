namespace APPventureBanking.Controllers.TransferObjects.Admin;

public class CreateAddressRequest
{
    public string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string City { get; set; }
    public string StateCode { get; set; }
    public string PostalCode { get; set; }
}
