using AutoMapper;
using YourPetAPI.Database;
using YourPetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using YourPetAPI.Models.DTOs.User;
using YourPetAPI.Models.DTOs;

namespace YourPetAPI.Helpers
{
    public class AutoMapperHelper : Profile
    {
        public AutoMapperHelper()
        {
            CreateMap<User, UserDto>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.Email, y => y.MapFrom(z => z.Email))
                .ForMember(x => x.RegisterDate, y => y.MapFrom(z => z.RegisterDate));

            CreateMap<UserDto, User>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.Email, y => y.MapFrom(z => z.Email))
                .ForMember(x => x.RegisterDate, y => y.MapFrom(z => z.RegisterDate));

            CreateMap<RegisterUserDto, User>()
                .ForMember(x => x.Email, y => y.MapFrom(z => z.Email));


            CreateMap<User, RegisterUserDto>()
                .ForMember(x => x.Email, y => y.MapFrom(z => z.Email));


            CreateMap<LoginDto, User>()
                .ForMember(x => x.Email, y => y.MapFrom(z => z.Email))
                .ForMember(x => x.PasswordHash, y => y.MapFrom(z => z.Password));

            CreateMap<User, LoginDto>()
               .ForMember(x => x.Email, y => y.MapFrom(z => z.Email))
               .ForMember(x => x.Password, y => y.MapFrom(z => z.PasswordHash));




        }
    }
}