using AutoMapper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TourAgency.BLL.DTO;
using TourAgency.BLL.Interfaces;
using TourAgency.WEB.Models;

namespace TourAgency.WEB.Controllers
{
    [Authorize]
    public class RolesController : ApiController
    {
        private IUserService Service { get; }

        public RolesController(IUserService service)
        {
            Service = service;
        }

        [Authorize(Roles = "administrator, moderator")]
        public HttpResponseMessage GetRoles()
        {
            var roleDtos = Service.GetRoles();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<RoleDto, RoleModel>())
                .CreateMapper();
            var roles = mapper.Map<IEnumerable<RoleDto>, List<RoleModel>>(roleDtos);

            return Request.CreateResponse(HttpStatusCode.OK, roles);
        }

        [Authorize(Roles = "administrator, moderator")]
        public HttpResponseMessage GetRole(string roleId)
        {
            RoleDto role;
            try
            {
                role = Service.GetRole(roleId);
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK,
                new RoleModel
                {
                    Id = role.Id,
                    Name = role.Name
                });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Service.Dispose();

            base.Dispose(disposing);
        }
    }
}