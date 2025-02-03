using gp_backend.Core.Models;
using gp_backend.EF.MySql.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;

namespace gp_backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
       private readonly IMessageRepo _service;
        private readonly IWebHostEnvironment _env;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IMessageRepo service, IWebHostEnvironment env, IHubContext<ChatHub> hubContext)
        {
            _env = env;
            _hubContext = hubContext;
            _service = service;
        }
        [HttpGet("history")]
       public async Task<IActionResult> GetMessages(string sen, string reci)
       {
           return Ok(await _service.GetChatHistory(sen, reci));
       }
        [HttpPost("file-upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] string senderId, [FromForm] string receiverId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);


            string fileName = $"{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string fileUrl = $"/uploads/{fileName}";
            var msg = new Message
            {
                SenderId = senderId,
                RecipientId = receiverId,
                FileUrl = fileUrl,
                FileName = fileName,
                FilePath = $"SkinScan/{fileName}",
                Type = "image",
                Timestamp = DateTime.Now
            };

            await _service.SaveMessage(msg);

            // Notify Receiver via SignalR

            await _hubContext.Clients.Group(receiverId).SendAsync("ReceiveFile", new
            {
                Sender = senderId,
                RecipientId = receiverId,
                FilePath = $"SkinScan/{fileName}",
                FileUrl = fileUrl,
                FileName = fileName,
                Type = "image"
            });

            return Ok(new { FileUrl = fileUrl });
        }
    }
}
