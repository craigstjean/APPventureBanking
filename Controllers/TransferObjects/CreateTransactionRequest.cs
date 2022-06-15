namespace APPventureBanking.Controllers.TransferObjects;

public class CreateTransactionRequest
{
    public int FromAccountId { get; set; }
    public int ToAccountNumber { get; set; }
    public string ToAccountName { get; set; }

    public decimal Amount { get; set; }

    public bool IgnoreNameMismatch { get; set; }
}
