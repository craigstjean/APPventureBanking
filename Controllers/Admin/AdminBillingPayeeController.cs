using APPventureBanking.Controllers.TransferObjects;
using APPventureBanking.Controllers.TransferObjects.Admin;
using APPventureBanking.Models;
using APPventureBanking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APPventureBanking.Controllers.Admin;

[ApiController]
[Route("/Admin/BillingPayee")]
public class AdminBillingPayeeController : ControllerBase
{
    private readonly ILogger<AdminBillingPayeeController> _logger;
    private readonly BankContext _context;
    private readonly AccountService _accountService;
    
    public AdminBillingPayeeController(ILogger<AdminBillingPayeeController> logger, BankContext context, AccountService accountService)
    {
        _logger = logger;
        _context = context;
        _accountService = accountService;
    } 
    
    [HttpGet]
    [ProducesResponseType(typeof(List<BillingPayeeResponse>), 200)]
    public IActionResult Get()
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null : authorizationHeader[0].Split(" ")[1];
        
        if (identity != "Admin")
        {
            return Unauthorized();
        }
        
        var responses = _context.BillingPayees
            .Include(bp => bp.Party)
            .Include(bp => bp.BillingAddress)
            .ToList()
            .OrderBy(bp => bp.Party.DisplayName)
            .Select(bp => new BillingPayeeResponse 
        {
            BillingPayeeId = bp.BillingPayeeId,
            PartyId = bp.PartyId,
            Party = bp.Party,
            BillingAddressId = bp.BillingAddressId,
            BillingAddress = bp.BillingAddress
        });
        
        return Ok(responses);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BillingPayeeResponse), 200)]
    public IActionResult Get(int id)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null : authorizationHeader[0].Split(" ")[1];
        
        if (identity != "Admin")
        {
            return Unauthorized();
        }

        var billingPayee = _context.BillingPayees
            .Include(bp => bp.Party)
            .Include(bp => bp.BillingAddress)
            .FirstOrDefault(bp => bp.BillingPayeeId == id);
        if (billingPayee == null)
        {
            return NotFound();
        }
        
        var response = new BillingPayeeResponse
        {
            BillingPayeeId = billingPayee.BillingPayeeId,
            PartyId = billingPayee.PartyId,
            Party = billingPayee.Party,
            BillingAddressId = billingPayee.BillingAddressId,
            BillingAddress = billingPayee.BillingAddress
        };
        
        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(BillingPayeeResponse), 200)]
    public IActionResult Post([FromBody] CreateBillingPayeeRequest request)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null : authorizationHeader[0].Split(" ")[1];
        
        if (identity != "Admin")
        {
            return Unauthorized();
        }

        var address = new Address
        {
            AddressLine1 = request.AddressLine1,
            AddressLine2 = request.AddressLine2,
            City = request.City,
            StateCode = request.StateCode,
            PostalCode = request.PostalCode
        };

        var party = new Party
        {
            Type = PartyType.Entity,
            EntityName = request.EntityName,
            PrimaryEmailAddress = request.PrimaryEmailAddress,
            MailingAddress = address
        };
        
        var account = new Account
        {
            AccountNumber = _accountService.NextAccountNumber(),
            AccountType = AccountType.Checking,
            Name = request.EntityName,
            IsDeleted = false
        };

        var billingPayee = new BillingPayee
        {
            Party = party,
            BillingAddress = address,
            ReferenceAccount = account
        };

        var newIdentity = new Identity
        {
            Party = party,
            Accounts = new List<Account>() { account }
        };

        _context.BillingPayees.Add(billingPayee);
        _context.Identities.Add(newIdentity);
        _context.SaveChanges();
        
        var response = new BillingPayeeResponse
        {
            BillingPayeeId = billingPayee.BillingPayeeId,
            PartyId = billingPayee.PartyId,
            Party = billingPayee.Party,
            BillingAddressId = billingPayee.BillingAddressId,
            BillingAddress = billingPayee.BillingAddress
        };
        
        return CreatedAtAction("Post", new { id = billingPayee.BillingPayeeId }, response);
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

        var billingPayee = _context.BillingPayees.Find(id);
        if (billingPayee == null)
        {
            return NotFound();
        }
        
        _context.BillingPayees.Remove(billingPayee);
        _context.SaveChanges();
        return NoContent();
    }
}
