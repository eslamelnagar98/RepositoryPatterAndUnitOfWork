using AutoMapper;
using RepositoryPatterAndUnitOfWork.Domain.Entities;
using RepositoryPatterAndUnitOfWork.DTOs;
using System.Linq;

namespace RepositoryPatterAndUnitOfWork.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                     .ForMember(ud => ud.FullName, u => u.MapFrom(u => $"{u.FirstName} {u.LastName}"));

            CreateMap<UserDto, User>()
                     .ForMember(ud => ud.FirstName, u => u.MapFrom(u => u.FullName.Substring(0, u.FullName.Trim().IndexOfAny(new char[] { ' ' },0))))
                     .ForMember(ud => ud.LastName, u => u.MapFrom(u => u.FullName.Substring(u.FullName.Trim().IndexOfAny(new char[] { ' ' }, 0)+1)));
        }
    }
}
