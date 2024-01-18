using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs;

public class AuthDto
{
    [Required(ErrorMessage = "Email is a required field.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is a required field.")]
    public string Password { get; set; } = null!;
}