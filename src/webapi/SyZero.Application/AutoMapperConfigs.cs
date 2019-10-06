using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using SyZero.Domain.Entities;

namespace SyZero.Application
{
    public class AutoMapperConfigs:Profile
    {
        //添加你的实体映射关系.
        public AutoMapperConfigs()
        {


            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

       
        }

    }
}
