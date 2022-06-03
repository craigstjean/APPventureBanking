using APPventureBanking.Models;

namespace APPventureBanking.Controllers.TransferObjects;

public class BillResponse
{
    public int BillId { get; set; }

    public int BillingPayeeId { get; set; }
    public BillingPayee BillingPayee { get; set; }
    
    public DateOnly DueDate { get; set; }
    public decimal AmountDue { get; set; }
}