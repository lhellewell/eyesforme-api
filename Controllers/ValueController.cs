using Azure.Core;
using EyesApiJwt.Data;
using EyesApiJwt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace EyesApiJwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        private readonly DataContext _context;

        public ValueController(DataContext context)
        {
            _context = context;
        }

        /**
         * Function for submitting an entry
         */
        [HttpPost("submit")]
        public ActionResult<Submission> Submit(Submission submission, string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            var jwtToken = jwtHandler.ReadJwtToken(token);

            var nameClaim = jwtToken.Payload.FirstOrDefault(c => c.Key == "name").Value.ToString();

            // Checks if user exists
            if (_context.Users.Any(o => o.Username == nameClaim))
            {
                _context.Submissions.Add(submission);
            }

            return BadRequest("User doesn't exist, submission failed");

        }

        [HttpGet("submissions")]
        public ActionResult<List<Submission>> Submissions(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            var jwtToken = jwtHandler.ReadJwtToken(token);

            var nameClaim = jwtToken.Payload.FirstOrDefault(c => c.Key == "name").Value.ToString();

            if (_context.Users.Any(o => o.Username == nameClaim))
            {
                

                var matchingEntries = _context.Submissions
                    .Where(m => m.UserId == nameClaim)
                        .ToList();

                return matchingEntries;
            }
            

            return BadRequest("User doesn't exist, get submissions failed");

        }

    }
}
