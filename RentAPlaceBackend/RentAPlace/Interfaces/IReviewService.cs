using RentAPlace.API.DTOs.Review;

namespace RentAPlace.API.Interfaces
{
    public interface IReviewService
    {
        Task<List<ReviewResponseDto>> GetPropertyReviewsAsync(int propertyId);
        Task<ReviewResponseDto> CreateReviewAsync(CreateReviewDto dto, int reviewerId);
    }
}
