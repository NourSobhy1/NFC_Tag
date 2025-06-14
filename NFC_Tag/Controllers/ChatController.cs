using Microsoft.AspNetCore.Mvc;
using NFC.Application.DTOs;
using NFC.Application.Interfaces.IServices;

namespace NFC_Tag.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequestDto request)
        {
            var answer = await _chatService.AskAsync(request.Message);
            return Ok(answer);
        }

    }
}
