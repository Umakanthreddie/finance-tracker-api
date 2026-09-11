using FinanceTracker.API.Data;
using FinanceTracker.API.DTOs;
using FinanceTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly AppDbContext _context;

        public IncomeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IncomeDto>> GetAllAsync()
        {
            return await _context.Incomes
                .OrderByDescending(i => i.Year)
                .ThenByDescending(i => i.Month)
                .Select(i => MapToDto(i))
                .ToListAsync();
        }

        public async Task<IEnumerable<IncomeDto>> GetByMonthAsync(int year, int month)
        {
            return await _context.Incomes
                .Where(i => i.Year == year && i.Month == month)
                .OrderByDescending(i => i.CreatedAt)
                .Select(i => MapToDto(i))
                .ToListAsync();
        }

        public async Task<IncomeDto?> GetByIdAsync(int id)
        {
            var income = await _context.Incomes.FindAsync(id);
            return income == null ? null : MapToDto(income);
        }

        public async Task<IncomeDto> CreateAsync(CreateIncomeDto dto)
        {
            var income = new Income
            {
                Amount = dto.Amount,
                Source = dto.Source,
                Month = dto.Month,
                Year = dto.Year,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();
            return MapToDto(income);
        }

        public async Task<IncomeDto?> UpdateAsync(int id, UpdateIncomeDto dto)
        {
            var income = await _context.Incomes.FindAsync(id);
            if (income == null) return null;

            income.Amount = dto.Amount;
            income.Source = dto.Source;
            income.Month = dto.Month;
            income.Year = dto.Year;
            income.Description = dto.Description;

            await _context.SaveChangesAsync();
            return MapToDto(income);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var income = await _context.Incomes.FindAsync(id);
            if (income == null) return false;

            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();
            return true;
        }

        private static IncomeDto MapToDto(Income i) => new()
        {
            Id = i.Id,
            Amount = i.Amount,
            Source = i.Source,
            Month = i.Month,
            Year = i.Year,
            Description = i.Description,
            CreatedAt = i.CreatedAt
        };
    }
}
