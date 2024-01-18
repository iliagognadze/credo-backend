using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Constants;

namespace Entities.Models;

[Table("users")]
public class User
{
    [Column("id")]
    public int Id { get; set; }

    [Column("private_number")]
    [Required(ErrorMessage = "PrivateNumber is a required field.")]
    [MaxLength(9, ErrorMessage = "PrivateNumber max length is 9.")]
    public string PrivateNumber { get; set; } = null!;

    [Column("email")]
    [Required(ErrorMessage = "Email is a required field.")]
    [MaxLength(255, ErrorMessage = "Email max length is 255.")]
    public string Email { get; set; } = null!;

    [Column("name")]
    [Required(ErrorMessage = "Name is a required field.")]
    [MaxLength(255, ErrorMessage = "Name max length is 255.")]
    public string Name { get; set; } = null!;

    [Column("surname")]
    [Required(ErrorMessage = "Surname is a required field.")]
    [MaxLength(255, ErrorMessage = "Surname max length is 255.")]
    public string Surname { get; set; } = null!;

    [Column("password")]
    [Required(ErrorMessage = "Password is a required field.")]
    [MaxLength(255, ErrorMessage = "Password max length is 255.")]
    public string Password { get; set; } = null!;

    [Column("birth_date")]
    [Required(ErrorMessage = "BirthDate is a required field.")]
    public DateTime BirthDate { get; set; } 

    [Column("status")]
    [Required(ErrorMessage = "Status is a required field.")]
    [MaxLength(50, ErrorMessage = "Status is a required field.")]
    public string Status { get; set; } = UserStatus.Active;
    
    [Column("created_at")]
    [Required(ErrorMessage = "CreatedAt is a required field.")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Column("updated_at")]
    [Required(ErrorMessage = "UpdatedAt is a required field.")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Application>? Applications { get; set; }
}