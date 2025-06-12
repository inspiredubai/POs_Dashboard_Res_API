using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("GetOutlets")]
    public async Task<IActionResult> GetOutlets()
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized(new ApiResponse<string>
                {
                    IsSuccess = false,
                    Status = "Failed",
                    Message = "Invalid user token"
                });
            }

            var outlets = await _context.OutletMasters
                .Where(x => x.UserId == userId && x.IsActive && !x.IsDeleted)
                .Select(x => new OutletResponseDto
                {
                    Id = x.Id,
                    Name = x.Name ?? "",
                    Value = x.CreatedAt.ToString("dd-MM-yyyy")
                })
                .ToListAsync();

            return Ok(new ApiResponse<List<OutletResponseDto>>
            {
                IsSuccess = true,
                Status = "Success",
                Message = outlets.Any() ? "Record Found" : "No Record Found",
                ReturnObject = outlets
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<string>
            {
                IsSuccess = false,
                Status = "Error",
                Message = ex.Message
            });
        }
    }
}
