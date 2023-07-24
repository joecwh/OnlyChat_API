using API.Models;
using API.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public interface IUserService
    {
        Task<List<UserResponse>> GetAllUsers();
        Task<UserResponse> GetUser(Guid id);
        Task<bool> UpdateUser(Guid userId, User User);
        Task<bool> DeleteUser(Guid userId);

        Task<List<MessageResponse>> GetAllMessages();
        Task<Message> GetMessage(Guid MessageId);
        Task<bool> WriteMessage(Message Message);
        Task<bool> UpdateMessage([FromRoute] Guid MessageId, Message Message);
        Task<bool> DeleteMessage([FromRoute] Guid MessageId);
    }
}
