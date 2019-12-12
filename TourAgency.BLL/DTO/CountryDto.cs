using System.ComponentModel.DataAnnotations;

namespace TourAgency.BLL.DTO
{
    public class CountryDto
    {
        [Required] public int Id { get; set; }
        [Required] public string Name { get; set; }
    }
}