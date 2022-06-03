using APPventureBanking.Models;

namespace APPventureBanking.Controllers.TransferObjects;

public class BillingPayeeResponse
{
    public int BillingPayeeId { get; set; }

    public int PartyId { get; set; }
    public Party Party { get; set; }

    public int BillingAddressId { get; set; }
    public Address BillingAddress { get; set; }
}