namespace Shared.DTOs;

public class ApplicationDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string LoanType { get; set; } = null!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = null!;
    public string Period { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}