namespace APPventureBanking.Models;

public enum CardType
{
    CreditCard,
    DebitCard
}

public class Card
{
    public int CardId { get; set; }

    public CardType CardType { get; set; }

    public int AccountId { get; set; }
    public Account Account { get; set; }

    public DateOnly ExpirationDate { get; set; }
}
