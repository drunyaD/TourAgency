using AutoMapper;
using Microsoft.TeamFoundation.WorkItemTracking.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
                return Request.CreateResponse(HttpStatusCode.NotFound,
                    e.Message);
            }

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CityDto, CityModel>()).CreateMapper();
            var cities = mapper.Map<IEnumerable<CityDto>, List<CityModel>>(tourDto.Cities);

            return Request.CreateResponse(HttpStatusCode.OK,
                new TourModel
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

            int totalCount = tours.Count();
            int currentPage = pagingModel.PageNumber;
            int pageSize = pagingModel.PageSize;
            int totalPages = (int) Math.Ceiling(totalCount / (double) pageSize);
            tours = tours.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            var previousPage = currentPage > 1 ? "Yes" : "No";
            var nextPage = currentPage < totalPages ? "Yes" : "No";

            var paginationMetadata = new
            {
                totalCount,
                pageSize,
                currentPage,
                totalPages,
                previousPage,
                nextPage
            };

            HttpContext.Current.Response.Headers.Add("Paging-Headers",
                JsonConvert.SerializeObject(paginationMetadata));
            return Request.CreateResponse(HttpStatusCode.OK,
                tours);
        }

        [Authorize]
        public HttpResponseMessage GetToursByUserName(string userName)
        {
            var principal = HttpContext.Current.User;
            if (userName == principal.Identity.Name ||
                principal.IsInRole("administrator") ||
                principal.IsInRole("moderator"))
            {
                IEnumerable<TourDto> tourDtos;
                try
                {
                    tourDtos = Service.GetToursByUser(userName);
                }
                catch (ArgumentException e)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                        e.Message);
                }

                var mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<CityDto, CityModel>();
                    cfg.CreateMap<TourDto, TourModel>();
                }).CreateMapper();
                var tours = mapper.Map<IEnumerable<TourDto>, List<TourModel>>(tourDtos);
                return Request.CreateResponse(HttpStatusCode.OK,
                    tours);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

        }

        [Authorize(Roles = "administrator, moderator")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateTour()

        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }

            var filesReadToProvider =  await Request.Content.ReadAsMultipartAsync();

            var json = filesReadToProvider.Contents[0].ReadAsStringAsync().Result;
            TourModel tourModel = JsonConvert.DeserializeObject<TourModel>(json);
           
                
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
                    MaxCapacity = tourModel.MaxCapacity,
                    Cities = cities
                });
            }
            catch (ValidationException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    e.Message);
            }

            tourModel.Id = tourId;

            List<ImageDto> imageDtos = new List<ImageDto>();
            foreach (var pict in filesReadToProvider.Contents.Skip(1))
            {
                imageDtos.Add(new ImageDto
                {
                    TourId = tourModel.Id,
                    File = pict.ReadAsStreamAsync().Result,
                    FileName = pict.Headers.ContentDisposition.FileName.Replace("\"", "")
                });
            }
            Service.CreateImages(imageDtos);
            var response = Request.CreateResponse(HttpStatusCode.Created, tourModel);
            return response;
        }

        [Authorize(Roles = "administrator, moderator")]
        [HttpPut]
        [Route("api/tours/{tourId}")]

        public async Task<HttpResponseMessage> ChangeTour([FromUri]int tourId)

        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }

            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();

            var json = filesReadToProvider.Contents[0].ReadAsStringAsync().Result;
            TourModel tourModel = JsonConvert.DeserializeObject<TourModel>(json);

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
                    MaxCapacity = tourModel.MaxCapacity,
                    Cities = cities,
                });
            }
            catch (ValidationException e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    e.Message);
            }
            List<ImageDto> imageDtos = new List<ImageDto>();
            foreach (var pict in filesReadToProvider.Contents.Skip(1))
            {
                imageDtos.Add(new ImageDto
                {
                    TourId = tourModel.Id,
                    File = pict.ReadAsStreamAsync().Result,
                    FileName = pict.Headers.ContentDisposition.FileName.Replace("\"", "")
                });
            }
            Service.CreateImages(imageDtos);
            return Request.CreateResponse(HttpStatusCode.OK);

        }



        [HttpPost]
        [Route("api/tours/{tourId}/users")]
        public HttpResponseMessage AddUserToTour([FromUri] int tourId)
        {
            try
            {
                Service.AddUserToTour(tourId,
                    HttpContext.Current.User.Identity.Name);
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound,
                    e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Authorize(Roles = "user")]
        [Route("api/tours/{tourId}/users")]
        public HttpResponseMessage DeleteUserFromTour([FromUri] int tourId)
        {
            try
            {
                Service.DeleteUserFromTour(tourId,
                    HttpContext.Current.User.Identity.Name);
            }
            catch (ArgumentException e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound,
                    e.Message);
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
                return Request.CreateResponse(HttpStatusCode.NotFound,
                    e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
                Service.Dispose();
            base.Dispose(disposing);
        }
    }
}