using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentAPlace.API.DTOs.Message;
using RentAPlace.API.Helpers;
using RentAPlace.API.Interfaces;

namespace RentAPlace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // POST /api/message — send or reply to a message
        [HttpPost]
        public async Task<IActionResult> Send([FromBody] SendMessageDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int senderId = JwtHelper.GetUserIdFromClaims(User);

            try
            {
                var message = await _messageService.SendMessageAsync(dto, senderId);
                return Ok(new { message = "Message sent.", data = message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET /api/message/inbox — see messages received
        [HttpGet("inbox")]
        public async Task<IActionResult> GetInbox()
        {
            int userId = JwtHelper.GetUserIdFromClaims(User);
            var messages = await _messageService.GetInboxAsync(userId);
            return Ok(messages);
        }

        // GET /api/message/property/{propertyId} — Owner views all messages for their property
        [HttpGet("property/{propertyId}")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> GetPropertyMessages(int propertyId)
        {
            int ownerId = JwtHelper.GetUserIdFromClaims(User);

            try
            {
                var messages = await _messageService.GetPropertyMessagesAsync(propertyId, ownerId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT /api/message/{id}/read
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            int userId = JwtHelper.GetUserIdFromClaims(User);
            var success = await _messageService.MarkAsReadAsync(id, userId);
            if (!success) return BadRequest();
            return Ok();
        }
    }
}
