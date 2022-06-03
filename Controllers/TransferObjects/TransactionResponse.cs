namespace APPventureBanking.Controllers.TransferObjects;

public class TransactionResponse
{
    public int TransactionId { get; set; }

    public int FromAccountId { get; set; }
    public int ToAccountId { get; set; }

    public DateTime TransactionDateTime { get; set; }
    
    public decimal Amount { get; set; }
}
