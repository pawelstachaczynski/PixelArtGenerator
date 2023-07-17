using AutoMapper;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourPetAPI.Database;
using YourPetAPI.Exceptions;
using YourPetAPI.Models;
using YourPetAPI.Models.DTOs.User;

namespace YourPetAPI.Services
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetAll();
        UserDto GetById(int id);
        int RegisterUser(RegisterUserDto dto);
        void DeleteUser(int id);
        //void UpdateUser(int id, UpdateUserDto dto);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UserService(AppDbContext dbContext, IMapper mapper, ILogger<UserService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public UserDto GetById(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user is null) throw new CustomException("User not found");
                var result = _mapper.Map<UserDto>(user);
            return result;
        }

        public IEnumerable<UserDto> GetAll()
        {
            var users = _dbContext
                .Users
                //.Include(r => r.Role)
                .ToList();

            var allusers = _mapper.Map<List<UserDto>>(users);
            return allusers;
        }

        public int RegisterUser(RegisterUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
           
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user.Id;
        }

        public void DeleteUser(int id)
        {
            _logger.LogWarning($"User with id: {id} DELETE action invoked");

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user is null) throw new CustomException("User not found");

            _dbContext.Remove(user);
            _dbContext.SaveChanges();

        }

    //    public void UpdateUser(int id, UpdateUserDto dto)
    //    {
    //        var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);

    //        if (user is null)
    //            throw new CustomException("User not found");


    //        _dbContext.SaveChanges();


    //}
    }
}