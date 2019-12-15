using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TourAgency.WEB.Validation;

namespace TourAgency.WEB.Models
{
    public class TourModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [DateTimeRange("FinishDate")]
        public DateTime StartDate { get; set; }

        [Required] public DateTime FinishDate { get; set; }

        [Required]
        [Range(1, 500000)]
        public int Price { get; set; }

        [Required]
        [Range(1, 1000)]
        public int MaxCapacity { get; set; }
        public ICollection<string> Images { get; set; }

        public IList<CityModel> Cities { get; set; }

        public TourModel()
        {
            Images = new HashSet<string>();
            Cities = new List<CityModel>();
        }
    }
}