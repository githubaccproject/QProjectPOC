using Domain.Entities;
using Infrastructure.Repositories.UserRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public AuthController(IConfiguration configuration,IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        // Validate model
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid registration request");
        }



        // Replace with your actual logic for registering a new user
        // For example, creating a new user record in the database with the provided registration data
        var userId = await _userRepository.AddUserAsync(model);

        // Optionally, you can return additional data, such as the created user's ID or other information, in the response

        // Return a success response
        return Ok("User registered successfully");
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        // Validate model
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid login request");
        }

        // Replace with your actual logic for validating the user's credentials
        // For example, querying a database to check if the username and password are valid
        var userId = await _userRepository.GetUserByUsernameAsync(model.Username, model.Password);

        if (userId == null)
        {
            return Unauthorized("Invalid username or password");
        }

        // Generate JWT token
        var token = GenerateJwtToken((int)userId);

        // Return the token as the response
        return Ok(new { token });
    }



    // Generate JWT token
    private string GenerateJwtToken(int userId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            // Add additional claims as needed for your application
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
