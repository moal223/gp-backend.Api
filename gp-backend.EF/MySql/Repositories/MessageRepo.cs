
using gp_backend.Core.Models;
using gp_backend.EF.MySql.Data;
using gp_backend.EF.MySql.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace gp_backend.EF.MySql.Repositories
{
    public class MessageRepo : IMessageRepo
    {
        private readonly MySqlDbContext _context; // Your EF Core context

        public MessageRepo(MySqlDbContext context)
        {
            _context = context;
        }

        public async Task SaveMessage(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Message>> GetChatHistory(string userId1, string userId2)
        {
            var history = await _context.Messages
                .Where(m => (m.SenderId == userId1 && m.RecipientId == userId2) ||
                             (m.SenderId == userId2 && m.RecipientId == userId1))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
            return history;
        }
    }
}
