namespace MShop.Infra.Keycloak.Config
{
    public class KeycloakSettings
    {
        public string AuthServerUrl { get; set; } = string.Empty;
        public string Realm { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}