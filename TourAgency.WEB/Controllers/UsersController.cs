using AutoMapper;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TourAgency.BLL.DTO;
using TourAgency.BLL.Interfaces;
using TourAgency.WEB.Models;

namespace TourAgency.WEB.Controllers
{
    public class UsersController : ApiController
    {
        private IUserService Service { get; }

        private IAuthenticationManager AuthenticationManager =>
            HttpContext.Current.GetOwinContext()
                .Authentication;

        public UsersController(IUserService service)
        {
            Service = service;
        }

        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/authentication")]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage Login([FromBody] LoginModel model)
        {
            if (model == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    "LoginModel must be attached");
            }

            var userDto = new UserDto
            {
                UserName = model.UserName,
                Password = model.Password
            };
            var claim = Service.Authenticate(userDto);
            if (claim == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    "Wrong login or password");
            }

            AuthenticationManager.SignOut();
            AuthenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = true
                },
                claim);
            var user = Service.GetUserByName(model.UserName);
            return Request.CreateResponse(HttpStatusCode.OK,
                new UserModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    CityId = user.CityId,
                    Role = Service.GetRole(user.RoleId)
                        .Name
                });

        }

        [System.Web.Http.Route("api/authentication")]
        [System.Web.Http.HttpDelete]
        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage Logout()
        {
            AuthenticationManager.SignOut();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage Register([FromBody] RegisterModel registerModel)
        {

            var user = new UserDto
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email,
                Password = registerModel.Password,
                RoleId = "8df3dd85-4249-49ff-ab02-4d13de9e1a1d",
                CityId = registerModel.CityId

            };
            string userId;
            try
            {
                userId = Service.Create(user);
            }
            catch (ValidationException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.Created,
                new UserModel
                {
                    Id = userId,
                    Email = user.Email,
                    UserName = user.UserName,
                    CityId = user.CityId
                });
        }

        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("api/users/{userId}")]
        [System.Web.Http.Authorize(Roles = "administrator")]
        public HttpResponseMessage ChangeRole([FromUri] string userId,
            [FromBody] RoleModel roleModel)
        {
            try
            {
                Service.ChangeRole(userId,
                    new RoleDto
                    {
                        Name = roleModel.Name
                    });
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [System.Web.Http.Authorize(Roles = "administrator, moderator")]
        public HttpResponseMessage GetUsers()
        {
            var userDtos = Service.GetUsers();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDto, UserModel>())
                .CreateMapper();
            var users = mapper.Map<IEnumerable<UserDto>, List<UserModel>>(userDtos);
            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage GetUsersByTourId([FromUri] int tourId)
        {
            var userDtos = Service.GetUsersByTour(tourId);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDto, UserModel>())
                .CreateMapper();
            var users = mapper.Map<IEnumerable<UserDto>, List<UserModel>>(userDtos);
            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.Route("api/users/{userId}")]
        public HttpResponseMessage GetUser(string userId)
        {
            UserDto user;
            try
            {
                user = Service.GetUser(userId);
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound,
                    e.Message);
            }

            var principal = HttpContext.Current.User;
            if (user.UserName == principal.Identity.Name ||
                principal.IsInRole("administrator") ||
                principal.IsInRole("moderator"))
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                    new UserModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                        CityId = user.CityId,
                        Role = Service.GetRole(user.RoleId).Name
                    });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Service.Dispose();

            base.Dispose(disposing);
        }
    }
}