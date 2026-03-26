using RentAPlace.API.DTOs.Review;
using RentAPlace.API.Interfaces;
using RentAPlace.API.Models;

namespace RentAPlace.API.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IPropertyRepository _propertyRepo;

        public ReviewService(IReviewRepository reviewRepo, IPropertyRepository propertyRepo)
        {
            _reviewRepo = reviewRepo;
            _propertyRepo = propertyRepo;
        }

        public async Task<List<ReviewResponseDto>> GetPropertyReviewsAsync(int propertyId)
        {
            var property = await _propertyRepo.GetByIdAsync(propertyId);
            if (property == null || !property.IsActive)
                throw new Exception("Property not found or is inactive.");

            var reviews = await _reviewRepo.GetByPropertyIdAsync(propertyId);
            
            return reviews.Select(r => new ReviewResponseDto
            {
                ReviewId = r.ReviewId,
                PropertyId = r.PropertyId,
                ReviewerId = r.ReviewerId,
                ReviewerName = r.Reviewer?.FullName ?? "Unknown",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task<ReviewResponseDto> CreateReviewAsync(CreateReviewDto dto, int reviewerId)
        {
            var property = await _propertyRepo.GetByIdAsync(dto.PropertyId);
            if (property == null || !property.IsActive)
                throw new Exception("Property not found or is inactive.");

            // Check if user already reviewed this property (optional rule, but good for anti-spam)
            var existingReview = await _reviewRepo.GetByUserAndPropertyAsync(reviewerId, dto.PropertyId);
            if (existingReview != null)
                throw new Exception("You have already reviewed this property.");

            var review = new Review
            {
                PropertyId = dto.PropertyId,
                ReviewerId = reviewerId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepo.AddAsync(review);
            await _reviewRepo.SaveChangesAsync();

            // Return the created review DTO
            var createdReview = await _reviewRepo.GetByUserAndPropertyAsync(reviewerId, dto.PropertyId);
            return new ReviewResponseDto
            {
                ReviewId = createdReview!.ReviewId,
                PropertyId = createdReview.PropertyId,
                ReviewerId = createdReview.ReviewerId,
                ReviewerName = createdReview.Reviewer?.FullName ?? "Unknown",
                Rating = createdReview.Rating,
                Comment = createdReview.Comment,
                CreatedAt = createdReview.CreatedAt
            };
        }
    }
}
