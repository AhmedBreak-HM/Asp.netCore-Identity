using Asp.netCore_Identity.Dtos;
using Asp.netCore_Identity.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netCore_Identity.Helper
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserForCreateDto, User>();
            CreateMap<User, UserForReturnDto > ();

        }
    }
}
