namespace APPventureBanking.Models;

public enum TransactionType
{
    Credit,
    Debit
}

public class Transaction
{
    public int TransactionId { get; set; }

    public int FromAccountId { get; set; }
    public Account FromAccount { get; set; }

    public int ToAccountId { get; set; }
    public Account ToAccount { get; set; }

    public TransactionType TransactionType { get; set; }
    public DateTime TransactionDateTime { get; set; }
    
    public int Dollars { get; set; }
    public int Cents { get; set; }
}
