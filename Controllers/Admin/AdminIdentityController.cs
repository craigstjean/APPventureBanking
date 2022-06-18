using APPventureBanking.Controllers.TransferObjects.Admin;
using APPventureBanking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APPventureBanking.Controllers.Admin;

[ApiController]
[Route("/Admin/Identity")]
public class AdminIdentityController : ControllerBase
{
    private readonly ILogger<AdminIdentityController> _logger;
    private readonly BankContext _context;
    
    public AdminIdentityController(ILogger<AdminIdentityController> logger, BankContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<IdentityResponse>), 200)]
    public IActionResult Get()
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null : authorizationHeader[0].Split(" ")[1];
        
        if (identity != "Admin")
        {
            return Unauthorized();
        }
        
        var responses = _context.Identities
            .Include(i => i.Party)
            .OrderBy(i => i.Party.DisplayName)
            .Select(i => new IdentityResponse
        {
            IdentityId = i.IdentityId,
            PartyId = i.PartyId,
            Type = i.Party.Type,
            FirstName = i.Party.FirstName,
            LastName = i.Party.LastName,
            EntityName = i.Party.EntityName,
            DisplayName = i.Party.DisplayName,
            PrimaryEmailAddress = i.Party.PrimaryEmailAddress
        });
        
        return Ok(responses);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IdentityResponse), 200)]
    public IActionResult Get(int id)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null : authorizationHeader[0].Split(" ")[1];
        
        if (identity != "Admin")
        {
            return Unauthorized();
        }

        var requestedIdentity = _context.Identities
            .Include(i => i.Party)
            .FirstOrDefault(i => i.IdentityId == id);
        if (requestedIdentity == null)
        {
            return NotFound();
        }
        
        var response = new IdentityResponse
        {
            IdentityId = requestedIdentity.IdentityId,
            PartyId = requestedIdentity.PartyId,
            Type = requestedIdentity.Party.Type,
            FirstName = requestedIdentity.Party.FirstName,
            LastName = requestedIdentity.Party.LastName,
            EntityName = requestedIdentity.Party.EntityName,
            DisplayName = requestedIdentity.Party.DisplayName,
            PrimaryEmailAddress = requestedIdentity.Party.PrimaryEmailAddress
        };
        
        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(IdentityResponse), 200)]
    public IActionResult Post([FromBody] CreateIdentityRequest request)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null : authorizationHeader[0].Split(" ")[1];
        
        if (identity != "Admin")
        {
            return Unauthorized();
        }

        var newAddress = new Address
        {
            AddressLine1 = request.MailingAddress.AddressLine1,
            AddressLine2 = request.MailingAddress.AddressLine2,
            City = request.MailingAddress.City,
            StateCode = request.MailingAddress.StateCode,
            PostalCode = request.MailingAddress.PostalCode
        };

        var newParty = new Party
        {
            Type = request.Type,
            FirstName = request.FirstName,
            LastName = request.LastName,
            EntityName = request.EntityName,
            PrimaryEmailAddress = request.PrimaryEmailAddress,
            MailingAddress = newAddress
        };

        var newIdentity = new Identity
        {
            Party = newParty
        };

        _context.Identities.Add(newIdentity);
        _context.SaveChanges();
        
        var response = new IdentityResponse
        {
            IdentityId = newIdentity.IdentityId,
            PartyId = newIdentity.PartyId,
            Type = newIdentity.Party.Type,
            FirstName = newIdentity.Party.FirstName,
            LastName = newIdentity.Party.LastName,
            EntityName = newIdentity.Party.EntityName,
            DisplayName = newIdentity.Party.DisplayName,
            PrimaryEmailAddress = newIdentity.Party.PrimaryEmailAddress
        };
        
        return CreatedAtAction("Post", new { id = newIdentity.IdentityId }, response);
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null : authorizationHeader[0].Split(" ")[1];
        
        if (identity != "Admin")
        {
            return Unauthorized();
        }

        var identityToDelete = _context.Identities.Find(id);
        if (identityToDelete == null)
        {
            return NotFound();
        }
        
        _context.Identities.Remove(identityToDelete);
        _context.SaveChanges();
        return NoContent();
    }
}
