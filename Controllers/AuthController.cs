using EyesApiJwt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net.NetworkInformation;
using System.Text;
using System.Linq;
using EyesApiJwt.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EyesApiJwt.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public AuthController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {
            
            
            // Checking if user already exists
            if (_context.Users.Any(o => o.Username == request.Username))
            {
                return BadRequest("User already exists");
            }

            // Creating user object
            
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.Username = request.Username;
            user.PasswordHash = passwordHash;

            // Adding to database
            _context.Users.AddAsync(user);
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserDto request)
        {
            // Checking if registered
            User? checkUser = _context.Users.Find(request.Username);
            if (checkUser == null)
            {
                return BadRequest("User not registered");
            }

            // Checking password
            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(user);
            

            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        [HttpPost("logout")]
        public ActionResult<string> Logout(string token)
        {
            

            var jwtHandler = new JwtSecurityTokenHandler();

            var jwtToken = jwtHandler.ReadJwtToken(token);

            
            var nameClaim = jwtToken.Payload.FirstOrDefault(c => c.Key == "name");

            user = new User();

            Console.WriteLine(nameClaim);

            return Ok("logged out" + nameClaim.Value);
        }
    }


}
