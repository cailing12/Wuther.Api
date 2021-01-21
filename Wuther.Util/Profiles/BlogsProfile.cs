using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wuther.Entities.Models;
using Wuther.Util.Models;

namespace Wuther.Util.Profiles
{
    public class BlogsProfile: Profile
    {
        public BlogsProfile()
        {
            CreateMap<Blogs, BlogDto>();

            CreateMap<BlogDto, Blogs>();

            CreateMap<Blogs, BlogFullDto>();

            CreateMap<BlogFullDto, Blogs>();
        }
    }
}
