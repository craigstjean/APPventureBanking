using APPventureBanking.Models;
using Microsoft.AspNetCore.Mvc;

namespace APPventureBanking.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountTypeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(Enum.GetNames(typeof(AccountType)));
    }
}
