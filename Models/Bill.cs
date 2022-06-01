namespace APPventureBanking.Models;

public class Bill
{
    public int BillId { get; set; }

    public int BillingPayeeId { get; set; }
    public BillingPayee BillingPayee { get; set; }
    
    public DateOnly DueDate { get; set; }
    public int DollarsDue { get; set; }
    public int CentsDue { get; set; }

    public int IdentityId { get; set; }
    public Identity Identity { get; set; }

    public List<Transaction> AssociatedTransactions { get; set; }
}
