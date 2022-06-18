namespace APPventureBanking.Controllers.TransferObjects;

public class PayBillRequest
{
    public int BillId { get; set; }
    public int FromAccountId { get; set; }
    public decimal Amount { get; set; }
}
