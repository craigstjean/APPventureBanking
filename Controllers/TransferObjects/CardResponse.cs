using APPventureBanking.Models;

namespace APPventureBanking.Controllers.TransferObjects;

public class CardResponse
{
    public int CardId { get; set; }

    public CardType CardType { get; set; }

    public int AccountId { get; set; }

    public DateOnly ExpirationDate { get; set; }
}
