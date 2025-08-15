using AutoMapper;
using Yemekhane.Business.DTOs;
using Yemekhane.Entities;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();

        CreateMap<UserDto, User>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.PasswordHash, opt => opt.Ignore()) 
            .ForMember(d => d.Ratings, opt => opt.Ignore()) // navigation
            .ForAllMembers(opt => opt.Condition((src, dest, val) => val != null));
    }
}
