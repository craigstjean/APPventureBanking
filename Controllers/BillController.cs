using APPventureBanking.Models;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult Get()
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }
        
        return Ok(_context.Bills.Where(b => b != null && b.IdentityId == identity.IdentityId).ToList());
    }
    
    [HttpGet("{id}")]
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
        
        return Ok(bill);
    }
    
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Bill bill)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }
        
        var billToUpdate = _context.Bills.Find(id);
        if (billToUpdate == null)
        {
            return NotFound();
        }
        
        if (billToUpdate.IdentityId != identity.IdentityId)
        {
            return Unauthorized();
        }
        
        billToUpdate.AssociatedTransactions = bill.AssociatedTransactions;
        _context.SaveChanges();
        
        return NoContent();
    }
}
