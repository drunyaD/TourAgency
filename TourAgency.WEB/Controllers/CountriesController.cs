using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TourAgency.BLL.DTO;
using TourAgency.BLL.Interfaces;
using TourAgency.WEB.Models;

namespace TourAgency.WEB.Controllers
{
    public class CountriesController : ApiController
    {
        private ITourService Service { get; }

        public CountriesController(ITourService service)
        {
            Service = service;
        }

        [AllowAnonymous]
        [Route("api/countries/{countryId}")]
        public HttpResponseMessage GetCountry(int countryId)
        {
            try
            {
                var countryDto = Service.GetСountry(countryId);
                return Request.CreateResponse(HttpStatusCode.OK,
                    new CountryModel
                    {
                        Id = countryId,
                        Name = countryDto.Name
                    });
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound,
                    e.Message);
            }

        }

        [AllowAnonymous]
        public HttpResponseMessage GetCountries()
        {

            var countryDtos = Service.GetCountries();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CountryDto, CountryModel>()).CreateMapper();
            var countries = mapper.Map<IEnumerable<CountryDto>, List<CountryModel>>(countryDtos);
            return Request.CreateResponse(HttpStatusCode.OK,
                countries);

        }

        [Authorize(Roles = "administrator, moderator")]
        [HttpPost]
        public HttpResponseMessage CreateCountry([FromBody] CountryModel countryModel)
        {
            int countryId;
            try
            {
                countryId = Service.AddCountry(new CountryDto
                {
                    Name = countryModel.Name,
                });
            }
            catch (ValidationException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    e.Message);
            }

            var response = Request.CreateResponse(HttpStatusCode.Created,
                new CountryModel
                {
                    Id = countryId,
                    Name = countryModel.Name,
                });
            return response;
        }

        [HttpDelete]
        [Route("api/countries/{countryId}")]
        [Authorize(Roles = "administrator, moderator")]
        public HttpResponseMessage DeleteCountry(int countryId)
        {
            try
            {
                Service.DeleteCity(countryId);
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound,
                    e.Message);
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