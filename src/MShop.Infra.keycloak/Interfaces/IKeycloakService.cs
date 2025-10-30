namespace MShop.Infra.Keycloak.Interfaces
{
    public interface IKeycloakService
    {
        Task<bool> CreateUserAsync(
            string name,
            string email,
            string phone,
            string password,
            CancellationToken cancellationToken = default);
    }
}