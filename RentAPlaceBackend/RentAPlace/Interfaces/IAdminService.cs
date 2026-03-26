using RentAPlace.API.DTOs.Property;
using RentAPlace.API.Models;

namespace RentAPlace.API.Interfaces
{
    public interface IAdminService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(int userId);
        Task<List<PropertyResponseDto>> GetAllPropertiesAsync();
        Task<bool> DeletePropertyAsync(int propertyId);
    }
}
