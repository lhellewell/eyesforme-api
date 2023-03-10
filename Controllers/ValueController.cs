using EyesApiJwt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EyesApiJwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        
        [HttpPost("submit")]
        public ActionResult<Submission>? Submit(Submission submission, string token)
        {

            return null;
        }

    }
}
