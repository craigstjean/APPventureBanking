using APPventureBanking.Controllers.TransferObjects;
using APPventureBanking.Models;
using Microsoft.AspNetCore.Mvc;

namespace APPventureBanking.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ILogger<TransactionController> _logger;
    private readonly BankContext _context;

    public TransactionController(ILogger<TransactionController> logger, BankContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpPost]
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

        return CreatedAtRoute("Get", new { id = transaction.TransactionId }, request);
    }
}
