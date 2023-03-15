using System.ComponentModel.DataAnnotations;

namespace EyesApiJwt.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public ICollection<Submission>? Submissions { get; set; } = null;
        public string? Name { get; set; } = string.Empty;
        public string? Image { get; set; } = null;
        public string? Email { get; set; } = string.Empty;
        public long? IssuesAtSeconds { get; set; } = null;

    }
}
