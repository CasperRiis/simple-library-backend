using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Entities;

public class BaseAccount
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    public bool IsAdmin { get; set; } = false;
}