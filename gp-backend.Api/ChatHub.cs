using gp_backend.Core.Models;
using gp_backend.EF.MySql.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace gp_backend.Api
{
    public class ChatHub : Hub
    {
        private readonly IMessageRepo _messageService;
        private static readonly ConcurrentDictionary<string, string> _connections = new();
        private readonly UserManager<ApplicationUser> _userManager;
        public ChatHub(IMessageRepo messageService, UserManager<ApplicationUser> userManager)
        {
            _messageService = messageService;
            _userManager = userManager;
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext().Request.Query["uid"];
            var user = await getUserById(userId);
            if (user != null)
                _connections[userId] = Context.ConnectionId;

            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = _connections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (userId != null)
            {
                _connections.TryRemove(userId, out _);
            }

            return base.OnDisconnectedAsync(exception);
        }
        [HubMethodName("sendMessage")]
        public async Task SendMessage(string senderId, string recipientId, string message)
        {
            var msg = new Message
            {
                SenderId = senderId,
                RecipientId = recipientId,
                Content = message,
                Timestamp = DateTime.Now
            };

            await _messageService.SaveMessage(msg);

            if (_connections.TryGetValue(recipientId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("receiveMessage", new
                {
                    Content = msg.Content,
                    senderId = senderId,
                    recipientId = recipientId
                });
            }
            if (_connections.TryGetValue(senderId, out var senderConId))
            {
                await Clients.Client(senderConId).SendAsync("receiveMessage", new
                {
                    Content = msg.Content,
                    senderId = senderId,
                    recipientId = recipientId
                });
            }
        }


        #region Methods
        private async Task<ApplicationUser> getUserById(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
