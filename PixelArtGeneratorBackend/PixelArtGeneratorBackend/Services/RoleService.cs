//using AutoMapper;
//using YourPetAPI.Models.DTOs.User;
//using Microsoft.AspNetCore.Authorization;
////using Microsoft.AspNetCore.JsonPatch;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Threading.Tasks;
//using YourPetAPI.Database;
//using YourPetAPI.Exceptions;
//using YourPetAPI.Models;
//using YourPetAPI.Models.Dto;
//using YourPetAPI.Repositories;

//namespace YourPetAPI.Services
//{
//    public interface IRoleService
//    {
//        Task<RoleDto> GetRoleById(int id);
//        Task<List<RoleDto>> GetRoles();
//    }

//    public class RoleService : IRoleService
//    {
//        private readonly IRoleRepository _roleRepository;

//        public RoleService(IRoleRepository roleRepository)
//        {
//            _roleRepository = roleRepository;
//        }

//        public async Task<RoleDto> GetRoleById(int id)
//        {
//            var roleDto = new RoleDto();
//            var role = await _roleRepository.GetRoleById(id);
//            roleDto.Id = role.Id;
//            roleDto.Name = role.Name;
//            return roleDto;
//        }

//        public async Task<List<RoleDto>> GetRoles()
//        {
//            var roles = await _roleRepository.GetRoles();
//            var rolesDto = new List<RoleDto>();
//            foreach(var role in roles)
//            {
//                rolesDto.Add(new RoleDto
//                {
//                    Id = role.Id,
//                    Name = role.Name
//                });
//            }
//            return rolesDto;
//        }
//    }
//}