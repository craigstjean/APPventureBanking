using APPventureBanking.Controllers.TransferObjects;
using APPventureBanking.Models;
using APPventureBanking.Services;
using Microsoft.AspNetCore.Mvc;

namespace APPventureBanking.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly BankContext _context;
    private readonly AccountService _accountService;

    public AccountController(ILogger<AccountController> logger, BankContext context, AccountService accountService)
    {
        _logger = logger;
        _context = context;
        _accountService = accountService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<AccountBalanceDTO>), 200)]
    public IActionResult Get()
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }

        var accounts = _context.Accounts.Where(a => !a.IsDeleted && a.Identities.Contains(identity));
        var results = accounts.Select(a => new AccountBalanceDTO
        {
            AccountId = a.AccountId,
            AccountNumber = a.AccountNumber,
            AccountType = a.AccountType,
            Name = a.Name,
            Balance = _accountService.GetBalance(a.AccountId)
        });

        return Ok(results);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AccountBalanceDTO), 200)]
    public IActionResult Get(int id)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
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
        
        var result = new AccountBalanceDTO
        {
            AccountId = account.AccountId,
            AccountNumber = account.AccountNumber,
            AccountType = account.AccountType,
            Name = account.Name,
            Balance = _accountService.GetBalance(account.AccountId)
        };
        
        return Ok(result);
    }
    
    [HttpGet("{id}/transactions")]
    public IActionResult GetTransactions(int id)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
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
    public IActionResult Post([FromBody] CreateAccountRequest? request)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }
        
        if (request == null)
        {
            return BadRequest();
        }

        var account = new Account
        {
            AccountType = request.AccountType,
            Name = request.Name
        };
            
        account.Identities.Add(identity);
        account.AccountNumber = _accountService.NextAccountNumber();
        account.IsDeleted = false;
        _context.Accounts.Add(account);
        _context.SaveChanges();

        Card? card;
        switch (account.AccountType)
        {
            case AccountType.Checking:
                card = new Card
                {
                    CardType = CardType.DebitCard,
                    AccountId = account.AccountId,
                    ExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddYears(1))
                };

                _context.Cards.Add(card);
                _context.SaveChanges();
                break;
            case AccountType.Card:
                card = new Card
                {
                    CardType = CardType.CreditCard,
                    AccountId = account.AccountId,
                    ExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddYears(1))
                };

                _context.Cards.Add(card);
                _context.SaveChanges();
                break;
        }
        
        return CreatedAtAction("Get", new { id = account.AccountId }, account);
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
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
