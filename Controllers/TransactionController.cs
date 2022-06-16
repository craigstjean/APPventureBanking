using APPventureBanking.Controllers.TransferObjects;
using APPventureBanking.Models;
using APPventureBanking.Services;
using Microsoft.AspNetCore.Mvc;

namespace APPventureBanking.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ILogger<TransactionController> _logger;
    private readonly BankContext _context;
    private readonly AccountService _accountService;

    public TransactionController(ILogger<TransactionController> logger, BankContext context, AccountService accountService)
    {
        _logger = logger;
        _context = context;
        _accountService = accountService;
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(CreateTransactionResponse), 200)]
    public IActionResult Post([FromBody] CreateTransactionRequest? request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        if (request.Amount <= 0)
        {
            return Ok(new CreateTransactionResponse
            {
                Success = false
            });
        }

        var toAccount =
            _context.Accounts.FirstOrDefault(a => a.AccountNumber == request.ToAccountNumber && !a.IsDeleted);
        if (toAccount == null)
        {
            return Ok(new CreateTransactionResponse
            {
                Success = false
            });
        }

        var partyNames = (from p in _context.Parties
            join i in _context.Identities on p.PartyId equals i.PartyId
            where i.Accounts.Contains(toAccount)
            select p.DisplayName.ToLower()).ToList();

        if (!request.IgnoreNameMismatch && !partyNames.Contains(request.ToAccountName.ToLower()))
        {
            return Ok(new CreateTransactionResponse
            {
                NameMatch = false,
                Success = false
            });
        }
        
        var transaction = new Transaction
        {
            Amount = request.Amount,
            FromAccountId = request.FromAccountId,
            ToAccountId = toAccount.AccountId,
            TransactionDateTime = DateTime.Now,
            Description = request.Description
        };

        _context.Transactions.Add(transaction);
        _context.SaveChanges();

        var response = new CreateTransactionResponse
        {
            TransactionId = transaction.TransactionId,
            Balance = _accountService.GetBalanceAsOfTransaction(request.FromAccountId, transaction.TransactionId),
            NameMatch = true,
            Success = true
        };
        
        return CreatedAtAction("Post", new { id = transaction.TransactionId }, response);
    }
}
