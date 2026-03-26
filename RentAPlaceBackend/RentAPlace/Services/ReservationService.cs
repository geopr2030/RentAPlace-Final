using RentAPlace.API.DTOs.Reservation;
using RentAPlace.API.Interfaces;
using RentAPlace.API.Models;

namespace RentAPlace.API.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepo;
        private readonly IPropertyRepository _propertyRepo;
        private readonly IMessageRepository _messageRepo;
        private readonly IUserRepository _userRepo;
        private readonly IEmailService _emailService;

        public ReservationService(IReservationRepository reservationRepo, 
            IPropertyRepository propertyRepo, 
            IMessageRepository messageRepo, 
            IUserRepository userRepo,
            IEmailService emailService)
        {
            _reservationRepo = reservationRepo;
            _propertyRepo = propertyRepo;
            _messageRepo = messageRepo;
            _userRepo = userRepo;
            _emailService = emailService;
        }

        public async Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto, int renterId)
        {
            if (dto.CheckInDate >= dto.CheckOutDate)
                throw new Exception("Check-out date must be after check-in date.");

            if (dto.CheckInDate < DateTime.UtcNow.Date)
                throw new Exception("Check-in date cannot be in the past.");

            var property = await _propertyRepo.GetByIdAsync(dto.PropertyId);
            if (property == null || !property.IsActive)
                throw new Exception("Property not found or unavailable.");

            // make sure no overlap exists
            bool isAvailable = await _reservationRepo.IsPropertyAvailableAsync(
                dto.PropertyId, dto.CheckInDate, dto.CheckOutDate);

            if (!isAvailable)
                throw new Exception("Property is not available for the selected dates.");

            int nights = (dto.CheckOutDate - dto.CheckInDate).Days;
            decimal total = nights * property.PricePerNight;

            var reservation = new Reservation
            {
                PropertyId = dto.PropertyId,
                RenterId = renterId,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                TotalPrice = total,
                Status = "Pending"
            };

            await _reservationRepo.AddAsync(reservation);
            await _reservationRepo.SaveChangesAsync();

            var renter = await _userRepo.GetByIdAsync(renterId);

            // ---- NEW: Admin Notification ----
            try
            {
                var allUsers = await _userRepo.GetAllUsersAsync();
                var admins = allUsers.Where(u => u.Role == "Admin").ToList();

                foreach (var admin in admins)
                {
                    var notification = new Message
                    {
                        PropertyId = property.PropertyId,
                        SenderId = renterId,
                        ReceiverId = admin.UserId,
                        Content = $"New Booking Alert: User {renter?.FullName} (ID: {renterId}) has booked '{property.Title}' from {dto.CheckInDate:yyyy-MM-dd} to {dto.CheckOutDate:yyyy-MM-dd}. Total: ${total}.",
                        SentAt = DateTime.UtcNow
                    };
                    await _messageRepo.AddAsync(notification);
                }
                await _messageRepo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log error but don't fail the reservation creation
                Console.WriteLine($"Failed to send admin notification: {ex.Message}");
            }

            // ---- NEW: Email Notification & System Alert to Owner ----
            try
            {
                var owner = await _userRepo.GetByIdAsync(property.OwnerId);
                if (owner != null)
                {
                    // 1) Send Email Alert
                    if (!string.IsNullOrEmpty(owner.Email))
                    {
                        string subject = $"New Reservation for {property.Title}!";
                        string body = $"Hello {owner.FullName},\n\nYou have a new pending reservation request from {renter?.FullName} for your property '{property.Title}'.\n" +
                                      $"Dates: {dto.CheckInDate:yyyy-MM-dd} to {dto.CheckOutDate:yyyy-MM-dd}\n" +
                                      $"Total Price: ₹{total}\n\n" +
                                      $"Please log in to your owner dashboard to approve or reject this booking.";

                        await _emailService.SendEmailAsync(owner.Email, subject, body);
                    }

                    // 2) Create In-App Notification (Live Bell)
                    var sysNotification = new Message
                    {
                        PropertyId = property.PropertyId,
                        SenderId = renterId,
                        ReceiverId = owner.UserId,
                        Content = $"🛎️ New Booking Request: {renter?.FullName} wants to book '{property.Title}' for ₹{total}. Check your reservations dashboard!",
                        SentAt = DateTime.UtcNow,
                        IsRead = false
                    };
                    await _messageRepo.AddAsync(sysNotification);
                    await _messageRepo.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send owner email: {ex.Message}");
            }

            return MapToDto(reservation, property);
        }

        public async Task<bool> CancelReservationAsync(int reservationId, int renterId)
        {
            var reservation = await _reservationRepo.GetByIdAsync(reservationId);
            if (reservation == null || reservation.RenterId != renterId)
                return false;

            if (reservation.Status == "Cancelled")
                throw new Exception("Reservation is already cancelled.");

            reservation.Status = "Cancelled";
            await _reservationRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ConfirmReservationAsync(int reservationId, int ownerId)
        {
            var reservation = await _reservationRepo.GetByIdAsync(reservationId);
            if (reservation == null)
                return false;

            // only the property owner can confirm
            if (reservation.Property?.OwnerId != ownerId)
                return false;

            reservation.Status = "Confirmed";
            await _reservationRepo.SaveChangesAsync();
            return true;
        }

        public async Task<List<ReservationResponseDto>> GetRenterReservationsAsync(int renterId)
        {
            var reservations = await _reservationRepo.GetByRenterIdAsync(renterId);
            return reservations.Select(r => MapToDto(r, r.Property!)).ToList();
        }

        public async Task<List<ReservationResponseDto>> GetPropertyReservationsAsync(int propertyId, int ownerId)
        {
            var property = await _propertyRepo.GetByIdAsync(propertyId);
            if (property == null || property.OwnerId != ownerId)
                throw new Exception("Property not found or access denied.");

            var reservations = await _reservationRepo.GetByPropertyIdAsync(propertyId);
            return reservations.Select(r => MapToDto(r, r.Property!)).ToList();
        }

        private static ReservationResponseDto MapToDto(Reservation r, Property p)
        {
            return new ReservationResponseDto
            {
                ReservationId = r.ReservationId,
                PropertyId = r.PropertyId,
                PropertyTitle = p.Title,
                RenterName = r.Renter?.FullName ?? "Unknown",
                CheckInDate = r.CheckInDate,
                CheckOutDate = r.CheckOutDate,
                TotalPrice = r.TotalPrice,
                Status = r.Status,
                CreatedAt = r.CreatedAt
            };
        }
    }
}
