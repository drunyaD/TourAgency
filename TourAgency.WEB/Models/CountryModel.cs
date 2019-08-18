using System.ComponentModel.DataAnnotations;

namespace TourAgency.WEB.Models
{
    public class CountryModel
    {
        public int Id { get; set; }
        [Required] [MaxLength(60)] public string Name { get; set; }
    }
}