using APPventureBanking.Controllers.TransferObjects;
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
    [ProducesResponseType(typeof(List<CardResponse>), 200)]
    public IActionResult Get()
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }

        var cards = _context.Cards.Where(c => c.Account.Identities.Contains(identity));
        var responses = cards.Select(c => new CardResponse
        {
            CardId = c.CardId,
            CardType = c.CardType,
            AccountId = c.AccountId,
            ExpirationDate = c.ExpirationDate
        });
        
        return Ok(responses);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CardResponse), 200)]
    public IActionResult Get(int id)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
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

        var response = new CardResponse
        {
            CardId = card.CardId,
            CardType = card.CardType,
            AccountId = card.AccountId,
            ExpirationDate = card.ExpirationDate
        };
        
        return Ok(response);
    }
}
