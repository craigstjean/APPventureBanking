using APPventureBanking.Models;
using Microsoft.AspNetCore.Mvc;

namespace APPventureBanking.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountTypeController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<string>), 200)]
    public IActionResult Get()
    {
        return Ok(Enum.GetNames(typeof(AccountType)));
    }
}
