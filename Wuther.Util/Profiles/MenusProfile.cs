using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Wuther.Entities.Models;
using Wuther.Util.Enums;
using Wuther.Util.Models;

namespace Wuther.Util.Profiles
{
    public class MenusProfile: Profile
    {
        public MenusProfile()
        {
            CreateMap<Menus, MenuDto>()
                .ForMember(dest => dest.PositionDisPlay,
                opt => opt.MapFrom(src => src.Position == null ? MenuPosition.others.ToString() : ((MenuPosition)src.Position).ToString()));
            CreateMap<MenuDto, Menus>();
            CreateMap<MenuAddDto, Menus>();
            CreateMap<MenuUpdateDto, Menus>();
            CreateMap<Menus, MenuUpdateDto>();
            CreateMap<Menus, MenuFullDto>().ForMember(dest => dest.Position,
                opt => opt.MapFrom(src => src.Position == null ? MenuPosition.others.ToString() : ((MenuPosition)src.Position).ToString()));
        }
    }
}
