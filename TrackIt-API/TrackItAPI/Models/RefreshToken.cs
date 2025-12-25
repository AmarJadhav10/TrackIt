using System.ComponentModel.DataAnnotations;

namespace TrackItAPI.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; } = false;

        // FK
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
