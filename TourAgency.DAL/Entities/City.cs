using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourAgency.DAL.Entities
{
    public class City
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        [ForeignKey("Country")] public int CountryId;
        [Required] public virtual Country Country { get; set; }
        public virtual ICollection<Node> Nodes { get; set; }

        public City()
        {
            Nodes = new HashSet<Node>();
        }
    }
}