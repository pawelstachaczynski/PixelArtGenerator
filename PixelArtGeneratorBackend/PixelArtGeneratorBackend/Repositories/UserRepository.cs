using YourPetAPI.Database;
using YourPetAPI.Models;
//using YourPetAPI.Models.DTOs.User;
//using YourPetAPI.Models.ViewModels;
//using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;

namespace YourPetAPI.Repositories
{
    public interface IUserRepository
    {
        Task<int> RegisterUser(User newUser);

        Task<User> GetUserByEmail(string email);

        Task<bool> EMailExist(User user);
    }

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> RegisterUser(User newUser)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser.Id;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task ChangeUserRole(int userId, int roleId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<bool>EMailExist(User user)
        {
            return await _context.Users.AnyAsync(x => x.Email == user.Email);
        }
    }
}