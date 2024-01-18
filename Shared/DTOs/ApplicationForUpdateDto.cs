namespace Shared.DTOs;

public class ApplicationForUpdateDto
{
    public string? LoanType { get; set; }
    public decimal? Amount { get; set; }
    public string? Currency { get; set; }
    public string? Period { get; set; } 
}