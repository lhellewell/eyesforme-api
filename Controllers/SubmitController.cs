using EyesApiJwt.Data;
using EyesApiJwt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EyesApiJwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmitController : ControllerBase
    {
        private readonly DataContext _context;

        public SubmitController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public List<Submission> GetSubmissions(string userId) 
        {
            
            var submissions = _context.Submissions.Where(e => e.UserId == userId).ToList();

            return submissions;
        }

        [HttpPost("new")]
        public ActionResult PostSubmission(Submission submission)
        {
            if (submission.UserId == null) { throw new ArgumentNullException(nameof(submission));}
            
            _context.Submissions.Add(submission);
            _context.SaveChanges();

            return Ok(submission);

        }

    }
}
