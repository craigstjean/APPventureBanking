namespace APPventureBanking.Controllers.TransferObjects;

public class CreateTransactionResponse
{
    public int TransactionId { get; set; }

    public decimal Balance { get; set; }

    public bool NameMatch { get; set; } = true;
    public bool Success { get; set; }
}
