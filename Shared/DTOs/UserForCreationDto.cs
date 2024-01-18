using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs;

public class UserForCreationDto
{
    [Required(ErrorMessage = "PrivateNumber is a required field.")]
    [MaxLength(9, ErrorMessage = "PrivateNumber max length is 9.")]
    public string PrivateNumber { get; set; } = null!;

    [Required(ErrorMessage = "Email is a required field.")]
    [MaxLength(255, ErrorMessage = "Email max length is 255.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Name is a required field.")]
    [MaxLength(255, ErrorMessage = "Name max length is 255.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Surname is a required field.")]
    [MaxLength(255, ErrorMessage = "Surname max length is 255.")]
    public string Surname { get; set; } = null!;

    [Required(ErrorMessage = "Password is a required field.")]
    [MaxLength(255, ErrorMessage = "Password max length is 255.")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "BirthDate is a required field.")]
    public DateTime BirthDate { get; set; } 
    
    [MaxLength(50, ErrorMessage = "Status is a required field.")]
    public string? Status { get; set; }
}