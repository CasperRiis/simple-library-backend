using Newtonsoft.Json;

namespace LibraryApi.Models;

public class AccountParent
{
    [JsonProperty("accountId")]
    public int AccountId { get; set; }

    [JsonProperty("username")]
    public string? Username { get; set; }

    [JsonProperty("isAdmin")]
    public bool IsAdmin { get; set; }
}

