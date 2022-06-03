using APPventureBanking.Controllers.TransferObjects;
using APPventureBanking.Models;
using Microsoft.AspNetCore.Mvc;

namespace APPventureBanking.Controllers;

[ApiController]
[Route("[controller]")]
public class BillingPayeeController : ControllerBase
{
    private readonly ILogger<BillingPayeeController> _logger;
    private readonly BankContext _context;

    public BillingPayeeController(ILogger<BillingPayeeController> logger, BankContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<BillingPayeeResponse>), 200)]
    public IActionResult Get()
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }

        var billingPayees = (from billingPayee in _context.BillingPayees
            join account in _context.Accounts on billingPayee.ReferenceAccountId equals account.AccountId
            where account.Identities.Contains(identity)
            select billingPayee).ToList();

        var responses = billingPayees.Select(b => new BillingPayeeResponse
        {
            BillingPayeeId = b.BillingPayeeId,
            PartyId = b.PartyId,
            Party = b.Party,
            BillingAddressId = b.BillingAddressId,
            BillingAddress = b.BillingAddress
        });
        
        return Ok(responses);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BillingPayeeResponse), 200)]
    public IActionResult Get(int id)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }

        var billingPayee = _context.BillingPayees.Find(id);
        if (billingPayee == null)
        {
            return NotFound();
        }
        
        if (!billingPayee.ReferenceAccount.Identities.Contains(identity))
        {
            return Unauthorized();
        }

        var response = new BillingPayeeResponse
        {
            BillingPayeeId = billingPayee.BillingPayeeId,
            PartyId = billingPayee.PartyId,
            Party = billingPayee.Party,
            BillingAddressId = billingPayee.BillingAddressId,
            BillingAddress = billingPayee.BillingAddress
        };
        
        return Ok(response);
    }
}
