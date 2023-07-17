
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
using YourPetAPI.Models;
using YourPetAPI.Models.DTOs.User;
using YourPetAPI.Services;


namespace YourPetAPI.Controllers
{

    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            //_roleService = roleService;
            _logger = logger;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> RegisterUser ([FromBody] RegisterUserDto registerUserDto)
        {
          await _accountService.RegisterUser(registerUserDto);
           return Ok();
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            return Ok(await _accountService.GenerateJwtToken(dto));
        }
    }

}