using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

[Table("applications")]
public class Application
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("user_id")]
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    [Column("loan_type")]
    [Required(ErrorMessage = "LoanType is a required field.")]
    [MaxLength(100, ErrorMessage = "LoanType max length is 100.")]
    public string LoanType { get; set; } = null!;
    
    [Column("amount")]
    [Required(ErrorMessage = "Amount is a required field.")]
    public decimal Amount { get; set; }

    [Column("currency")]
    [Required(ErrorMessage = "Currency is a required field.")]
    [MaxLength(3, ErrorMessage = "Currency max length is 3.")]
    public string Currency { get; set; } = null!;

    [Column("period")]
    [Required(ErrorMessage = "Period is a required field.")]
    [MaxLength(50, ErrorMessage = "Period max length is 50.")]
    public string Period { get; set; } = null!;

    [Column("status")]
    [Required(ErrorMessage = "Status is a required field.")]
    [MaxLength(50, ErrorMessage = "Status max length is 50.")]
    public string Status { get; set; } = null!;

    [Column("created_at")]
    [Required(ErrorMessage = "CreatedAt is a required field.")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    [Required(ErrorMessage = "UpdatedAt is a required field.")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public User? User { get; set; }
}