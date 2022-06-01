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
    public IActionResult Get()
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }

        var billingPayees = (from billingPayee in _context.BillingPayees
            join account in _context.Accounts on billingPayee.ReferenceAccountId equals account.AccountId
            where account.Identities.Contains(identity)
            select billingPayee).ToList();
        
        return Ok(billingPayees);
    }
    
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
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
        
        return Ok(billingPayee);
    }
    
    [HttpPost]
    public IActionResult Post([FromBody] BillingPayee? billingPayee)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }
        
        if (billingPayee == null)
        {
            return BadRequest();
        }
        
        if (!billingPayee.ReferenceAccount.Identities.Contains(identity))
        {
            return Unauthorized();
        }
        
        _context.BillingPayees.Add(billingPayee);
        _context.SaveChanges();
        
        return CreatedAtAction("Get", new { id = billingPayee.BillingPayeeId }, billingPayee);
    }
    
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] BillingPayee? billingPayee)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }
        
        if (billingPayee == null)
        {
            return BadRequest();
        }
        
        if (!billingPayee.ReferenceAccount.Identities.Contains(identity))
        {
            return Unauthorized();
        }
        
        var billingPayeeToUpdate = _context.BillingPayees.Find(id);
        if (billingPayeeToUpdate == null)
        {
            return NotFound();
        }
        billingPayeeToUpdate.PartyId = billingPayee.PartyId;
        billingPayeeToUpdate.ReferenceAccountId = billingPayee.ReferenceAccountId;
        billingPayeeToUpdate.BillingAddressId = billingPayee.BillingAddressId;
        _context.BillingPayees.Update(billingPayeeToUpdate);
        _context.SaveChanges();
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
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
        
        _context.BillingPayees.Remove(billingPayee);
        _context.SaveChanges();
        return NoContent();
    }
}
