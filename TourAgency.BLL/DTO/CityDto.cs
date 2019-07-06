using System.ComponentModel.DataAnnotations;

namespace TourAgency.BLL.DTO
{
    public class CityDto
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string CountryName { get; set; }
    }
}
