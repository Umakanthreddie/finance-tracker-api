namespace FinanceTracker.API.Models
{
    public class Income
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Source { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
