using APPventureBanking.Models;
using Microsoft.AspNetCore.Mvc;

namespace APPventureBanking.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly BankContext _context;

    public AccountController(ILogger<AccountController> logger, BankContext context)
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
        
        return Ok(_context.Accounts.Where(a => a != null && !a.IsDeleted && a.Identities.Contains(identity)));
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
        
        var account = _context.Accounts.Find(id);
        if (account == null || account.IsDeleted)
        {
            return NotFound();
        }
        
        if (!account.Identities.Contains(identity))
        {
            return Unauthorized();
        }
        
        return Ok(account);
    }
    
    [HttpGet("{id}/transactions")]
    public IActionResult GetTransactions(int id)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }
        
        var transactions =
            (from transaction in _context.Transactions
            where (transaction.FromAccountId == id || transaction.ToAccountId == id)
            select transaction).ToList();
       
        return Ok(transactions);
    }
    
    [HttpPost]
    public IActionResult Post([FromBody] Account? account)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }
        
        if (account == null)
        {
            return BadRequest();
        }
        
        account.Identities.Clear();
        account.Identities.Add(identity);
        _context.Accounts.Add(account);
        _context.SaveChanges();
        
        return CreatedAtAction("Get", new { id = account.AccountId }, account);
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
        
        var accountToDelete = _context.Accounts.Find(id);
        if (accountToDelete == null)
        {
            return NotFound();
        }

        if (!accountToDelete.Identities.Contains(identity))
        {
            return Unauthorized();
        }
        
        accountToDelete.IsDeleted = true;
        _context.SaveChanges();
        
        return NoContent();
    }
}
