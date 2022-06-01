namespace APPventureBanking.Models;

public class Address
{
    public int AddressId { get; set; }
    public string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string City { get; set; }
    public string StateCode { get; set; }
    public string PostalCode { get; set; }
}
