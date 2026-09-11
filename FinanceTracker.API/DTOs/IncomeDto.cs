namespace FinanceTracker.API.DTOs
{
    public class IncomeDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Source { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateIncomeDto
    {
        public decimal Amount { get; set; }
        public string Source { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateIncomeDto
    {
        public decimal Amount { get; set; }
        public string Source { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
        public string? Description { get; set; }
    }
}
