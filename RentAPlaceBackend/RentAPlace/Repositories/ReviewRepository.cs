using Microsoft.EntityFrameworkCore;
using RentAPlace.API.Data;
using RentAPlace.API.Interfaces;
using RentAPlace.API.Models;

namespace RentAPlace.API.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _db;

        public ReviewRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Review>> GetByPropertyIdAsync(int propertyId)
        {
            return await _db.Reviews
                .Include(r => r.Reviewer)
                .Where(r => r.PropertyId == propertyId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Review?> GetByUserAndPropertyAsync(int userId, int propertyId)
        {
            return await _db.Reviews
                .Include(r => r.Reviewer)
                .FirstOrDefaultAsync(r => r.ReviewerId == userId && r.PropertyId == propertyId);
        }

        public async Task AddAsync(Review review)
        {
            await _db.Reviews.AddAsync(review);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
