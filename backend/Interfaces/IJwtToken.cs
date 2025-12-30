using Backend.Entity;

namespace Backend.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }

}
