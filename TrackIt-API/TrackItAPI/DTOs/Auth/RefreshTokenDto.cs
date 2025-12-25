using System.ComponentModel.DataAnnotations;

namespace TrackItAPI.DTOs.Auth
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
