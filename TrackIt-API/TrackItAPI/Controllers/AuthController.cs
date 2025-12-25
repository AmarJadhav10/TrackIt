using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrackItAPI.Data;
using TrackItAPI.DTOs.Auth;
using TrackItAPI.Helpers;
using TrackItAPI.Models;

namespace TrackItAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var hashedPassword = PasswordHelper.HashPassword(dto.Password);

            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Username == dto.Username &&
                    u.PasswordHash == hashedPassword);

            if (user == null)
                return Unauthorized("Invalid credentials");

            var accessToken = GenerateJwtToken(user);
            var refreshToken = TokenHelper.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(
                    int.Parse(_config["Jwt:RefreshTokenExpiryDays"])
                )
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                accessToken,
                refreshToken
            });
        }


        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("Permission", "EditIssue")
            };


            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto dto)
        {
            var token = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt =>
                    rt.Token == dto.RefreshToken &&
                    !rt.IsRevoked &&
                    rt.ExpiresAt > DateTime.UtcNow);

            if (token == null)
                return Unauthorized("Invalid refresh token");

            var newAccessToken = GenerateJwtToken(token.User);

            return Ok(new
            {
                accessToken = newAccessToken
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshTokenDto dto)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == dto.RefreshToken);

            if (token == null)
                return NotFound();

            token.IsRevoked = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}

