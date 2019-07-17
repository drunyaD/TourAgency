using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourAgency.BLL.DTO;
using TourAgency.BLL.Interfaces;
using TourAgency.BLL.Models;
using TourAgency.DAL.Entities;
using TourAgency.DAL.Interfaces;

namespace TourAgency.BLL.Services
{
    public class TourService : ITourService
    {
        private IUnitOfWork Database { get; }
        public TourService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public int AddCity(CityDto cityDto)
        {
            var countries = Database.Countries.Find(country => country.Name == cityDto.CountryName);
            if (countries == null || !countries.Any())
            {
                throw new ValidationException("No country with such name");
            }
            City city = new City
            {
                Name = cityDto.Name,
                CountryId = countries.First().Id,
                Country = countries.First()
            };
            Database.Cities.Create(city);
            Database.Save();
            return city.Id;
        }

        public int AddCountry(CountryDto countryDto)
        {
            var countries = Database.Countries.Find(c
                => c.Name == countryDto.Name);

            if (countries != null && countries.Any())
            {
                throw new ValidationException("Such country already exists");
            }
            Country country = new Country
            {
                Name = countryDto.Name
            };
            Database.Countries.Create(country);
            Database.Save();
            return country.Id;
        }
        public int AddTour(TourDto tourDto)
        {

            Tour tour = new Tour
            {
                Name = tourDto.Name,
                Description = tourDto.Description,
                StartDate = tourDto.StartDate,
                FinishDate = tourDto.FinishDate,
                MaxCapacity = tourDto.MaxCapacity,
                Price = tourDto.Price
            };
            foreach (var picture in tourDto.Images)
            {
                var image = new Image { Picture = picture, Tour = tour };
                Database.Images.Create(image);
                tour.Images.Add(image);
            }

            for (int i = 0; i < tourDto.Cities.Count; i++)
            {
                var cityDto = tourDto.Cities[i];
                var city = Database.Cities.Get(cityDto.Id);
                if (city == null || city.Name != cityDto.Name)
                {
                    throw new ValidationException("No such city exists");
                }
                Node node = new Node
                {
                    OrderNumber = i,
                    CityId = cityDto.Id,
                    City = city,
                    Tour = tour
                };
                Database.Nodes.Create(node);
                tour.Nodes.Add(node);

            }
            Database.Tours.Create(tour);
            Database.Save();
            return tour.Id;
        }

        public void DeleteCity(int cityId)
        {
            City city = Database.Cities.Get(cityId);
            if (city == null)
            {
                throw new ArgumentException("No city with such id exists");
            }
            Database.Cities.Delete(city);
            Database.Save();
        }

        public void DeleteCountry(int countryId)
        {
            Country country = Database.Countries.Get(countryId);
            if (country == null)
            {
                throw new ArgumentException("No country with such id exists");
            }
            Database.Countries.Delete(country);
            Database.Save();
        }

        public void DeleteTour(int tourId)
        {
            Tour tour = Database.Tours.Get(tourId);
            if (tour == null)
            {
                throw new ArgumentException("No tour with such id exists");
            }
            Database.Tours.Delete(tour);
            Database.Save();
        }


        public void EditCity(CityDto cityDto)
        {
            City city = Database.Cities.Get(cityDto.Id);
            var countries = Database.Countries.Find(c => c.Name == cityDto.CountryName);
            if (city == null)
            {
                throw new ValidationException("No city with such id");
            }
            if (countries == null || !countries.Any())
            {
                throw new ValidationException("No country with such name");
            }
            var country = countries.FirstOrDefault();
            city.Name = cityDto.Name;
            city.Country = country;
            Database.Cities.Update(city);
            Database.Save();
        }

        public void EditTour(TourDto tourDto)
        {
            var tour = Database.Tours.Get(tourDto.Id);
            if (tour == null)
            {
                throw new ValidationException("No tour with such id exists");
            }

            if (tourDto.MaxCapacity < tourDto.UserNames.Count())
            {
                throw new ValidationException("Too much people registred");
            }

            tour.Name = tourDto.Name;
            tour.Description = tourDto.Description;
            tour.StartDate = tourDto.StartDate;
            tour.FinishDate = tourDto.FinishDate;
            tour.MaxCapacity = tourDto.MaxCapacity;
            tour.Price = tourDto.Price;

            foreach (var i in Database.Images.Find(i => i.TourId == tourDto.Id))
            {
                Database.Images.Delete(i);
            }

            foreach (var n in Database.Nodes.Find(n => n.TourId == tourDto.Id))
            {
                Database.Nodes.Delete(n);
            }

            foreach (var picture in tourDto.Images)
            {
                var image = new Image { Picture = picture, Tour = tour };
                Database.Images.Create(image);
                tour.Images.Add(image);
            }

            for (int i = 0; i < tourDto.Cities.Count; i++)
            {
                var cityDto = tourDto.Cities[i];
                var city = Database.Cities.Get(cityDto.Id);
                if (city == null || city.Name != cityDto.Name)
                {
                    throw new ValidationException("No such city exists");
                }
                Node node = new Node
                {
                    OrderNumber = i,
                    CityId = cityDto.Id,
                    City = city,
                    Tour = tour,
                };
                Database.Nodes.Create(node);
                tour.Nodes.Add(node);
            }
            Database.Tours.Update(tour);
            Database.Save();
        }

        public IEnumerable<CityDto> GetCities()
        {
            return Mapper.Map<IEnumerable<City>, IEnumerable<CityDto>>(Database.Cities.GetAll());
        }

        public CityDto GetCity(int cityId)
        {
            var city = Database.Cities.Get(cityId);
            if (city == null)
            {
                throw new ArgumentException("No city with such id");
            }
            return Mapper.Map<City, CityDto>(city);
        }

        public IEnumerable<CountryDto> GetCountries()
        {
            return Mapper.Map<IEnumerable<Country>, IEnumerable<CountryDto>>(Database.Countries.GetAll());
        }

        public TourDto GetTour(int tourId)
        {
            var tour = Database.Tours.Get(tourId);
            if (tour == null)
            {
                throw new ArgumentException("No tour with such id");
            }
            return Mapper.Map<Tour, TourDto>(tour);
        }

        public IEnumerable<TourDto> GetTours()
        {
            return Mapper.Map<IEnumerable<Tour>, IEnumerable<TourDto>>(Database.Tours.GetAll());
        }

        public IEnumerable<TourDto> GetToursByOptions(SearchModel searchModel)
        {
            var tours = Database.Tours.GetAll();
            if (searchModel.MinPrice.HasValue)
                tours = tours.Where(t => t.Price >= searchModel.MinPrice);
            if (searchModel.MaxPrice.HasValue)
                tours = tours.Where(t => t.Price <= searchModel.MaxPrice);
            if (searchModel.MinTime.HasValue)
                tours = tours.Where(t => t.StartDate >= searchModel.MinTime);
            if (searchModel.MaxTime.HasValue)
                tours = tours.Where(t => t.FinishDate <= searchModel.MaxTime);
            if (searchModel.NotFullOnly.HasValue && searchModel.NotFullOnly == true)
                tours = tours.Where(t => t.Users.Count < t.MaxCapacity);
            if (!string.IsNullOrEmpty(searchModel.SearchString))
                tours = tours.Where(t => t.Name.IndexOf(searchModel.SearchString) != -1
                || t.Description.IndexOf(searchModel.SearchString) != -1);
            if (searchModel.CountryId.HasValue)
                tours = tours.Where(t => t.Nodes.Where(n => n.City.Country.Id == searchModel.CountryId).Any());
            return Mapper.Map<IEnumerable<Tour>, IEnumerable<TourDto>>(tours);
        }

        public CountryDto GetСountry(int countryId)
        {
            var country = Database.Countries.Get(countryId);
            if (country == null)
            {
                throw new ArgumentException("No country with such id exists");
            }
            return Mapper.Map<Country, CountryDto>(country);
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public IEnumerable<TourDto> GetToursByUser(string userName)
        {
            return Mapper.Map<IEnumerable<Tour>,IEnumerable<TourDto>>(
                Database.Tours.Find(t => t.Users.Where(u => u.UserName == userName).Any()));
        }

        public void AddUserToTour(int tourId, string userName)
        {
            var tour = Database.Tours.Get(tourId);
            if (tour == null)
            {
                throw new ArgumentException ("No tour with such id exists");
            }
            var userTask = Database.UserManager.FindByNameAsync(userName);
            tour.Users.Add(userTask.Result);
            Database.Tours.Update(tour);
            Database.Save();
        }
    }
}
