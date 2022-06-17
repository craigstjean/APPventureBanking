using APPventureBanking.Controllers.TransferObjects;
using APPventureBanking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APPventureBanking.Controllers;

[ApiController]
[Route("[controller]")]
public class BillController : ControllerBase
{
    private readonly ILogger<BillController> _logger;
    private readonly BankContext _context;

    public BillController(ILogger<BillController> logger, BankContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<BillResponse>), 200)]
    public IActionResult Get()
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }

        var bills = _context.Bills
            .Include(b => b.BillingPayee)
            .Include(b => b.BillingPayee.Party)
            .Include(b => b.BillingPayee.BillingAddress)
            .Where(b => b.IdentityId == identity.IdentityId).ToList();
        var responses = bills.Select(b => new BillResponse
        {
            BillId = b.BillId,
            BillingPayeeId = b.BillingPayeeId,
            BillingPayee = new BillingPayeeResponse
            {
                BillingPayeeId = b.BillingPayee.BillingPayeeId,
                PartyId = b.BillingPayee.PartyId,
                Party = b.BillingPayee.Party,
                BillingAddressId = b.BillingPayee.BillingAddressId,
                BillingAddress = b.BillingPayee.BillingAddress
            },
            DueDate = b.DueDate.ToDateTime(TimeOnly.MaxValue),
            AmountDue = b.AmountDue
        });
        
        return Ok(responses);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BillResponse), 200)]
    public IActionResult Get(int id)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }
        
        var bill = _context.Bills.Find(id);
        if (bill == null)
        {
            return NotFound();
        }
        
        if (bill.IdentityId != identity.IdentityId)
        {
            return Unauthorized();
        }
        
        var response = new BillResponse
        {
            BillId = bill.BillId,
            BillingPayeeId = bill.BillingPayeeId,
            BillingPayee = new BillingPayeeResponse
            {
                BillingPayeeId = bill.BillingPayee.BillingPayeeId,
                PartyId = bill.BillingPayee.PartyId,
                Party = bill.BillingPayee.Party,
                BillingAddressId = bill.BillingPayee.BillingAddressId,
                BillingAddress = bill.BillingPayee.BillingAddress
            },
            DueDate = bill.DueDate.ToDateTime(TimeOnly.MaxValue),
            AmountDue = bill.AmountDue
        };
        
        return Ok(response);
    }
}
