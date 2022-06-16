namespace APPventureBanking.Models;

public class Transaction
{
    public int TransactionId { get; set; }

    public int FromAccountId { get; set; }
    public Account FromAccount { get; set; }

    public int ToAccountId { get; set; }
    public Account ToAccount { get; set; }

    public DateTime TransactionDateTime { get; set; }
    
    public decimal Amount { get; set; }
    public string Description { get; set; }
}
