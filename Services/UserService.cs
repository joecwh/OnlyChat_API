using API.Data;
using API.Models;
using API.Responses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public UserService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<bool> DeleteMessage([FromRoute] Guid MessageId)
        {
            try
            {
                var message = await _dataContext.Messages.FindAsync(MessageId);
                if (message != null)
                {
                    _dataContext.Messages.Remove(message);
                    await _dataContext.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            try
            {
                var user = (await _dataContext.Users.FindAsync(userId));
                if (user != null)
                {
                    _dataContext.Remove(user);
                    await _dataContext.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public async Task<List<MessageResponse>> GetAllMessages()
        {
            try
            {
                var message = await _dataContext.Messages.ToListAsync();
                if(message != null)
                {
                    List<MessageResponse> messageResponse = new List<MessageResponse>();
                    foreach (var ms in message)
                    {
                        var user = await _dataContext.Users.FindAsync(ms.UserId);
                        MessageResponse response = _mapper.Map<MessageResponse>(ms);
                        response.Username = user == null ? "Deleted User" : user.UserName;
                        messageResponse.Add(response);
                    }
                    return messageResponse;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<List<UserResponse>> GetAllUsers()
        {
            try
            {
                var users = await _dataContext.Users
                .ProjectTo<UserResponse>(_mapper.ConfigurationProvider)
                .ToListAsync();

                return users;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<Message> GetMessage(Guid MessageId)
        {
            return await _dataContext.Messages.FindAsync(MessageId);
        }

        public async Task<UserResponse> GetUser(Guid id)
        {
            return _mapper.Map<UserResponse>(await _dataContext.Users.FindAsync(id));
        }

        public async Task<bool> UpdateMessage([FromRoute] Guid MessageId, Message Message)
        {
            try
            {
                var message = await _dataContext.Messages.FindAsync(MessageId);
                if (message != null)
                {
                    message.Text = Message.Text;
                    await _dataContext.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public async Task<bool> UpdateUser(Guid userId, User User)
        {
            try
            {
                var user = await _dataContext.Users.FindAsync(userId);
                if (user != null)
                {
                    user.UserName = User.UserName;
                    user.Firstname = User.Firstname;
                    user.Lastname = User.Lastname;
                    user.Email = User.Email;
                    user.PhoneNumber = User.PhoneNumber;

                    await _dataContext.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public async Task<bool> WriteMessage(Message Message)
        {
            try
            {
                Message.Id = new Guid();
                await _dataContext.Messages.AddAsync(Message);
                await _dataContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
    }
}
