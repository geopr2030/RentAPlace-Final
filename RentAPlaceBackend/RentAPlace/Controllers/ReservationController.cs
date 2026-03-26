using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentAPlace.API.DTOs.Reservation;
using RentAPlace.API.Helpers;
using RentAPlace.API.Interfaces;

namespace RentAPlace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // POST /api/reservation — Renter only
        [HttpPost]
        [Authorize(Roles = "Renter")]
        public async Task<IActionResult> Create([FromBody] CreateReservationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int renterId = JwtHelper.GetUserIdFromClaims(User);

            try
            {
                var reservation = await _reservationService.CreateReservationAsync(dto, renterId);
                return Ok(new { message = "Reservation created successfully.", reservation });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET /api/reservation/my-reservations — Renter
        [HttpGet("my-reservations")]
        [Authorize(Roles = "Renter")]
        public async Task<IActionResult> GetMyReservations()
        {
            int renterId = JwtHelper.GetUserIdFromClaims(User);
            var reservations = await _reservationService.GetRenterReservationsAsync(renterId);
            return Ok(reservations);
        }

        // PUT /api/reservation/{id}/cancel — Renter
        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "Renter")]
        public async Task<IActionResult> Cancel(int id)
        {
            int renterId = JwtHelper.GetUserIdFromClaims(User);

            try
            {
                bool cancelled = await _reservationService.CancelReservationAsync(id, renterId);
                if (!cancelled)
                    return NotFound(new { message = "Reservation not found or access denied." });

                return Ok(new { message = "Reservation cancelled." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET /api/reservation/property/{propertyId} — Owner
        [HttpGet("property/{propertyId}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> GetPropertyReservations(int propertyId)
        {
            int ownerId = JwtHelper.GetUserIdFromClaims(User);

            try
            {
                var reservations = await _reservationService.GetPropertyReservationsAsync(propertyId, ownerId);
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT /api/reservation/{id}/confirm — Owner
        [HttpPut("{id}/confirm")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Confirm(int id)
        {
            int ownerId = JwtHelper.GetUserIdFromClaims(User);
            bool confirmed = await _reservationService.ConfirmReservationAsync(id, ownerId);

            if (!confirmed)
                return NotFound(new { message = "Reservation not found or access denied." });

            return Ok(new { message = "Reservation confirmed." });
        }
    }
}
