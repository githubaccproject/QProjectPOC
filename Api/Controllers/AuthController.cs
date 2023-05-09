using Domain.Entities;
using Infrastructure.Repositories.UserRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public AuthController(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid registration request");
        }
        await _userRepository.AddUserAsync(model);
        return Ok("User registered successfully");
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid login request");
        }
        var userId = await _userRepository.GetUserByUsernameAsync(model.Username, model.Password);

        if (userId == null)
        {
            return Unauthorized("Invalid username or password");
        }

        var token = GenerateJwtToken((int)userId);
        return Ok(new { token });
    }


    private string GenerateJwtToken(int userId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),

        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"])),
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }
}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}
