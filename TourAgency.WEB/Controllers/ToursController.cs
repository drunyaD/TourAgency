using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TourAgency.BLL.DTO;
using TourAgency.BLL.Interfaces;
using TourAgency.BLL.Models;
using TourAgency.WEB.Models;

namespace TourAgency.WEB.Controllers
{
    public class ToursController : ApiController
    {
        private ITourService Service { get; }

        public ToursController(ITourService service)
        {
            Service = service;
        }
        [AllowAnonymous]
        [Route("api/tours/{tourId}")]
        public HttpResponseMessage GetTour(int tourId)
        {
            TourDto tourDto;
            try
            {
                tourDto = Service.GetTour(tourId);
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CityDto, CityModel>()).CreateMapper();
            var cities = mapper.Map<IEnumerable<CityDto>, List<CityModel>>(tourDto.Cities);

            return Request.CreateResponse(HttpStatusCode.OK, new TourModel
            {
                Id = tourId,
                Name = tourDto.Name,
                Description = tourDto.Description,
                Price = tourDto.Price,
                StartDate = tourDto.StartDate,
                FinishDate = tourDto.FinishDate,
                MaxCapacity = tourDto.MaxCapacity,
                Cities = cities,
                Images = tourDto.Images,
            });
        }
        

        [AllowAnonymous]
        public HttpResponseMessage GetTours([FromUri] TourSearchModel searchModel, 
            [FromUri] PagingModel pagingModel)
        {
            var tourDtos = Service.GetToursByOptions(searchModel);
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CityDto, CityModel>();
                cfg.CreateMap<TourDto, TourModel>();
            }).CreateMapper();
            var tours = mapper.Map<IEnumerable<TourDto>, List<TourModel>>(tourDtos);

            int TotalCount = tours.Count();
            int CurrentPage = pagingModel.PageNumber;
            int PageSize = pagingModel.PageSize;
            int TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            tours = tours.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            var previousPage = CurrentPage > 1 ? "Yes" : "No";
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage
            };

            HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            return Request.CreateResponse(HttpStatusCode.OK, tours);
        }

        [Authorize(Roles = "administrator, moderator")]
        public HttpResponseMessage GetToursByUserId(string userName)
        {
            var principal = HttpContext.Current.User;
            if (userName == principal.Identity.Name || principal.IsInRole("administrator")
                || principal.IsInRole("moderator"))
            {
                var tourDtos = Service.GetToursByUser(userName);
                var mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<CityDto, CityModel>();
                    cfg.CreateMap<TourDto, TourModel>();
                }).CreateMapper();
                var tours = mapper.Map<IEnumerable<TourDto>, List<TourModel>>(tourDtos);
                return Request.CreateResponse(HttpStatusCode.OK, tours);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

        }

        [Authorize(Roles = "administrator, moderator")]
        [HttpPost]
        public HttpResponseMessage CreateTour([FromBody]TourModel tourModel)
        {
            int tourId;
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CityModel, CityDto>()).CreateMapper();
            var cities = mapper.Map<IEnumerable<CityModel>, List<CityDto>>(tourModel.Cities);
            try
            {
                tourId = Service.AddTour(new TourDto
                {
                    Name = tourModel.Name,
                    Description = tourModel.Description,
                    Price = tourModel.Price,
                    StartDate = tourModel.StartDate,
                    FinishDate = tourModel.FinishDate,
                    Images = tourModel.Images,
                    MaxCapacity = tourModel.MaxCapacity,
                    Cities = cities
                });
            }
            catch (ValidationException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
            tourModel.Id = tourId;
            var response = Request.CreateResponse(HttpStatusCode.Created, tourModel);
            return response;
        }
        [Authorize(Roles = "administrator, moderator")]
        [HttpPut]
        [Route("api/tours/{tourId}")]
        public HttpResponseMessage ChangeTour([FromUri]int tourId,
                                                  [FromBody]TourModel tourModel)
        {

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CityModel, CityDto>()).CreateMapper();
            var cities = mapper.Map<IEnumerable<CityModel>, List<CityDto>>(tourModel.Cities);
            try
            {
                Service.EditTour(new TourDto
                {
                    Id = tourId,
                    Name = tourModel.Name,
                    Description = tourModel.Description,
                    Price = tourModel.Price,
                    StartDate = tourModel.StartDate,
                    FinishDate = tourModel.FinishDate,
                    Images = tourModel.Images,
                    MaxCapacity = tourModel.MaxCapacity,
                    Cities = cities,
                });
            }
            catch (ValidationException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK);

        }

        
        [Authorize(Roles = "user")]
        [HttpPost]
        [Route("api/tours/{tourId}/users")]
        public HttpResponseMessage AddUserToTour([FromUri]int tourId)
        {
            try
            {
                Service.AddUserToTour(tourId, HttpContext.Current.User.Identity.Name);
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK);

        }

        [HttpDelete]
        [Route("api/tours/{tourId}")]
        [Authorize(Roles = "administrator, moderator")]
        public HttpResponseMessage DeleteTour(int tourId)
        {
            try
            {
                Service.DeleteTour(tourId);
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, e.Message);
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing) Service.Dispose();
            base.Dispose(disposing);
        }
    }
}