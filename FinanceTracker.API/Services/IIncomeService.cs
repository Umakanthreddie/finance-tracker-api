using FinanceTracker.API.DTOs;

namespace FinanceTracker.API.Services
{
    public interface IIncomeService
    {
        Task<IEnumerable<IncomeDto>> GetAllAsync();
        Task<IEnumerable<IncomeDto>> GetByMonthAsync(int year, int month);
        Task<IncomeDto?> GetByIdAsync(int id);
        Task<IncomeDto> CreateAsync(CreateIncomeDto dto);
        Task<IncomeDto?> UpdateAsync(int id, UpdateIncomeDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
