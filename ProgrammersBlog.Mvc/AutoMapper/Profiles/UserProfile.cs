using AutoMapper;
using ProgrammersBlog.Entities.Concreate;
using ProgrammersBlog.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc.AutoMapper.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<UserAddDto,User>();
        }
    }
}
