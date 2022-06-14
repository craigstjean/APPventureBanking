namespace APPventureBanking.Controllers.TransferObjects;

public class TransactionsResponse
{
    public List<TransactionResponse> Transactions { get; set; } = new();
    public int TotalTransactions { get; set; }
}
