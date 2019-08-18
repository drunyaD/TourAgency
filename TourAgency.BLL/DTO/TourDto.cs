using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TourAgency.BLL.DTO
{
    public class TourDto
    {
        [Required] public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime FinishDate { get; set; }
        [Required] public int Price { get; set; }
        [Required] public int MaxCapacity { get; set; }
        public ICollection<byte[]> Images { get; set; }
        public IList<CityDto> Cities { get; set; }
        public ICollection<string> UserNames { get; set; }
        public TourDto()
        {
            Images = new HashSet<byte[]>();
            Cities = new List<CityDto>();
            UserNames = new HashSet<string>();
        }
    }
}
