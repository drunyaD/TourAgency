using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourAgency.DAL.Entities
{
    public class Node

    {
        [Key] public int Id { get; set; }
        [ForeignKey("Tour")]
        public int TourId { get; set; }
        [ForeignKey("City")]
        public int CityId { get; set; }
        [Required] public int OrderNumber { get; set; }
        public virtual City City { get; set; }
        public virtual Tour Tour { get; set; }
    }
}
