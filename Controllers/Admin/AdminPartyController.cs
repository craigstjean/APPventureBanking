using APPventureBanking.Controllers.TransferObjects.Admin;
using APPventureBanking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APPventureBanking.Controllers.Admin;

[ApiController]
[Route("/Admin/Party")]
public class AdminPartyController : ControllerBase
{
    private readonly ILogger<AdminPartyController> _logger;
    private readonly BankContext _context;
    
    public AdminPartyController(ILogger<AdminPartyController> logger, BankContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] UpdatePartyRequest request)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null : authorizationHeader[0].Split(" ")[1];
        
        if (identity != "Admin")
        {
            return Unauthorized();
        }
        
        var partyToUpdate = _context.Parties.Include(p => p.MailingAddress).FirstOrDefault(p => p.PartyId == id);
        if (partyToUpdate == null)
        {
            return NotFound();
        }
        
        partyToUpdate.Type = request.Type;
        partyToUpdate.FirstName = request.FirstName;
        partyToUpdate.LastName = request.LastName;
        partyToUpdate.EntityName = request.EntityName;
        partyToUpdate.PrimaryEmailAddress = request.PrimaryEmailAddress;
        partyToUpdate.MailingAddress.AddressLine1 = request.AddressLine1;
        partyToUpdate.MailingAddress.AddressLine2 = request.AddressLine2;
        partyToUpdate.MailingAddress.City = request.City;
        partyToUpdate.MailingAddress.StateCode = request.StateCode;
        partyToUpdate.MailingAddress.PostalCode = request.PostalCode;
        
        _context.SaveChanges();
        return NoContent();
    }
}
