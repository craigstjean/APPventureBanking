using APPventureBanking.Models;

namespace APPventureBanking.Controllers.TransferObjects;

public class CreateAccountRequest
{
    public AccountType AccountType { get; set; }
    public string Name { get; set; }
}