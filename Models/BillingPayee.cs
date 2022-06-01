namespace APPventureBanking.Models;

public class BillingPayee
{
    public int BillingPayeeId { get; set; }

    public int PartyId { get; set; }
    public Party Party { get; set; }

    public int BillingAddressId { get; set; }
    public Address BillingAddress { get; set; }

    public int ReferenceAccountId { get; set; }
    public Account ReferenceAccount { get; set; }
}
