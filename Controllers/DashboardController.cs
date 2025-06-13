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

    [HttpGet("GetTransaction")]
    public async Task<IActionResult> GetTransactions(int outletId)
    {
        var transactions = await _context.Transactions
            .Where(x => x.OutletId == outletId && x.IsDeleted != true)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new TransactionDto
            {
                Id = x.Id,
                BillNo = x.BillNo,
                Amount = x.Amount,
                Cash = x.Cash,
                Credit = x.Credit,
                CreatedDateString = x.CreatedAt.HasValue
                    ? x.CreatedAt.Value.ToString("HH:mm:ss")
                    : null,
                OutletId = x.OutletId
            })
            .ToListAsync();

        if (!transactions.Any())
        {
            return NotFound(new
            {
                IsSuccess = false,
                Status = "NotFound",
                Message = "No transactions found.",
                ReturnObject = new List<TransactionDto>()
            });
        }

        return Ok(new
        {
            IsSuccess = true,
            Status = "Success",
            Message = "Record Found",
            ReturnObject = transactions
        });
    }
    [HttpGet("GetDailySummaryByDate")]
    public async Task<IActionResult> GetDailySummaryByDate(int outletId)
    {
        var summary = await _context.DailySummaries
            .Where(x => x.OutletId == outletId)
            .OrderByDescending(x => x.SummaryDate)
            .Select(x => new DailySummaryDto
            {
                Id = x.Id,
                CashAmount = x.CashAmount,
                CreditCardAmount = x.CreditCardAmount,
                SalesAmount = x.SalesAmount,
                TotalBills = x.TotalBills,
                SummaryDate = x.SummaryDate
            })
            .ToListAsync();

        if (!summary.Any())
        {
            return NotFound(new
            {
                IsSuccess = false,
                Status = "NotFound",
                Message = "No records found.",
                ReturnObject = new List<DailySummaryDto>()
            });
        }

        return Ok(new
        {
            IsSuccess = true,
            Status = "Success",
            Message = "Record Found",
            ReturnObject = summary
        });
    }
    [HttpGet("GetChartsByType")]
    public async Task<IActionResult> GetChartsByType([FromQuery] string type, [FromQuery] int outletId)
    {
        var groupSummaries = await _context.GroupSummaries
            .Where(g => g.OutletId == outletId)
            .ToListAsync();

        if (groupSummaries == null || !groupSummaries.Any())
        {
            return NotFound(new ChartResponse
            {
                IsSuccess = false,
                Status = "NotFound",
                Message = "No records found.",
                ReturnObject = null
            });
        }

        var labels = groupSummaries.Select(g => g.GroupName).ToList();
        var data = groupSummaries.Select(g => g.Amount).ToList();

        var backgroundColors = new List<string>
    {
        "#c38cfe", "#20807c", "#7cdc7c", "#2fa992", "#721a90", "#d5708d",
        "#9c2521", "#7033e7", "#e7cfa5", "#8216ac", "#1f4a23", "#bd0d7d"
    };

        object chartData;

        if (type.ToLower() == "pie" || type.ToLower() == "line")
        {
            chartData = new
            {
                labels = labels,
                datasets = new[]
                {
                new {
                    data = data,
                    backgroundColor = backgroundColors.Take(data.Count).ToList()
                }
            }
            };
        }
        else
        {
            return BadRequest(new ChartResponse
            {
                IsSuccess = false,
                Status = "Error",
                Message = "Unsupported chart type",
                ReturnObject = null
            });
        }

        return Ok(new ChartResponse
        {
            IsSuccess = true,
            Status = "Success",
            Message = "Record Found",
            ReturnObject = chartData
        });
    }

[HttpGet("GetTodayStatus")]
public async Task<IActionResult> GetTodayStatus([FromQuery] int outletId)
{
    var status = await _context.TodayStatuses
        .Where(s => s.OutletId == outletId)
        .OrderByDescending(s => s.LastBillTime) // optional: get latest entry
        .FirstOrDefaultAsync();

    if (status == null)
    {
        return NotFound(new ApiResponse<object>
        {
            IsSuccess = false,
            Status = "NotFound",
            Message = "No records found",
            ReturnObject = null
        });
    }

    var dto = new TodayStatusDto
    {
        Id = status.Id,
        CashAmount = status.CashAmount,
        CreditCardAmount = status.CreditCardAmount,
        SalesAmount = status.SalesAmount,
        LastBillAmount = status.LastBillAmount,
        LastBillTime = status.LastBillTime,
        TotalBills = status.TotalBills,
        NoOfCustomers = status.NoOfCustomers
    };

    return Ok(new ApiResponse<List<TodayStatusDto>>
    {
        IsSuccess = true,
        Status = "Success",
        Message = "Record Found",
        ReturnObject = new List<TodayStatusDto> { dto }
    });
}

}