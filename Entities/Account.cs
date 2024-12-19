using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Entities;

public class Account : BaseAccount
{
    [Required]
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();

    [Required]
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
}