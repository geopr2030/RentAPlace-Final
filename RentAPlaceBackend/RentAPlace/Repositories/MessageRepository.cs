using Microsoft.EntityFrameworkCore;
using RentAPlace.API.Data;
using RentAPlace.API.Interfaces;
using RentAPlace.API.Models;

namespace RentAPlace.API.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _db;

        public MessageRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Message>> GetByPropertyIdAsync(int propertyId)
        {
            return await _db.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Include(m => m.Property)
                .Where(m => m.PropertyId == propertyId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<List<Message>> GetInboxAsync(int userId)
        {
            return await _db.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Include(m => m.Property)
                .Where(m => m.ReceiverId == userId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
        }

        public async Task AddAsync(Message message)
        {
            await _db.Messages.AddAsync(message);
        }

        public async Task<Message?> GetByIdAsync(int messageId)
        {
            return await _db.Messages.FindAsync(messageId);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
