using System.ComponentModel.DataAnnotations;

namespace EcoAlert.DTOs
{
    public class RegisterDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? DisplayName { get; set; }
    }

    public class LoginDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class AuthResponseDto
    {
        public required string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public required UserDto User { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public string? DisplayName { get; set; }
        public string? Role { get; set; }
    }
}