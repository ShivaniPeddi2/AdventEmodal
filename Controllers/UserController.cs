using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public UserController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = _context.Users.SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password);

        if (user == null)
        {
            return Unauthorized(); // User not found or incorrect password
        }

        var token = GenerateJwtToken(user);
        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(User user)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]); // Ensure this key is 256 bits
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
        }),
        Expires = DateTime.UtcNow.AddHours(1),
        Issuer = _configuration["Jwt:Issuer"],
        Audience = _configuration["Jwt:Issuer"],
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}


    // Secure endpoints with [Authorize] attribute
    [HttpGet]
    [Authorize(Roles = "Admin")] // Only Admins can access this endpoint
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _context.Users.ToListAsync());
    }

    [HttpGet("{id}")]
    [Authorize] // Any authenticated user can access this endpoint
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Validate unique username and email
        if (_context.Users.Any(u => u.Username == user.Username || u.Email == user.Email))
        {
            return Conflict("Username or email already exists");
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
    }

    [HttpPut("{id}")]
    
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
    {
        if (id != user.UserId)
        {
            return BadRequest();
        }

        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        // Update only specific fields
        existingUser.Username = user.Username;
        existingUser.Email = user.Email;
        existingUser.IsAdmin = user.IsAdmin;
        existingUser.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(user.Password))
        {
            existingUser.Password = user.Password; // Update plain text password
        }

        _context.Entry(existingUser).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Only Admins can delete users
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
