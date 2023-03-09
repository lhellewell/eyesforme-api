using System.ComponentModel.DataAnnotations;

namespace EyesApiJwt.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public ICollection<Submission> Submissions { get; set; } = null!;

    }
}
