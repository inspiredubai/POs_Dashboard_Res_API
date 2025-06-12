using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PosApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthenticationController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole<int>> roleManager,
        AppDbContext context,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Unauthorized(new LoginResponse { IsSuccess = false, Status = "Failed", Message = "User not found" });
        }

        if (!await _userManager.IsInRoleAsync(user, request.Role))
        {
            return Unauthorized(new LoginResponse { IsSuccess = false, Status = "Failed", Message = "Role mismatch" });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return Unauthorized(new LoginResponse { IsSuccess = false, Status = "Failed", Message = "Invalid credentials" });
        }

        var token = GenerateJwtToken(user, request.Role);

        var outlets = await _context.OutletMasters
            .Where(o => o.UserId == user.Id && o.IsActive && !o.IsDeleted)
            .ToListAsync();

        var totalOutlets = outlets.Count;
        var outletId = outlets.FirstOrDefault()?.Id ?? 0;

        var responseObject = new
        {
            user.Id,
            user.Name,
            user.FatherName,
            user.RegisterationCode,
            user.DOB,
            user.Image,
            user.Email,
            user.MemberType,
            user.JoiningDate,
            user.CNIC,
            user.Contact,
            user.IsManagement,
            Token = token,
            Role = request.Role,
            TokenValidation = user.TokenValidation,
            user.IsActive,
            user.IsDeleted,
            user.CreatedBy,
            user.ModifiedBy,
            TotalOutlets = totalOutlets,
            OutletId = outletId
        };

        return Ok(new LoginResponse
        {
            IsSuccess = true,
            Status = "Success",
            Message = "User is verified",
            ReturnObject = responseObject
        });
    }

    private string GenerateJwtToken(ApplicationUser user, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("email", user.Email)
        };

        var key = Convert.FromBase64String(_configuration["Jwt:SecretKey"]);
        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddHours(5);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiry,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    [Authorize]
[HttpPost("Logout")]
public IActionResult Logout()
{
    return Ok(new ApiResponse<bool>
    {
        IsSuccess = true,
        Status = "Success",
        Message = "User logged out",
        ReturnObject = true
    });
}
}
