using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs;

public class ApplicationForCreationDto
{
    [Required(ErrorMessage = "LoanType is a required field.")]
    [MaxLength(100, ErrorMessage = "LoanType max length is 100.")]
    public string LoanType { get; set; } = null!;
    
    [Required(ErrorMessage = "Amount is a required field.")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Currency is a required field.")]
    [MaxLength(3, ErrorMessage = "Currency max length is 3.")]
    public string Currency { get; set; } = null!;

    [Required(ErrorMessage = "Period is a required field.")]
    [MaxLength(50, ErrorMessage = "Period max length is 50.")]
    public string Period { get; set; } = null!;
}