namespace MShop.Infra.Keycloak.Interfaces
{
    public interface IKeycloakUserService
    {
        Task<bool> CreateUserAsync(
            string name,
            string email,
            string phone,
            string password,
            CancellationToken cancellationToken = default);
    }
}