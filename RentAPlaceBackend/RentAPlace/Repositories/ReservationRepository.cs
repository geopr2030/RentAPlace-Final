using Microsoft.EntityFrameworkCore;
using RentAPlace.API.Data;
using RentAPlace.API.Interfaces;
using RentAPlace.API.Models;

namespace RentAPlace.API.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _db;

        public ReservationRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Reservation>> GetByRenterIdAsync(int renterId)
        {
            return await _db.Reservations
                .Include(r => r.Property)
                .Include(r => r.Renter)
                .Where(r => r.RenterId == renterId)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetByPropertyIdAsync(int propertyId)
        {
            return await _db.Reservations
                .Include(r => r.Property)
                .Include(r => r.Renter)
                .Where(r => r.PropertyId == propertyId)
                .ToListAsync();
        }

        public async Task<Reservation?> GetByIdAsync(int reservationId)
        {
            return await _db.Reservations
                .Include(r => r.Property)
                .Include(r => r.Renter)
                .FirstOrDefaultAsync(r => r.ReservationId == reservationId);
        }

        // check if property is free between the given dates
        public async Task<bool> IsPropertyAvailableAsync(int propertyId, DateTime checkIn, DateTime checkOut)
        {
            var hasConflict = await _db.Reservations
                .Where(r => r.PropertyId == propertyId
                    && r.Status != "Cancelled"
                    && r.CheckInDate < checkOut
                    && r.CheckOutDate > checkIn)
                .AnyAsync();

            return !hasConflict;
        }

        public async Task AddAsync(Reservation reservation)
        {
            await _db.Reservations.AddAsync(reservation);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
