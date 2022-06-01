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
    public IActionResult Post([FromBody] Transaction? transaction)
    {
        if (transaction == null)
        {
            return BadRequest();
        }

        _context.Transactions.Add(transaction);
        _context.SaveChanges();

        return CreatedAtRoute("Get", new { id = transaction.TransactionId }, transaction);
    }
}
