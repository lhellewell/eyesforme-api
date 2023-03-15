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

        [HttpGet("login")]
        public async Task<ActionResult<User>> GetUser()
        {
            Console.WriteLine("HELLOYO \n \n TEST ETEST");

            // Get the access token from the Authorization header
            string accessToken = Request.Headers["Authorization"].ToString().Split(' ')[1];


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

                // The user has not been authenticated
                if (authenticatedUserId == null)
                {
                    return Forbid();
                }



                // Retrieve the user's data from the database
                User? user = await _context.Users.FindAsync(authenticatedUserId);

                // Check if user has logged in before
                if (user == null)
                {
                    // Creating new user entity
                    user = new User();
                    user.UserId = authenticatedUserId;
                    user.Email = payload.Email;
                    user.IssuesAtSeconds = payload.IssuedAtTimeSeconds;
                    user.Image = payload.Picture;
                    user.Name = payload.Name;


                    // Hasn't logged into database before
                    await _context.Users.AddAsync(user);
                    _context.SaveChanges();
                }

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
