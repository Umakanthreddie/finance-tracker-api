namespace FinanceTracker.API.DTOs
{
    public class MonthlySummaryDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetSavings { get; set; }
        public List<CategoryBreakdownDto> ExpensesByCategory { get; set; } = new();
    }

    public class CategoryBreakdownDto
    {
        public string Category { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }

    public class YearlySummaryDto
    {
        public int Year { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetSavings { get; set; }
        public List<MonthlyBreakdownDto> MonthlyBreakdown { get; set; } = new();
    }

    public class MonthlyBreakdownDto
    {
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
        public decimal Savings { get; set; }
    }
}
