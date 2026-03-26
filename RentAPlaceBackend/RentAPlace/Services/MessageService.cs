using RentAPlace.API.DTOs.Message;
using RentAPlace.API.Interfaces;
using RentAPlace.API.Models;

namespace RentAPlace.API.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepo;
        private readonly IPropertyRepository _propertyRepo;

        public MessageService(IMessageRepository messageRepo, IPropertyRepository propertyRepo)
        {
            _messageRepo = messageRepo;
            _propertyRepo = propertyRepo;
        }

        public async Task<MessageResponseDto> SendMessageAsync(SendMessageDto dto, int senderId)
        {
            var property = await _propertyRepo.GetByIdAsync(dto.PropertyId);
            if (property == null)
                throw new Exception("Property not found.");

            var message = new Message
            {
                PropertyId = dto.PropertyId,
                SenderId = senderId,
                ReceiverId = dto.ReceiverId,
                Content = dto.Content,
                ParentMessageId = dto.ParentMessageId
            };

            await _messageRepo.AddAsync(message);
            await _messageRepo.SaveChangesAsync();

            // reload to get nav props
            var saved = await _messageRepo.GetByIdAsync(message.MessageId);

            return new MessageResponseDto
            {
                MessageId = saved!.MessageId,
                PropertyId = saved.PropertyId,
                PropertyTitle = property.Title,
                SenderId = saved.SenderId,
                SenderName = saved.Sender?.FullName ?? "Unknown",
                ReceiverId = saved.ReceiverId,
                ReceiverName = saved.Receiver?.FullName ?? "Unknown",
                Content = saved.Content,
                ParentMessageId = saved.ParentMessageId,
                SentAt = saved.SentAt,
                IsRead = saved.IsRead
            };
        }

        public async Task<List<MessageResponseDto>> GetInboxAsync(int userId)
        {
            var messages = await _messageRepo.GetInboxAsync(userId);
            return messages.Select(m => new MessageResponseDto
            {
                MessageId = m.MessageId,
                PropertyId = m.PropertyId,
                PropertyTitle = m.Property?.Title ?? "Unknown",
                SenderId = m.SenderId,
                SenderName = m.Sender?.FullName ?? "Unknown",
                ReceiverId = m.ReceiverId,
                ReceiverName = m.Receiver?.FullName ?? "Unknown",
                Content = m.Content,
                ParentMessageId = m.ParentMessageId,
                SentAt = m.SentAt,
                IsRead = m.IsRead
            }).ToList();
        }

        public async Task<List<MessageResponseDto>> GetPropertyMessagesAsync(int propertyId, int ownerId)
        {
            var property = await _propertyRepo.GetByIdAsync(propertyId);
            if (property == null || property.OwnerId != ownerId)
                throw new Exception("Property not found or access denied.");

            var messages = await _messageRepo.GetByPropertyIdAsync(propertyId);
            return messages.Select(m => new MessageResponseDto
            {
                MessageId = m.MessageId,
                PropertyId = m.PropertyId,
                PropertyTitle = property.Title,
                SenderId = m.SenderId,
                SenderName = m.Sender?.FullName ?? "Unknown",
                ReceiverId = m.ReceiverId,
                ReceiverName = m.Receiver?.FullName ?? "Unknown",
                Content = m.Content,
                ParentMessageId = m.ParentMessageId,
                SentAt = m.SentAt,
                IsRead = m.IsRead
            }).ToList();
        }

        public async Task<bool> MarkAsReadAsync(int messageId, int userId)
        {
            var message = await _messageRepo.GetByIdAsync(messageId);
            if (message == null || message.ReceiverId != userId)
                return false;

            message.IsRead = true;
            await _messageRepo.SaveChangesAsync();
            return true;
        }
    }
}
