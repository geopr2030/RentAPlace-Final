using RentAPlace.API.Models;

namespace RentAPlace.API.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetByPropertyIdAsync(int propertyId);
        Task<Review?> GetByUserAndPropertyAsync(int userId, int propertyId);
        Task AddAsync(Review review);
        Task SaveChangesAsync();
    }
}
