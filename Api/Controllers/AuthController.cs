using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PlantAnimalApi.Contracts;
using PlantAnimalApi.Data;
using PlantAnimalApi.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly BioscopeDbContext _db;
    private readonly string _jwtKey;

    public AuthController(BioscopeDbContext db, IConfiguration cfg)
    {
        _db = db;
        _jwtKey = cfg["JWT:Key"] ?? Environment.GetEnvironmentVariable("JWT__Key") ?? "dev-super-secret";
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest req)
    {
        if (await _db.Users.AnyAsync(u => u.Email.ToLower() == req.Email.ToLower()))
            return Conflict("Email already registered.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = req.Email,
            DisplayName = req.DisplayName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok(new AuthResponse(GenerateJwt(user), user.Id, user.DisplayName));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest req)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == req.Email.ToLower());
        if (user is null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials.");

        return Ok(new AuthResponse(GenerateJwt(user), user.Id, user.DisplayName));
    }

    private string GenerateJwt(User user)
    {
        var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("display_name", user.DisplayName)
            },
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
