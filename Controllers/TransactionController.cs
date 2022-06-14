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
    [ProducesResponseType(typeof(TransactionResponse), 200)]
    public IActionResult Post([FromBody] CreateTransactionRequest? request)
    {
        if (request == null)
        {
            return BadRequest();
        }
        
        var transaction = new Transaction
        {
            Amount = request.Amount,
            FromAccountId = request.FromAccountId,
            ToAccountId = request.ToAccountId,
            TransactionDateTime = DateTime.Now
        };

        _context.Transactions.Add(transaction);
        _context.SaveChanges();

        var response = new TransactionResponse
        {
            TransactionId = transaction.TransactionId,
            FromAccountId = transaction.FromAccountId,
            ToAccountId = transaction.ToAccountId,
            TransactionDateTime = transaction.TransactionDateTime,
            Amount = transaction.Amount,
            Balance = _accountService.GetBalanceAsOfTransaction(request.FromAccountId, transaction.TransactionId)
        };

        return CreatedAtRoute("Get", new { id = transaction.TransactionId }, response);
    }
}
