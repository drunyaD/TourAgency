using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourAgency.BLL.DTO;
using TourAgency.BLL.Services;
using TourAgency.DAL.Entities;
using TourAgency.DAL.Interfaces;
using Xunit;

namespace BLLUnitTests
{
    public class TourServiceTests
    {
        [Fact]
        public void AddTourWithCorrectCitiesReturnTourId()
        {
            var uofMock = new Mock<IUnitOfWork>();
            uofMock.Setup(u => u.Tours.Create(It.IsAny<Tour>())).Callback<Tour>(t => t.Id = 7);
            uofMock.Setup(u => u.Images.Create(It.IsAny<Image>())).Verifiable();
            uofMock.Setup(u => u.Nodes.Create(It.IsAny<Node>())).Verifiable();
            uofMock.Setup(u => u.Cities.Get(2)).Returns(new City { Name = "Харьков"});
            uofMock.Setup(u => u.Cities.Get(3)).Returns(new City { Name = "Львов" });
            var service = new TourService(uofMock.Object);
            var tourDto = new TourDto
            {
                Name = "Прекрасный тур по городам",
                Description = "Увидете много нового",
                StartDate = new DateTime(2020, 7, 20),
                FinishDate = new DateTime(2020, 7, 25),
                MaxCapacity = 10,
                Price = 1000,
                Images = new HashSet<byte[]>
                {
                    new byte[] { 34, 43, 2 },
                    new byte[] { 1, 2, 3 }
                },
                Cities = new List<CityDto>
                {
                    new CityDto { Id = 2, Name = "Харьков", CountryName = "Украина" },
                    new CityDto { Id = 3, Name = "Львов", CountryName = "Украина" }
                }
            };

            int tourId = service.AddTour(tourDto);
            uofMock.Verify(m => m.Nodes.Create(It.IsAny<Node>()), Times.Exactly(2));
            uofMock.Verify(m => m.Images.Create(It.IsAny<Image>()), Times.Exactly(2));
            Assert.Equal(7, tourId);
        }
        [Fact]
        public void AddTourWithIncorrectCitiesThrowsArgumentException()
        {
            var uofMock = new Mock<IUnitOfWork>();
            uofMock.Setup(u => u.Tours.Create(It.IsAny<Tour>())).Callback<Tour>(t => t.Id = 7);
            uofMock.Setup(u => u.Images.Create(It.IsAny<Image>())).Verifiable();
            uofMock.Setup(u => u.Nodes.Create(It.IsAny<Node>())).Verifiable();
            uofMock.Setup(u => u.Cities.Get(1)).Returns(new City { Name = "Киев" });
            uofMock.Setup(u => u.Cities.Get(-10)).Returns((City)null);
            var service = new TourService(uofMock.Object);
            var tourDto = new TourDto
            {
                Name = "Прекрасный тур по городам",
                Description = "Увидете много нового",
                StartDate = new DateTime(2020, 7, 20),
                FinishDate = new DateTime(2020, 7, 25),
                MaxCapacity = 10,
                Price = 1000,
                Images = new HashSet<byte[]>
                {
                    new byte[] { 34, 43, 2 },
                    new byte[] { 1, 2, 3 }
                },
                Cities = new List<CityDto>
                {
                    new CityDto { Id = -10, Name = "Чернигов", CountryName = "Украина" },
                    new CityDto { Id = 1, Name = "Львов", CountryName = "Украина" }
                }
            };

            Action addingTour = () => service.AddTour(tourDto);

            Assert.Throws<ValidationException>(addingTour);
        }
    }
}
