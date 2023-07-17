using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PixelArtGeneratorBackend.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YourPetAPI.Database;
using YourPetAPI.Exceptions;
using YourPetAPI.Models;
using YourPetAPI.Models.DTOs.User;
using YourPetAPI.Repositories;
using YourPetBackend.Validators;

namespace YourPetAPI.Services
{

    public interface IAccountService
    {
        Task<AuthToken> GenerateJwtToken(LoginDto dto);
        Task RegisterUser(RegisterUserDto registerUserDto);
    }

    public class AccountService : IAccountService
    {
        //private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
       // private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IUserValidator _userValidator;

        public AccountService(IMapper mapper, IUserRepository userRepository, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IUserValidator userValidator)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _userValidator = userValidator;
        }

        public async Task<AuthToken> GenerateJwtToken(LoginDto dto)
        {
            var user = await _userRepository.GetUserByEmail(dto.Email);
            //if(user is null)
            //{
            //    throw new Exception("Nie istnieje taki u¿ytkownik");
            //};

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if(verificationResult == PasswordVerificationResult.Failed)
            {
                user = null;
                throw new Exception("B³¹d logowania");
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
             //   new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
               // new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                new Claim("RegisterDate", user.RegisterDate.Value.ToString("dd-mm-yyyy"))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer, claims, expires: expires, signingCredentials: cred);
            var tokenHandler = new JwtSecurityTokenHandler();
      
            try
            {
                 var result = tokenHandler.WriteToken(token);
                var authToken = new AuthToken
                {
                    UserId = user.Id,
                    Email = user.Email,
                    //RoleId = user.RoleId,
                     Token = result,
                    TokenExpirationDate = expires
                };
                return authToken;
            }
            catch (Exception ex)
            {
                // Obs³uga b³êdu.
                Console.WriteLine($"Wyst¹pi³ b³¹d podczas pisania tokena JWT: {ex.Message}");
                return null;
                // Mo¿na te¿ u¿yæ loggera zamiast Console.WriteLine().
            }

        }

        public async Task RegisterUser(RegisterUserDto registerUserDto)
        {
            var newUser = _mapper.Map<User>(registerUserDto);
            newUser.PasswordHash = registerUserDto.Password;
            newUser.RegisterDate = DateTime.UtcNow;
            newUser.IsActive = false;
            await _userValidator.RegisterUserValidate(newUser);
          
           
           // newUser.RoleId = 3;
            var hashedPassword = _passwordHasher.HashPassword(newUser, registerUserDto.Password);
            newUser.PasswordHash = hashedPassword;
            
            await _userRepository.RegisterUser(newUser);
        }
    }
}
