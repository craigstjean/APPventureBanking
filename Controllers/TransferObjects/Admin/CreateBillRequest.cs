namespace APPventureBanking.Controllers.TransferObjects.Admin;

public class CreateBillRequest
{
    public int BillingPayeeId { get; set; }
    
    public DateTime DueDate { get; set; }
    public decimal AmountDue { get; set; }

    public int IdentityId { get; set; }
}
