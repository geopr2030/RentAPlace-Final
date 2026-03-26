using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentAPlace.API.DTOs.Review;
using RentAPlace.API.Helpers;
using RentAPlace.API.Interfaces;

namespace RentAPlace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // GET /api/review/property/{propertyId}
        [HttpGet("property/{propertyId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPropertyReviews(int propertyId)
        {
            try
            {
                var reviews = await _reviewService.GetPropertyReviewsAsync(propertyId);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST /api/review
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = JwtHelper.GetUserIdFromClaims(User);

            try
            {
                var review = await _reviewService.CreateReviewAsync(dto, userId);
                return Ok(new { message = "Review added successfully.", review });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
            }
        }
    }
}
