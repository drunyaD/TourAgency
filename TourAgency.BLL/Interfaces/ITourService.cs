using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourAgency.BLL.DTO;

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
        TourDto GetTour(int tourId);
        CityDto GetCity(int cityId);
        CountryDto GetСountry(int countryId);
        IEnumerable<CountryDto> GetCountries();
        IEnumerable<CityDto> GetCities();
        IEnumerable<TourDto> GetTours();
        IEnumerable<TourDto> GetToursByOptions(SearchModel searchModel);

    }
}
