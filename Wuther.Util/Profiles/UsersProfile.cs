using AutoMapper;
using Wuther.Entities.Models;
using Wuther.Util.Enums;
using Wuther.Util.Models;

namespace Wuther.Util.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<Users, UserDto>()
                .ForMember(dest => dest.Test,
                opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.GenderDisplay,
                opt => opt.MapFrom(src =>src.Sex == null? Gender.None.ToString() : ((Gender)src.Sex).ToString()));

            CreateMap<UserAddDto, Users>();
            CreateMap<UserUpdateDto, Users>();
            CreateMap<Users, UserUpdateDto>();
            CreateMap<Users, UserFullDto>().ForMember(dest => dest.GenderDisplay,
                opt => opt.MapFrom(src => src.Sex == null ? Gender.None.ToString() : ((Gender)src.Sex).ToString()));

            CreateMap<UserAddWithWrittenOffTimeDto, Users>();
        }
    }
}
