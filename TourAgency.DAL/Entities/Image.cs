using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourAgency.DAL.Entities
{
    public class Image
    {
        [Key] public int Id { get; set; }
        [Required] public byte[] Picture { get; set; }
        [Required] [ForeignKey("Tour")] public int TourId { get; set; }
        [Required] public virtual Tour Tour { get; set; }
    }
}