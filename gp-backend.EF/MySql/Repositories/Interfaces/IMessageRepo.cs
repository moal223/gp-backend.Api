using gp_backend.Core.Models;

namespace gp_backend.EF.MySql.Repositories.Interfaces
{
    public interface IMessageRepo
    {
        Task SaveMessage(Message message);
        Task<List<Message>> GetChatHistory(string userId1, string userId2);
    }
}
