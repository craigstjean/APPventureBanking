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
    private readonly CardService _cardService;

    public AccountController(ILogger<AccountController> logger, BankContext context, AccountService accountService, CardService cardService)
    {
        _logger = logger;
        _context = context;
        _accountService = accountService;
        _cardService = cardService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<AccountBalanceResponse>), 200)]
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
        var responses = accounts.Select(a => new AccountBalanceResponse
        {
            AccountId = a.AccountId,
            AccountNumber = a.AccountNumber,
            AccountType = a.AccountType,
            Name = a.Name,
            Balance = _accountService.GetBalance(a.AccountId)
        });

        return Ok(responses);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AccountBalanceResponse), 200)]
    public IActionResult Get(int id)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }
        
        var account = _context.Accounts.FirstOrDefault(a => a.AccountId == id && !a.IsDeleted && a.Identities.Contains(identity));
        if (account == null)
        {
            return NotFound();
        }
        
        var response = new AccountBalanceResponse
        {
            AccountId = account.AccountId,
            AccountNumber = account.AccountNumber,
            AccountType = account.AccountType,
            Name = account.Name,
            Balance = _accountService.GetBalance(account.AccountId)
        };
        
        return Ok(response);
    }
    
    [HttpGet("{id}/transactions")]
    [ProducesResponseType(typeof(TransactionsResponse), 200)]
    public IActionResult GetTransactions(int id, int maxRecords = 0, int pageNumber = 1)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }

        if (maxRecords == 0)
        {
            maxRecords = int.MaxValue;
        }

        var query =
            (from transaction in _context.Transactions
                where (transaction.FromAccountId == id || transaction.ToAccountId == id)
                select transaction);

        var totalTransactions = query.Count();
        
        var transactions = query
            .Skip((pageNumber - 1) * maxRecords)
            .Take(maxRecords)
            .ToList();

        var responses = transactions.Select(t => new TransactionResponse
        {
            TransactionId = t.TransactionId,
            FromAccountId = t.FromAccountId,
            ToAccountId = t.ToAccountId,
            TransactionDateTime = t.TransactionDateTime,
            Amount = t.Amount,
            Balance = 0 //TODO
        });

        var response = new TransactionsResponse
        {
            Transactions = responses.ToList(),
            TotalTransactions = totalTransactions
        };
       
        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(AccountBalanceResponse), 200)]
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
                    ExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
                    CardNumber = _cardService.NewCardNumber(),
                    SecurityCode = _cardService.NewSecurityCode()
                };

                _context.Cards.Add(card);
                _context.SaveChanges();
                break;
            case AccountType.Card:
                card = new Card
                {
                    CardType = CardType.CreditCard,
                    AccountId = account.AccountId,
                    ExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddYears(1)),
                    CardNumber = _cardService.NewCardNumber(),
                    SecurityCode = _cardService.NewSecurityCode()
                };

                _context.Cards.Add(card);
                _context.SaveChanges();
                break;
        }
        
        var response = new AccountBalanceResponse()
        {
            AccountId = account.AccountId,
            AccountNumber = account.AccountNumber,
            AccountType = account.AccountType,
            Name = account.Name,
            Balance = 0
        };
        
        return CreatedAtAction("Get", new { id = account.AccountId }, response);
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
        
        var accountToDelete = _context.Accounts.FirstOrDefault(a => a.AccountId == id && !a.IsDeleted && a.Identities.Contains(identity));
        if (accountToDelete == null)
        {
            return NotFound();
        }

        accountToDelete.IsDeleted = true;
        _context.SaveChanges();
        
        return NoContent();
    }
}
