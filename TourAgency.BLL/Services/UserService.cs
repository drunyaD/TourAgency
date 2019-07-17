using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TourAgency.BLL.DTO;
using TourAgency.BLL.Interfaces;
using TourAgency.DAL.Entities;
using TourAgency.DAL.Interfaces;

namespace TourAgency.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork Database { get; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public string Create(UserDto userDto)
        {
            if (userDto.CityId.HasValue){
                var city = Database.Cities.Get(userDto.CityId.Value);

                if (city == null)
                {
                    throw new ValidationException("No city with such id exists");
                }
            }
            var userByName = Database.UserManager.FindByName(userDto.UserName);
            if (userByName != null)
            {
                throw new ValidationException("User with such name already exists");
            }
            var userByEmail = Database.UserManager.FindByEmail(userDto.Email);
            if (userByEmail != null)
            {
                throw new ValidationException("User with such email already exists");
            }
            var user = new User {
                Email = userDto.Email,
                UserName = userDto.UserName,
                CityId = userDto.CityId
            };
            Database.UserManager.Create(user, userDto.Password);
            Database.UserManager.AddToRole(user.Id, userDto.Role);
            return user.Id;
        }

        public ClaimsIdentity Authenticate(UserDto userDto)
        {
            ClaimsIdentity claim = null;
            var user = Database.UserManager.Find(userDto.UserName, userDto.Password);
            if (user != null)
                claim = Database.UserManager.CreateIdentity(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public UserDto GetUser(string userId)
        {
            var user = Database.UserManager.FindById(userId);
            if (user == null) throw new ArgumentException("no user exists with such id");
            return Mapper.Map<User, UserDto>(user);
        }

        public IEnumerable<UserDto> GetUsers()
        {
            return Mapper.Map<IQueryable<User>, IEnumerable<UserDto>>(Database.UserManager.Users);
        }

        public void ChangeRole(string userId, RoleDto roleDto)
        {
            var user = Database.UserManager.FindById(userId);
            if (user == null) throw new ArgumentException("No user exists with such id");
            var role = Database.RoleManager.FindByName(roleDto.Name);
            if (role == null) throw new ArgumentException("No role exists with such name");

            Database.UserManager.RemoveFromRole(userId,
                Database.RoleManager.FindById(user.Roles.First().RoleId).Name);

            Database.UserManager.AddToRole(userId, roleDto.Name);
        }

        public RoleDto GetRole(string roleId)
        {
            var role = Database.RoleManager.FindById(roleId);
            if (role == null)
            {
                throw new ValidationException("No role with such role");
            }
            return Mapper.Map<Role, RoleDto>(role);
        }

        public IEnumerable<RoleDto> GetRoles()
        {
            return Mapper.Map<IEnumerable<Role>, IEnumerable<RoleDto>>(Database.RoleManager.Roles);
        }

        public IEnumerable<UserDto> GetUsersByTour(int tourId)
        {
            return Mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(Database.UserManager.Users
                .Where(u => u.Tours.Where(t => t.Id == tourId).Any()));
        }
    }
}
