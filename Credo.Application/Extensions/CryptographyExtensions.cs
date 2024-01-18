using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Credo.Application.Extensions;

public static class CryptographyExtensions
{
    public static string HashPassword(this string input)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));

        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}