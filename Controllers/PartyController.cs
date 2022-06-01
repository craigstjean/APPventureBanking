using APPventureBanking.Models;
using Microsoft.AspNetCore.Mvc;

namespace APPventureBanking.Controllers;

[ApiController]
[Route("[controller]")]
public class PartyController : ControllerBase
{
    private readonly ILogger<PartyController> _logger;
    private readonly BankContext _context;

    public PartyController(ILogger<PartyController> logger, BankContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpGet]
    public IEnumerable<Party> Get()
    {
        return _context.Parties;
    }
    
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var party = _context.Parties.Find(id);
        if (party == null)
        {
            return NotFound();
        }
        return Ok(party);
    }
    
    [HttpPost]
    public IActionResult Post([FromBody] Party? party)
    {
        if (party == null)
        {
            return BadRequest();
        }
        
        _context.Parties.Add(party);
        _context.SaveChanges();
        
        return CreatedAtAction("Get", new { id = party.PartyId }, party);
    }
    
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Party party)
    {
        var partyToUpdate = _context.Parties.Find(id);
        if (partyToUpdate == null)
        {
            return NotFound();
        }
        partyToUpdate.Type = party.Type;
        partyToUpdate.EntityName = party.EntityName;
        partyToUpdate.FirstName = party.FirstName;
        partyToUpdate.LastName = party.LastName;
        partyToUpdate.PrimaryEmailAddress = party.PrimaryEmailAddress;
        partyToUpdate.MailingAddress = party.MailingAddress;
        _context.SaveChanges();
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var partyToDelete = _context.Parties.Find(id);
        if (partyToDelete == null)
        {
            return NotFound();
        }
        _context.Parties.Remove(partyToDelete);
        _context.SaveChanges();
        return NoContent();
    }
}