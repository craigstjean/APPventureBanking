using APPventureBanking.Models;
using Microsoft.AspNetCore.Mvc;

namespace APPventureBanking.Controllers;

[ApiController]
[Route("[controller]")]
public class CardController : ControllerBase
{
    private readonly ILogger<CardController> _logger;
    private readonly BankContext _context;

    public CardController(ILogger<CardController> logger, BankContext context)
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

        return Ok(_context.Cards.Where(c => c.Account.Identities.Contains(identity)));
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
        
        var card = _context.Cards.Find(id);
        if (card == null)
        {
            return NotFound();
        }
        
        if (!card.Account.Identities.Contains(identity))
        {
            return Unauthorized();
        }
        
        return Ok(card);
    }
}
