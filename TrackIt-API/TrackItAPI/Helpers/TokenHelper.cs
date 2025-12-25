using System.Security.Cryptography;

namespace TrackItAPI.Helpers
{
    public static class TokenHelper
    {
        public static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
