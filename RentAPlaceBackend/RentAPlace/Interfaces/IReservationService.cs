using RentAPlace.API.DTOs.Reservation;

namespace RentAPlace.API.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationResponseDto> CreateReservationAsync(CreateReservationDto dto, int renterId);
        Task<bool> CancelReservationAsync(int reservationId, int renterId);
        Task<bool> ConfirmReservationAsync(int reservationId, int ownerId);
        Task<List<ReservationResponseDto>> GetRenterReservationsAsync(int renterId);
        Task<List<ReservationResponseDto>> GetPropertyReservationsAsync(int propertyId, int ownerId);
    }
}
