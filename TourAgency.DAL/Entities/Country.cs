using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TourAgency.DAL.Entities
{
    public class Country
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}
