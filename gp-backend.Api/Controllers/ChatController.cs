using gp_backend.EF.MySql.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace gp_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMessageRepo _service;

        public ChatController(IMessageRepo service)
        {
            _service = service;
        }
        [HttpGet("history")]
        public async Task<IActionResult> GetMessages(string sen, string reci)
        {
            return Ok(await _service.GetChatHistory(sen, reci));
        }
    }
}
