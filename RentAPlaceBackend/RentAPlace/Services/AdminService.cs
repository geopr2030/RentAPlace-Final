using RentAPlace.API.DTOs.Property;
using RentAPlace.API.Interfaces;
using RentAPlace.API.Models;

namespace RentAPlace.API.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepo;
        private readonly IPropertyService _propertyService;
        private readonly IPropertyRepository _propertyRepo;

        public AdminService(IUserRepository userRepo, IPropertyService propertyService, IPropertyRepository propertyRepo)
        {
            _userRepo = userRepo;
            _propertyService = propertyService;
            _propertyRepo = propertyRepo;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepo.GetAllUsersAsync();
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return false;

            await _userRepo.DeleteAsync(user);
            await _userRepo.SaveChangesAsync();
            return true;
        }

        public async Task<List<PropertyResponseDto>> GetAllPropertiesAsync()
        {
            return await _propertyService.GetAllPropertiesAsync();
        }

        public async Task<bool> DeletePropertyAsync(int propertyId)
        {
            var property = await _propertyRepo.GetByIdAsync(propertyId);
            if (property == null) return false;

            await _propertyRepo.DeleteAsync(property);
            await _propertyRepo.SaveChangesAsync();
            return true;
        }
    }
}
