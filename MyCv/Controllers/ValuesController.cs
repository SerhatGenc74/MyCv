using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyCv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        Claim claim = new Claim(
            {
            ClaimTypes.Name, "JohnDoe"
            ,});
    }
}
