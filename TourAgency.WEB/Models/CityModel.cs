using System.ComponentModel.DataAnnotations;

namespace TourAgency.WEB.Models
{
    public class CityModel
    {
        public int Id { get; set; }
        [Required] [MaxLength(60)] public string Name { get; set; }
        [Required] [MaxLength(60)] public string CountryName { get; set; }
    }
}