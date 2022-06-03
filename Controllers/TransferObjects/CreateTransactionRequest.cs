namespace APPventureBanking.Controllers.TransferObjects;

public class CreateTransactionRequest
{
    public int FromAccountId { get; set; }
    public int ToAccountId { get; set; }

    public decimal Amount { get; set; }
}
