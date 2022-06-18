using APPventureBanking.Controllers.TransferObjects;
using APPventureBanking.Controllers.TransferObjects.Admin;
using APPventureBanking.Models;
using APPventureBanking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APPventureBanking.Controllers.Admin;

[ApiController]
[Route("/Admin/Account")]
public class AdminAccountController : ControllerBase
{
    private readonly ILogger<AdminAccountController> _logger;
    private readonly BankContext _context;
    private readonly AccountService _accountService;
    
    public AdminAccountController(ILogger<AdminAccountController> logger, BankContext context, AccountService accountService)
    {
        _logger = logger;
        _context = context;
        _accountService = accountService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(AccountBalanceResponse), 200)]
    public IActionResult GetByAccountNumber(int accountNumber)
    {
        Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
        var identity = authorizationHeader.Count == 0 ? null : authorizationHeader[0].Split(" ")[1];
        
        if (identity != "Admin")
        {
            return Unauthorized();
        }

        var account = _context.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
        if (account == null)
        {
            return NotFound();
        }
        
        var response = new AccountBalanceResponse
        {
            AccountId = account.AccountId,
            AccountNumber = account.AccountNumber,
            AccountType = account.AccountType,
            Name = account.Name,
            Balance = _accountService.GetBalance(account.AccountId)
        };
        
        return Ok(response);
    }
}
