using RentAPlace.API.DTOs.Auth;
using RentAPlace.API.Helpers;
using RentAPlace.API.Interfaces;
using RentAPlace.API.Models;

namespace RentAPlace.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IUserRepository userRepo, JwtHelper jwtHelper)
        {
            _userRepo = userRepo;
            _jwtHelper = jwtHelper;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            // make sure email isn't already taken
            var existing = await _userRepo.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new Exception("Email is already registered.");

            // only allow Renter or Owner from public registration
            var allowedRoles = new[] { "Renter", "Owner" };
            if (!allowedRoles.Contains(dto.Role))
                throw new Exception("Invalid role. Use Renter or Owner.");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                PhoneNumber = dto.PhoneNumber
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();

            return _jwtHelper.GenerateToken(user);
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("Invalid email or password.");

            bool passwordOk = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!passwordOk)
                throw new Exception("Invalid email or password.");

            return _jwtHelper.GenerateToken(user);
        }
    }
}
