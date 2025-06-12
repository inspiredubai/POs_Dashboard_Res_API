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

    [HttpGet("GetGroupSummary")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> GetGroupSummary(int outletId)
    {
        var summaries = await _context.GroupSummaries
            .Where(g => g.OutletId == outletId)
            .Select(g => new
            {
                g.Id,
                g.GroupName,
                g.Amount,
                g.Percentage
            })
            .ToListAsync();

        if (summaries == null || summaries.Count == 0)
        {
            return NotFound(new
            {
                IsSuccess = false,
                Status = "Failed",
                Message = "No records found",
                ReturnObject = new object[] { }
            });
        }

        return Ok(new
        {
            IsSuccess = true,
            Status = "Success",
            Message = "Record Found",
            ReturnObject = summaries
        });
    }
}
