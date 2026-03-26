using RentAPlace.API.Models;

namespace RentAPlace.API.Interfaces
{
    public interface IPropertyRepository
    {
        Task<List<Property>> GetAllAsync();
        Task<Property?> GetByIdAsync(int propertyId);
        Task<List<Property>> GetByOwnerIdAsync(int ownerId);
        Task<List<Property>> SearchAsync(string? location, string? propertyType, string? features, DateTime? checkIn, DateTime? checkOut);
        Task AddAsync(Property property);
        Task UpdateAsync(Property property);
        Task DeleteAsync(Property property);
        Task SaveChangesAsync();
    }
}
