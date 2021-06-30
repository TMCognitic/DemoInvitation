namespace DemoInvitation.Infrastructure.Security
{
    public interface ITokenRepository
    {
        string GenerateToken(TokenUser user);
        TokenUser ValidateToken(string token);
    }
}