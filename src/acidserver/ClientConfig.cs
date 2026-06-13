namespace IdentityServerAspNetIdentity;

public class ClientConfig
{
    public string ClientName { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string[] RedirectUris { get; set; } = [];
    public string[] PostLogoutRedirectUris { get; set; } = [];
}

public class AcidServerConfig
{
    public ClientConfig[] Clients { get; set; } = [];
    public string[] AllowedCorsOrigins { get; set; } = [];
}
