using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth;
using EyesApiJwt.Models;
using EyesApiJwt.Data;
using Microsoft.EntityFrameworkCore;

namespace EyesApiJwt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUser(string userId)
        {
            Console.WriteLine("HELLOYO \n \n TEST ETEST");

            // Get the access token from the Authorization header
            string accessToken = Request.Headers["Authorization"].ToString().Split(' ')[1];

            var request = Request;

            try
            {
                // Verify the access token with Google's authentication server
                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(
                    accessToken,
                    new GoogleJsonWebSignature.ValidationSettings()
                    {
                        Audience = new[] { "41057098010-78mhsj3br4r5ki9lpii0ed0ef9bch0kr.apps.googleusercontent.com" }
                    }
                );

                // The access token is valid, and the user is authenticated
                string authenticatedUserId = payload.Subject;

                if (authenticatedUserId != "111750033314175717878")
                {
                    // The authenticated user does not have permission to access this user's data
                    return Forbid();
                }

                // Retrieve the user's data from the database and return it
                User? user = await _context.Users.FindAsync("strin1g");
                return Ok(user);
            }
            catch (InvalidJwtException)
            {
                // The access token is invalid or has expired
                return Unauthorized();
            }
        }
    }
}
