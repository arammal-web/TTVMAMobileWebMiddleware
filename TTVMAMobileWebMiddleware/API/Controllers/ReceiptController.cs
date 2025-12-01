using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.RequestUtility;
using TTVMAMobileWebMiddleware.Application.Interfaces;
using TTVMAMobileWebMiddleware.Domain.Entities.DLS;
using TTVMAMobileWebMiddleware.Domain.Views;

namespace TTVMAMobileWebMiddleware.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ReceiptController : ControllerBase
    {
        private readonly IReceiptService _service;

        /// <summary>
        /// Constructor for the ReceiptController class.
        /// </summary>
        /// <param name="service"></param>
        public ReceiptController(IReceiptService service)
        {
            _service = service;
        }

       /// <summary>
        /// Creates a new receipt with details.`
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("create-with-details")]
        public async Task<IActionResult> CreateWithDetails([FromBody] ReceiptWithDetailRequest request)
        {
            if (!ModelState.IsValid) return BadRequest("Not Valid");

            var result = await _service.CreateWithDetailsAsync(request);
            // return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            return Ok(result);
        }
         
    }
}
