using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourAgency.DAL.Entities
{
    public class Image
    {
        [Key] public int Id { get; set; }
        [Required] public string Picture { get; set; }
        [Required] [ForeignKey("Tour")] public int TourId { get; set; } 
        [Required] public virtual Tour Tour { get; set; }
    }
}
