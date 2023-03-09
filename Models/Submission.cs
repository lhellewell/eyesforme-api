using System.ComponentModel.DataAnnotations;

namespace EyesApiJwt.Models
{
    public class Submission
    {
        public int SubmissionId { get; set; }
        public DateTime TimeSubmitted { get; set; }
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
        public byte[] Image { get; set; } = null!;
        public string Task { get; set; } = null!;
        public string? Input { get; set; }
    }
}
