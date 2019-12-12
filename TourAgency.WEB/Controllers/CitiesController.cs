using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TourAgency.BLL.DTO;
using TourAgency.BLL.Interfaces;
using TourAgency.WEB.Models;

namespace TourAgency.WEB.Controllers
{
    [Authorize]
    public class CitiesController : ApiController
    {
        private ITourService Service { get; }

        public CitiesController(ITourService service)
        {
            Service = service;
        }
        [AllowAnonymous]
        [Route("api/cities/{cityId}")]
        public HttpResponseMessage GetCity(int cityId)
        {
            try
            {
                var cityDto = Service.GetCity(cityId);
                return Request.CreateResponse(HttpStatusCode.OK, new CityModel
                {
                    Id = cityId,
                    Name = cityDto.Name,
                    CountryName = cityDto.CountryName
                });
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }

        }
        [AllowAnonymous]
        public HttpResponseMessage GetCities()
        {

            var cityDtos = Service.GetCities();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CityDto, CityModel>()).CreateMapper();
            var cities = mapper.Map<IEnumerable<CityDto>, List<CityModel>>(cityDtos);
            return Request.CreateResponse(HttpStatusCode.OK, cities);

        }
        [Authorize(Roles = "administrator, moderator")]
        [HttpPost]
        public HttpResponseMessage CreateCity([FromBody]CityModel cityModel)
        {
            int cityId;
            try {
                cityId = Service.AddCity(new CityDto
                {
                    Name = cityModel.Name,
                    CountryName = cityModel.CountryName
                });
            }
            catch (ValidationException e) {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
            
            var response = Request.CreateResponse(HttpStatusCode.Created, new CityModel
            {
                Id = cityId,
                Name = cityModel.Name,
                CountryName = cityModel.CountryName
            });
            return response;
        }
        [Authorize(Roles = "administrator, moderator")]
        [HttpPut]
        [Route("api/cities/{cityId}")]
        public HttpResponseMessage ChangeCity([FromUri] int cityId,
                                                  [FromBody]CityModel cityModel)
        {
            try
            {
                Service.EditCity(new CityDto
                {
                    Id = cityId,
                    Name = cityModel.Name,
                    CountryName = cityModel.CountryName
                });
            }
            catch (ValidationException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
            
            return Request.CreateResponse(HttpStatusCode.OK);
             
        }
        [HttpDelete]
        [Route("api/cities/{cityId}")]
        [Authorize(Roles = "administrator, moderator")]
        public HttpResponseMessage DeleteCity(int cityId)
        {
            try
            {
                Service.DeleteCity(cityId);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
        }
    
        protected override void Dispose(bool disposing)
        {
            if (disposing) Service.Dispose();
            base.Dispose(disposing);
        }
    }
}