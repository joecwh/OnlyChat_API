using API.Models;
using API.Responses;
using AutoMapper;

namespace API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<SignupRequest, User>();
            CreateMap<User, UserResponse>();
            CreateMap<Message, MessageResponse>();
        }
    }
}
