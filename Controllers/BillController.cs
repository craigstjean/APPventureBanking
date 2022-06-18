using APPventureBanking.Controllers.TransferObjects;
using APPventureBanking.Models;
using APPventureBanking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APPventureBanking.Controllers;

[ApiController]
[Route("[controller]")]
public class BillController : ControllerBase
{
    private readonly ILogger<BillController> _logger;
    private readonly BankContext _context;
    private readonly AccountService _accountService;
    private readonly BillService _billService;

    public BillController(ILogger<BillController> logger, BankContext context, AccountService accountService, BillService billService)
    {
        _logger = logger;
        _context = context;
        _accountService = accountService;
        _billService = billService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<BillResponse>), 200)]
    public IActionResult Get()
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }

        var bills = _context.Bills
            .Include(b => b.BillingPayee)
            .Include(b => b.BillingPayee.Party)
            .Include(b => b.BillingPayee.BillingAddress)
            .Where(b => b.IdentityId == identity.IdentityId)
            .OrderByDescending(b => b.DueDate)
            .ToList();
        var responses = bills.Select(b => new BillResponse
        {
            BillId = b.BillId,
            BillingPayeeId = b.BillingPayeeId,
            BillingPayee = new BillingPayeeResponse
            {
                BillingPayeeId = b.BillingPayee.BillingPayeeId,
                PartyId = b.BillingPayee.PartyId,
                Party = b.BillingPayee.Party,
                BillingAddressId = b.BillingPayee.BillingAddressId,
                BillingAddress = b.BillingPayee.BillingAddress
            },
            DueDate = b.DueDate.ToDateTime(TimeOnly.MaxValue),
            AmountDue = b.AmountDue,
            AmountPaid = _billService.GetAmountPaid(b.BillId)
        });
        
        return Ok(responses);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BillResponse), 200)]
    public IActionResult Get(int id)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }
        
        var bill = _context.Bills.Find(id);
        if (bill == null)
        {
            return NotFound();
        }
        
        if (bill.IdentityId != identity.IdentityId)
        {
            return Unauthorized();
        }
        
        var response = new BillResponse
        {
            BillId = bill.BillId,
            BillingPayeeId = bill.BillingPayeeId,
            BillingPayee = new BillingPayeeResponse
            {
                BillingPayeeId = bill.BillingPayee.BillingPayeeId,
                PartyId = bill.BillingPayee.PartyId,
                Party = bill.BillingPayee.Party,
                BillingAddressId = bill.BillingPayee.BillingAddressId,
                BillingAddress = bill.BillingPayee.BillingAddress
            },
            DueDate = bill.DueDate.ToDateTime(TimeOnly.MaxValue),
            AmountDue = bill.AmountDue,
            AmountPaid = _billService.GetAmountPaid(bill.BillId)
        };
        
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BillResponse), 200)]
    public IActionResult Post([FromBody] PayBillRequest request)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }
        
        var fromAccount =
            _context.Accounts.FirstOrDefault(a => a.AccountId == request.FromAccountId && !a.IsDeleted);
        var identityAccount = (from i in _context.Identities
                where i.IdentityId == identity.IdentityId
                    && i.Accounts.Contains(fromAccount)
                select i).FirstOrDefault();
        if (identityAccount == null)
        {
            return Unauthorized();
        }
        
        var bill = _context.Bills
            .Include(b => b.AssociatedTransactions)
            .Include(b => b.BillingPayee)
            .Include(b => b.BillingPayee.Party)
            .FirstOrDefault(b => b.BillId == request.BillId);

        if (bill == null)
        {
            return NotFound();
        }
        
        var transaction = new Transaction
        {
            FromAccountId = request.FromAccountId,
            ToAccountId = bill.BillingPayee.ReferenceAccountId,
            TransactionDateTime = DateTime.Now,
            Amount = request.Amount,
            Description = "Bill Pay - " + bill.BillId + " - " + bill.BillingPayee.Party.DisplayName,
        };
        
        bill.AssociatedTransactions.Add(transaction);
        _context.SaveChanges();
        
        var response = new BillResponse
        {
            BillId = request.BillId,
            BillingPayeeId = bill.BillingPayeeId,
            DueDate = bill.DueDate.ToDateTime(TimeOnly.MaxValue),
            AmountDue = bill.AmountDue,
            AmountPaid = _billService.GetAmountPaid(bill.BillId)
        };

        return Ok(response);
    }

    [HttpGet("{id}/Transactions")]
    [ProducesResponseType(typeof(List<TransactionResponse>), 200)]
    public IActionResult GetTransactionsByBill(int id)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null
            : _context.Identities.Find(int.Parse(authorizationHeader[0].Split(" ")[1]));
        
        if (identity == null)
        {
            return Unauthorized();
        }
        
        var bill = _context.Bills
            .Include(b => b.AssociatedTransactions)
            .FirstOrDefault(b => b.BillId == id);
        if (bill == null)
        {
            return NotFound();
        }
        
        if (bill.IdentityId != identity.IdentityId)
        {
            return Unauthorized();
        }

        var responses = bill.AssociatedTransactions
            .OrderByDescending(t => t.TransactionDateTime)
            .Select(t => new TransactionResponse
        {
            TransactionId = t.TransactionId,
            FromAccountId = t.FromAccountId,
            ToAccountId = t.ToAccountId,
            TransactionDateTime = t.TransactionDateTime,
            Amount = t.Amount,
            Balance = _accountService.GetBalanceAsOfTransaction(t.FromAccountId, t.TransactionId),
            Description = t.Description
        });
        
        return Ok(responses);
    }
}
