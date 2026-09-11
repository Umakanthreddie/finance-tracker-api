using FinanceTracker.API.Data;
using FinanceTracker.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SummaryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private static readonly string[] MonthNames =
        [
            "January","February","March","April","May","June",
            "July","August","September","October","November","December"
        ];

        public SummaryController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>Get financial summary for a specific month and year</summary>
        [HttpGet("{year:int}/{month:int}")]
        public async Task<ActionResult<MonthlySummaryDto>> GetMonthlySummary(int year, int month)
        {
            if (month < 1 || month > 12)
                return BadRequest("Month must be between 1 and 12.");

            var totalIncome = await _context.Incomes
                .Where(i => i.Year == year && i.Month == month)
                .SumAsync(i => (decimal?)i.Amount) ?? 0;

            var totalExpenses = await _context.Expenses
                .Where(e => e.Year == year && e.Month == month)
                .SumAsync(e => (decimal?)e.Amount) ?? 0;

            var expensesByCategory = await _context.Expenses
                .Where(e => e.Year == year && e.Month == month)
                .GroupBy(e => e.Category)
                .Select(g => new CategoryBreakdownDto
                {
                    Category = g.Key,
                    Total = g.Sum(e => e.Amount)
                })
                .ToListAsync();

            return Ok(new MonthlySummaryDto
            {
                Year = year,
                Month = month,
                MonthName = MonthNames[month - 1],
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                NetSavings = totalIncome - totalExpenses,
                ExpensesByCategory = expensesByCategory
            });
        }

        /// <summary>Get yearly financial summary with month-by-month breakdown</summary>
        [HttpGet("yearly/{year:int}")]
        public async Task<ActionResult<YearlySummaryDto>> GetYearlySummary(int year)
        {
            var incomeByMonth = await _context.Incomes
                .Where(i => i.Year == year)
                .GroupBy(i => i.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(i => i.Amount) })
                .ToListAsync();

            var expenseByMonth = await _context.Expenses
                .Where(e => e.Year == year)
                .GroupBy(e => e.Month)
                .Select(g => new { Month = g.Key, Total = g.Sum(e => e.Amount) })
                .ToListAsync();

            var monthlyBreakdown = Enumerable.Range(1, 12).Select(m =>
            {
                var income = incomeByMonth.FirstOrDefault(x => x.Month == m)?.Total ?? 0;
                var expenses = expenseByMonth.FirstOrDefault(x => x.Month == m)?.Total ?? 0;
                return new MonthlyBreakdownDto
                {
                    Month = m,
                    MonthName = MonthNames[m - 1],
                    Income = income,
                    Expenses = expenses,
                    Savings = income - expenses
                };
            }).ToList();

            return Ok(new YearlySummaryDto
            {
                Year = year,
                TotalIncome = monthlyBreakdown.Sum(m => m.Income),
                TotalExpenses = monthlyBreakdown.Sum(m => m.Expenses),
                NetSavings = monthlyBreakdown.Sum(m => m.Savings),
                MonthlyBreakdown = monthlyBreakdown
            });
        }
    }
}
