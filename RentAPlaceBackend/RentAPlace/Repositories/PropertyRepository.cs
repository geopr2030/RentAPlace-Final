using Microsoft.EntityFrameworkCore;
using RentAPlace.API.Data;
using RentAPlace.API.Interfaces;
using RentAPlace.API.Models;

namespace RentAPlace.API.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly AppDbContext _db;

        public PropertyRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Property>> GetAllAsync()
        {
            return await _db.Properties
                .Include(p => p.Owner)
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        public async Task<Property?> GetByIdAsync(int propertyId)
        {
            return await _db.Properties
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.PropertyId == propertyId);
        }

        public async Task<List<Property>> GetByOwnerIdAsync(int ownerId)
        {
            return await _db.Properties
                .Include(p => p.Owner)
                .Where(p => p.OwnerId == ownerId && p.IsActive)
                .ToListAsync();
        }

        public async Task<List<Property>> SearchAsync(string? location, string? propertyType, string? features, DateTime? checkIn, DateTime? checkOut)
        {
            var query = _db.Properties
                .Include(p => p.Owner)
                .Where(p => p.IsActive)
                .AsQueryable();

            if (!string.IsNullOrEmpty(location) && location.Trim() != "")
            {
                var loc = location.Trim().ToLower();
                query = query.Where(p => p.Location.ToLower().Contains(loc));
            }

            if (!string.IsNullOrEmpty(propertyType) && propertyType.Trim() != "" && propertyType != "Any Type")
            {
                var pt = propertyType.Trim().ToLower();
                query = query.Where(p => p.PropertyType.ToLower().Contains(pt));
            }

            if (!string.IsNullOrEmpty(features) && features.Trim() != "")
            {
                var feat = features.Trim().ToLower();
                query = query.Where(p => p.Features != null && p.Features.ToLower().Contains(feat));
            }

            if (checkIn.HasValue && checkOut.HasValue)
            {
                query = query.Where(p => !_db.Reservations.Any(r =>
                    r.PropertyId == p.PropertyId
                    && r.Status != "Cancelled"
                    && r.CheckInDate < checkOut.Value
                    && r.CheckOutDate > checkIn.Value));
            }

            return await query.ToListAsync();
        }

        public async Task AddAsync(Property property)
        {
            await _db.Properties.AddAsync(property);
        }

        public async Task UpdateAsync(Property property)
        {
            _db.Properties.Update(property);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Property property)
        {
            _db.Properties.Remove(property);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}