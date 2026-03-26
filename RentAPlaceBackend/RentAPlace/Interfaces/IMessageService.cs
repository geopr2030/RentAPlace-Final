using RentAPlace.API.DTOs.Message;

namespace RentAPlace.API.Interfaces
{
    public interface IMessageService
    {
        Task<MessageResponseDto> SendMessageAsync(SendMessageDto dto, int senderId);
        Task<List<MessageResponseDto>> GetInboxAsync(int userId);
        Task<List<MessageResponseDto>> GetPropertyMessagesAsync(int propertyId, int ownerId);
        Task<bool> MarkAsReadAsync(int messageId, int userId);
    }
}
