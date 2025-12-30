using Backend.DTOs;
using Backend.Entity;


namespace Backend.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse?> GetByIdAsync(Guid uid);
        Task<User?> GetByEmailAsync(string Email);
        Task<User?> GetByUUIDAsync(Guid uuid);
        Task<UserResponse> CreateAsync(RegisterUserDTO user);
        // Task<bool> UpdateAsync(Guid uid, User user);
        // Task<bool> DeleteAsync(Guid uid);
    }
}
