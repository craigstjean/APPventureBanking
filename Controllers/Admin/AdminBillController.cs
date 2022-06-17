using APPventureBanking.Controllers.TransferObjects;
using APPventureBanking.Controllers.TransferObjects.Admin;
using APPventureBanking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APPventureBanking.Controllers.Admin;

[ApiController]
[Route("/Admin/Bill")]
public class AdminBillController : ControllerBase
{
    private readonly ILogger<AdminBillController> _logger;
    private readonly BankContext _context;
    
    public AdminBillController(ILogger<AdminBillController> logger, BankContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(BillResponse), 200)]
    public IActionResult Post([FromBody] CreateBillRequest request)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null : authorizationHeader[0].Split(" ")[1];
        
        if (identity != "Admin")
        {
            return Unauthorized();
        }
        
        var bill = new Bill
        {
            BillingPayeeId = request.BillingPayeeId,
            DueDate = DateOnly.FromDateTime(request.DueDate),
            AmountDue = request.AmountDue,
            IdentityId = request.IdentityId
        };
        
        _context.Bills.Add(bill);
        _context.SaveChanges();
        
        var billingPayee = _context.BillingPayees
            .Include(bp => bp.Party)
            .Include(bp => bp.BillingAddress)
            .FirstOrDefault(bp => bp.BillingPayeeId == request.BillingPayeeId);
        var response = new BillResponse
        {
            BillId = bill.BillId,
            BillingPayeeId = bill.BillingPayeeId,
            BillingPayee = new BillingPayeeResponse
            {
                BillingPayeeId = billingPayee!.BillingPayeeId,
                PartyId = billingPayee.PartyId,
                Party = billingPayee.Party,
                BillingAddressId = billingPayee.BillingAddressId,
                BillingAddress = billingPayee.BillingAddress
            },
            DueDate = bill.DueDate.ToDateTime(TimeOnly.MaxValue),
            AmountDue = bill.AmountDue
        };
        
        return CreatedAtAction("Post", new { id = bill.BillId });
    }
}
