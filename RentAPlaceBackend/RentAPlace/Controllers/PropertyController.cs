using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentAPlace.API.DTOs.Property;
using RentAPlace.API.Helpers;
using RentAPlace.API.Interfaces;

namespace RentAPlace.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        // GET /api/property
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var results = await _propertyService.GetAllPropertiesAsync();
            return Ok(results);
        }

        // GET /api/property/search?location=Dubai&propertyType=Villa&features=Pool
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? location,
            [FromQuery] string? propertyType,
            [FromQuery] string? features,
            [FromQuery] DateTime? checkIn,
            [FromQuery] DateTime? checkOut)
        {
            var results = await _propertyService.SearchPropertiesAsync(location, propertyType, features, checkIn, checkOut);
            return Ok(results);
        }

        // GET /api/property/top-rated?count=5
        [HttpGet("top-rated")]
        public async Task<IActionResult> GetTopRated([FromQuery] int count = 5)
        {
            var results = await _propertyService.GetTopRatedPropertiesAsync(count);
            return Ok(results);
        }

        // GET /api/property/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var property = await _propertyService.GetPropertyByIdAsync(id);
            if (property == null)
                return NotFound(new { message = "Property not found." });

            return Ok(property);
        }

        // GET /api/property/my-listings — Owner only
        [HttpGet("my-listings")]
        [Authorize(Roles = "Owner,Admin")]
        public async Task<IActionResult> GetMyListings()
        {
            int ownerId = JwtHelper.GetUserIdFromClaims(User);
            var props = await _propertyService.GetOwnerPropertiesAsync(ownerId);
            return Ok(props);
        }

        // POST /api/property — Owner only
        // accepts form-data to support image uploads
        [HttpPost]
        [Authorize(Roles = "Owner,Admin")]
        public async Task<IActionResult> Create([FromForm] CreatePropertyDto dto, [FromForm] List<IFormFile>? images)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int ownerId = JwtHelper.GetUserIdFromClaims(User);

            try
            {
                var property = await _propertyService.CreatePropertyAsync(dto, ownerId, images);
                return CreatedAtAction(nameof(GetById), new { id = property.PropertyId }, property);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Owner,Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePropertyDto dto)
        {
            int ownerId = JwtHelper.GetUserIdFromClaims(User);

            var updated = await _propertyService.UpdatePropertyAsync(id, dto, ownerId);

            if (updated == null)
                return NotFound(new { message = "Property not found or you don't have access." });

            return Ok(updated);
        }

        // DELETE /api/property/{id} — Owner only
        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            int ownerId = JwtHelper.GetUserIdFromClaims(User);
            bool deleted = await _propertyService.DeletePropertyAsync(id, ownerId);

            if (!deleted)
                return NotFound(new { message = "Property not found or you don't have access." });

            return Ok(new { message = "Property deleted successfully." });
        }
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllProperties()
        {
            var properties = await _propertyService.GetAllPropertiesAsync();
            return Ok(properties);
        }
    }
}
