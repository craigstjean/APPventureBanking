namespace APPventureBanking.Models;

public enum AccountType
{
    Checking,
    Savings,
    Card
}

public class Account
{
    public int AccountId { get; set; }
    public int AccountNumber { get; set; }
    public AccountType AccountType { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; } = false;

    public ICollection<Identity> Identities { get; set; } = new List<Identity>();
}
