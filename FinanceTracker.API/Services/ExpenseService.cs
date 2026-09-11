using FinanceTracker.API.Data;
using FinanceTracker.API.DTOs;
using FinanceTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly AppDbContext _context;

        public ExpenseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExpenseDto>> GetAllAsync()
        {
            return await _context.Expenses
                .OrderByDescending(e => e.Year)
                .ThenByDescending(e => e.Month)
                .ThenByDescending(e => e.Date)
                .Select(e => MapToDto(e))
                .ToListAsync();
        }

        public async Task<IEnumerable<ExpenseDto>> GetByMonthAsync(int year, int month)
        {
            return await _context.Expenses
                .Where(e => e.Year == year && e.Month == month)
                .OrderByDescending(e => e.Date)
                .Select(e => MapToDto(e))
                .ToListAsync();
        }

        public async Task<ExpenseDto?> GetByIdAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            return expense == null ? null : MapToDto(expense);
        }

        public async Task<ExpenseDto> CreateAsync(CreateExpenseDto dto)
        {
            var expense = new Expense
            {
                Amount = dto.Amount,
                Category = dto.Category,
                Description = dto.Description,
                Date = dto.Date,
                Month = dto.Date.Month,
                Year = dto.Date.Year,
                CreatedAt = DateTime.UtcNow
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return MapToDto(expense);
        }

        public async Task<ExpenseDto?> UpdateAsync(int id, UpdateExpenseDto dto)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) return null;

            expense.Amount = dto.Amount;
            expense.Category = dto.Category;
            expense.Description = dto.Description;
            expense.Date = dto.Date;
            expense.Month = dto.Date.Month;
            expense.Year = dto.Date.Year;

            await _context.SaveChangesAsync();
            return MapToDto(expense);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) return false;

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return true;
        }

        private static ExpenseDto MapToDto(Expense e) => new()
        {
            Id = e.Id,
            Amount = e.Amount,
            Category = e.Category,
            Description = e.Description,
            Date = e.Date,
            Month = e.Month,
            Year = e.Year,
            CreatedAt = e.CreatedAt
        };
    }
}
