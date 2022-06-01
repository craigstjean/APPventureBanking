namespace APPventureBanking.Models;

public class Identity
{
    public int IdentityId { get; set; }
    public int PartyId { get; set; }
    public Party Party { get; set; }

    public ICollection<Account> Accounts { get; set; }
}
