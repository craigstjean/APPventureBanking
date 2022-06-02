using APPventureBanking.Models;

namespace APPventureBanking.Controllers.TransferObjects;

public class AccountBalanceDTO
{
    public int AccountId { get; set; }
    public int AccountNumber { get; set; }
    public AccountType AccountType { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
}
