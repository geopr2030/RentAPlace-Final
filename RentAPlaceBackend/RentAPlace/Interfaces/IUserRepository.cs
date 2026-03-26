using RentAPlace.API.Models;

namespace RentAPlace.API.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int userId);
        Task<List<User>> GetAllUsersAsync();
        Task AddAsync(User user);
        Task DeleteAsync(User user);
        Task SaveChangesAsync();
    }
}
