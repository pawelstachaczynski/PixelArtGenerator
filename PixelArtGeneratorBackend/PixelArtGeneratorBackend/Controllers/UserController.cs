//using EatThisAPI.Models.DTOs;
//using EatThisAPI.Models.DTOs.User;
//using YourPetAPI.Services;
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
using YourPetAPI.Services;
using YourPetAPI.Models.DTOs.User;

namespace YourPetAPI.Controllers
{
    // [ApiController]
    // [Route("api/[controller]")]
    //  [ApiController]
    //test
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        //private readonly IUserService userService;
        //public UserController(IUserService userService)
        //{
        //    this.userService = userService;
        //}
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<UserDto>> GetAllUsers()
        {

            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<UserDto> GetUser([FromRoute] int id)
        {
             _userService.GetById(id);

            return Ok();
        }

        //[HttpPost]
        //public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        //{

        //    var id = _userService.RegisterUser(dto);
        //    return Created($"/api/user/{id}", null);
        //}

        [HttpDelete("{id}")]
        public ActionResult DeleteUser([FromRoute] int id)
        {
            _userService.DeleteUser(id);
            return NotFound();
        }

        //[HttpPut("{id}")]
        //public ActionResult UpdateUser([FromRoute] int id, [FromBody] UpdateUserDto dto)
        //{
  
        //     _userService.UpdateUser(id, dto);

        //    //if (!isUpdated) return NotFound();
        //    return Ok();
        //}


    }
}