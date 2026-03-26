using RentAPlace.API.Models;

namespace RentAPlace.API.Interfaces
{
    public interface IReservationRepository
    {
        Task<List<Reservation>> GetByRenterIdAsync(int renterId);
        Task<List<Reservation>> GetByPropertyIdAsync(int propertyId);
        Task<Reservation?> GetByIdAsync(int reservationId);
        Task<bool> IsPropertyAvailableAsync(int propertyId, DateTime checkIn, DateTime checkOut);
        Task AddAsync(Reservation reservation);
        Task SaveChangesAsync();
    }
}
