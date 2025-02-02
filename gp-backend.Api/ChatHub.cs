using gp_backend.Api.Dtos.Chat;
using gp_backend.Core.Models;
using gp_backend.EF.MySql.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            {
                _connections[userId] = Context.ConnectionId;
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task<Task> OnDisconnectedAsync(Exception? exception)
        {
            var userId = _connections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (userId != null)
            {
                _connections.TryRemove(userId, out _);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId); // Remove user from the group
            }

            return base.OnDisconnectedAsync(exception);
        }
        [HubMethodName("sendMessage")]
        public async Task SendMessage(SendFileDto message)
        {
            var msg = new Message
            {
                SenderId = message.SenderId,
                RecipientId = message.RecipientId,
                Content = message.Content,
                Type = "text",
                Timestamp = DateTime.Now
            };

            await _messageService.SaveMessage(msg);

            if (_connections.TryGetValue(msg.RecipientId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("receiveMessage", new
                {
                    Content = msg.Content,
                    senderId = msg.SenderId,
                    recipientId = msg.RecipientId,
                    type = "text"
                });
            }
            if (_connections.TryGetValue(msg.SenderId, out var senderConId))
            {
                await Clients.Client(senderConId).SendAsync("receiveMessage", new
                {
                    Content = msg.Content,
                    senderId = msg.SenderId,
                    recipientId = msg.RecipientId,
                    type = "text"
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
