using RentAPlace.API.Models;

namespace RentAPlace.API.Interfaces
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetByPropertyIdAsync(int propertyId);
        Task<List<Message>> GetInboxAsync(int userId);
        Task AddAsync(Message message);
        Task<Message?> GetByIdAsync(int messageId);
        Task SaveChangesAsync();
    }
}
