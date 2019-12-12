using System;
using System.Collections.Generic;
using TourAgency.BLL.DTO;
using TourAgency.BLL.Models;

namespace TourAgency.BLL.Interfaces
{
    public interface ITourService : IDisposable
    {
        int AddTour(TourDto tourDto);
        int AddCountry(CountryDto countryDto);
        int AddCity(CityDto cityDto);
        void DeleteTour(int tourId);
        void DeleteCity(int cityId);
        void DeleteCountry(int countryId);
        void EditCity(CityDto cityDto);
        void EditTour(TourDto tourDto);
        void AddUserToTour(int tourId, string userName);
        void DeleteUserFromTour(int tourId, string userName);
        TourDto GetTour(int tourId);
        CityDto GetCity(int cityId);
        CountryDto GetСountry(int countryId);
        IEnumerable<CountryDto> GetCountries();
        IEnumerable<CityDto> GetCities();
        IEnumerable<TourDto> GetTours();
        IEnumerable<TourDto> GetToursByOptions(TourSearchModel searchModel);
        IEnumerable<TourDto> GetToursByUser(string userName);
        void CreateImages(IEnumerable<ImageDto> imageDtos);
    }
}
