using Newtonsoft.Json;

namespace LibraryApi.Models;

public class Account : AccountParent
{
    [JsonProperty("passwordHash")]
    public byte[]? PasswordHash { get; set; }

    [JsonProperty("passwordSalt")]
    public byte[]? PasswordSalt { get; set; }
}

